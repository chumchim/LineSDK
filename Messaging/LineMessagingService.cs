using System.Net.Http.Json;
using System.Text.Json;
using LineSDK.Messages;
using LineSDK.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LineSDK.Messaging;

/// <summary>
/// LINE Messaging Service Implementation
/// </summary>
public class LineMessagingService : ILineMessaging
{
    private const string BaseUrl = "https://api.line.me/v2/bot";

    private readonly HttpClient _httpClient;
    private readonly LineClientOptions _options;
    private readonly ILogger<LineMessagingService>? _logger;

    public LineMessagingService(
        HttpClient httpClient,
        IOptions<LineClientOptions> options,
        ILogger<LineMessagingService>? logger = null)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.ChannelAccessToken);
    }

    #region Push

    public Task PushAsync(string message, string to, CancellationToken ct = default)
        => PushAsync(new TextMessage(message), to, ct);

    public Task PushAsync(ILineMessage message, string to, CancellationToken ct = default)
        => PushAsync(new[] { message }, to, ct);

    public async Task PushAsync(IEnumerable<ILineMessage> messages, string to, CancellationToken ct = default)
    {
        var payload = new
        {
            to,
            messages = messages.Select(m => m.ToJson())
        };

        await SendAsync($"{BaseUrl}/message/push", payload, ct);
    }

    #endregion

    #region Reply

    public Task ReplyAsync(string message, string replyToken, CancellationToken ct = default)
        => ReplyAsync(new TextMessage(message), replyToken, ct);

    public Task ReplyAsync(ILineMessage message, string replyToken, CancellationToken ct = default)
        => ReplyAsync(new[] { message }, replyToken, ct);

    public async Task ReplyAsync(IEnumerable<ILineMessage> messages, string replyToken, CancellationToken ct = default)
    {
        var payload = new
        {
            replyToken,
            messages = messages.Select(m => m.ToJson())
        };

        await SendAsync($"{BaseUrl}/message/reply", payload, ct);
    }

    #endregion

    #region Multicast

    public Task MulticastAsync(string message, IEnumerable<string> userIds, CancellationToken ct = default)
        => MulticastAsync(new TextMessage(message), userIds, ct);

    public async Task MulticastAsync(ILineMessage message, IEnumerable<string> userIds, CancellationToken ct = default)
    {
        var payload = new
        {
            to = userIds,
            messages = new[] { message.ToJson() }
        };

        await SendAsync($"{BaseUrl}/message/multicast", payload, ct);
    }

    #endregion

    #region Broadcast

    public Task BroadcastAsync(string message, CancellationToken ct = default)
        => BroadcastAsync(new TextMessage(message), ct);

    public async Task BroadcastAsync(ILineMessage message, CancellationToken ct = default)
    {
        var payload = new
        {
            messages = new[] { message.ToJson() }
        };

        await SendAsync($"{BaseUrl}/message/broadcast", payload, ct);
    }

    #endregion

    #region Private Methods

    private async Task SendAsync(string url, object payload, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(url, payload, ct);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger?.LogError("LINE API Error: {StatusCode} - {Error}", response.StatusCode, error);
                throw new LineApiException(response.StatusCode, error);
            }
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "LINE API Request failed");
            throw;
        }
    }

    #endregion
}

/// <summary>
/// LINE API Exception
/// </summary>
public class LineApiException : Exception
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public string ErrorResponse { get; }

    public LineApiException(System.Net.HttpStatusCode statusCode, string errorResponse)
        : base($"LINE API Error: {statusCode} - {errorResponse}")
    {
        StatusCode = statusCode;
        ErrorResponse = errorResponse;
    }
}
