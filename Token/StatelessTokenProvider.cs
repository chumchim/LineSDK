using System.Net.Http.Json;
using System.Text.Json.Serialization;
using LineSDK.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LineSDK.Token;

/// <summary>
/// Stateless token provider - issues and auto-refreshes JWT tokens
/// Recommended for production environments
/// </summary>
public class StatelessTokenProvider : ILineTokenProvider
{
    private readonly HttpClient _httpClient;
    private readonly LineClientOptions _options;
    private readonly ILogger<StatelessTokenProvider>? _logger;
    private readonly SemaphoreSlim _refreshLock = new(1, 1);

    private string? _cachedToken;
    private DateTime _tokenExpiry = DateTime.MinValue;

    public StatelessTokenProvider(
        HttpClient httpClient,
        IOptions<LineClientOptions> options,
        ILogger<StatelessTokenProvider>? logger = null)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        if (string.IsNullOrEmpty(_options.ChannelId))
        {
            throw new InvalidOperationException(
                "ChannelId is required for Stateless token mode. " +
                "Please configure Line:ChannelId in your settings or environment variables.");
        }

        if (string.IsNullOrEmpty(_options.ChannelSecret))
        {
            throw new InvalidOperationException(
                "ChannelSecret is required for Stateless token mode. " +
                "Please configure Line:ChannelSecret in your settings or environment variables.");
        }
    }

    /// <inheritdoc />
    public async Task<string> GetTokenAsync(CancellationToken ct = default)
    {
        // Return cached token if still valid
        if (IsTokenValid && !string.IsNullOrEmpty(_cachedToken))
        {
            return _cachedToken;
        }

        // Refresh token
        await RefreshTokenAsync(ct);

        if (string.IsNullOrEmpty(_cachedToken))
        {
            throw new InvalidOperationException("Failed to obtain LINE access token");
        }

        return _cachedToken;
    }

    /// <inheritdoc />
    public async Task RefreshTokenAsync(CancellationToken ct = default)
    {
        // Use lock to prevent concurrent token refresh
        await _refreshLock.WaitAsync(ct);
        try
        {
            // Double-check after acquiring lock
            if (IsTokenValid && !string.IsNullOrEmpty(_cachedToken))
            {
                return;
            }

            _logger?.LogDebug("Refreshing LINE stateless token...");

            var tokenResponse = await IssueTokenAsync(ct);

            _cachedToken = tokenResponse.AccessToken;
            _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn)
                .AddMinutes(-_options.TokenRefreshBufferMinutes); // Refresh before actual expiry

            _logger?.LogInformation(
                "LINE stateless token refreshed successfully. Expires at {Expiry} UTC",
                _tokenExpiry);
        }
        finally
        {
            _refreshLock.Release();
        }
    }

    /// <inheritdoc />
    public bool IsTokenValid => DateTime.UtcNow < _tokenExpiry;

    /// <summary>
    /// Issue a new stateless channel access token from LINE API
    /// </summary>
    private async Task<LineTokenResponse> IssueTokenAsync(CancellationToken ct)
    {
        var url = $"{_options.OAuthBaseUrl}/oauth2/v3/token";

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "client_credentials",
            ["client_id"] = _options.ChannelId,
            ["client_secret"] = _options.ChannelSecret
        });

        try
        {
            var response = await _httpClient.PostAsync(url, content, ct);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync(ct);
                _logger?.LogError(
                    "Failed to issue LINE token: {StatusCode} - {Error}",
                    response.StatusCode, error);
                throw new LineTokenException(response.StatusCode, error);
            }

            var tokenResponse = await response.Content.ReadFromJsonAsync<LineTokenResponse>(ct);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
            {
                throw new LineTokenException(
                    System.Net.HttpStatusCode.OK,
                    "Invalid token response from LINE API");
            }

            return tokenResponse;
        }
        catch (HttpRequestException ex)
        {
            _logger?.LogError(ex, "HTTP error while issuing LINE token");
            throw;
        }
    }
}

/// <summary>
/// LINE OAuth token response
/// </summary>
internal class LineTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = "";

    [JsonPropertyName("token_type")]
    public string TokenType { get; set; } = "";

    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonPropertyName("key_id")]
    public string? KeyId { get; set; }
}

/// <summary>
/// LINE Token Exception
/// </summary>
public class LineTokenException : Exception
{
    public System.Net.HttpStatusCode StatusCode { get; }
    public string ErrorResponse { get; }

    public LineTokenException(System.Net.HttpStatusCode statusCode, string errorResponse)
        : base($"LINE Token Error: {statusCode} - {errorResponse}")
    {
        StatusCode = statusCode;
        ErrorResponse = errorResponse;
    }
}
