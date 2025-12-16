using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using LineSDK.Options;
using LineSDK.Token;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LineSDK.RichMenu;

/// <summary>
/// Implementation ของ ILineRichMenu
/// </summary>
public class LineRichMenuService : ILineRichMenu
{
    private readonly HttpClient _httpClient;
    private readonly ILineTokenProvider _tokenProvider;
    private readonly ILogger<LineRichMenuService> _logger;
    private readonly JsonSerializerOptions _jsonOptions;

    private const string BaseUrl = "https://api.line.me/v2/bot/richmenu";
    private const string UserBaseUrl = "https://api.line.me/v2/bot/user";

    public LineRichMenuService(
        HttpClient httpClient,
        IOptions<LineClientOptions> options,
        ILineTokenProvider tokenProvider,
        ILogger<LineRichMenuService> logger)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;
        _logger = logger;

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    #region Create/Delete

    public async Task<string> CreateAsync(RichMenuRequest request, CancellationToken ct = default)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await PostAsync(BaseUrl, content, ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CreateRichMenuResponse>(ct);
        _logger.LogInformation("Created Rich Menu: {RichMenuId}", result?.RichMenuId);

        return result?.RichMenuId ?? throw new InvalidOperationException("Failed to create Rich Menu");
    }

    public async Task UploadImageAsync(string richMenuId, byte[] imageData, string contentType = "image/png", CancellationToken ct = default)
    {
        var url = $"https://api-data.line.me/v2/bot/richmenu/{richMenuId}/content";
        var content = new ByteArrayContent(imageData);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

        var response = await PostAsync(url, content, ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Uploaded image for Rich Menu: {RichMenuId}", richMenuId);
    }

    public async Task DeleteAsync(string richMenuId, CancellationToken ct = default)
    {
        var response = await DeleteRequestAsync($"{BaseUrl}/{richMenuId}", ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Deleted Rich Menu: {RichMenuId}", richMenuId);
    }

    #endregion

    #region Link/Unlink

    public async Task LinkToUserAsync(string richMenuId, string userId, CancellationToken ct = default)
    {
        var response = await PostAsync(
            $"{UserBaseUrl}/{userId}/richmenu/{richMenuId}",
            null,
            ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Linked Rich Menu {RichMenuId} to user {UserId}", richMenuId, userId);
    }

    public async Task UnlinkFromUserAsync(string userId, CancellationToken ct = default)
    {
        var response = await DeleteRequestAsync(
            $"{UserBaseUrl}/{userId}/richmenu",
            ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Unlinked Rich Menu from user {UserId}", userId);
    }

    public async Task LinkToMultipleUsersAsync(string richMenuId, IEnumerable<string> userIds, CancellationToken ct = default)
    {
        var request = new { richMenuId, userIds = userIds.ToList() };
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await PostAsync(
            "https://api.line.me/v2/bot/richmenu/bulk/link",
            content,
            ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Linked Rich Menu {RichMenuId} to {Count} users", richMenuId, userIds.Count());
    }

    public async Task UnlinkFromMultipleUsersAsync(IEnumerable<string> userIds, CancellationToken ct = default)
    {
        var request = new { userIds = userIds.ToList() };
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await PostAsync(
            "https://api.line.me/v2/bot/richmenu/bulk/unlink",
            content,
            ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Unlinked Rich Menu from {Count} users", userIds.Count());
    }

    #endregion

    #region Default

    public async Task SetDefaultAsync(string richMenuId, CancellationToken ct = default)
    {
        var response = await PostAsync(
            $"{UserBaseUrl}/all/richmenu/{richMenuId}",
            null,
            ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Set default Rich Menu: {RichMenuId}", richMenuId);
    }

    public async Task<string?> GetDefaultIdAsync(CancellationToken ct = default)
    {
        try
        {
            var response = await SendGetAsync(
                "https://api.line.me/v2/bot/user/all/richmenu",
                ct);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetDefaultRichMenuResponse>(ct);
            return result?.RichMenuId;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task CancelDefaultAsync(CancellationToken ct = default)
    {
        var response = await DeleteRequestAsync(
            "https://api.line.me/v2/bot/user/all/richmenu",
            ct);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Cancelled default Rich Menu");
    }

    #endregion

    #region Query

    public async Task<string?> GetUserRichMenuIdAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var response = await SendGetAsync(
                $"{UserBaseUrl}/{userId}/richmenu",
                ct);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<GetUserRichMenuResponse>(ct);
            return result?.RichMenuId;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<IEnumerable<RichMenuInfo>> GetAllAsync(CancellationToken ct = default)
    {
        var response = await SendGetAsync($"{BaseUrl}/list", ct);
        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<GetAllRichMenusResponse>(_jsonOptions, ct);
        return result?.RichMenus ?? Enumerable.Empty<RichMenuInfo>();
    }

    public async Task<RichMenuInfo?> GetAsync(string richMenuId, CancellationToken ct = default)
    {
        try
        {
            var response = await SendGetAsync($"{BaseUrl}/{richMenuId}", ct);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<RichMenuInfo>(_jsonOptions, ct);
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    #endregion

    #region Private HTTP Helper Methods

    private async Task<HttpResponseMessage> SendGetAsync(string url, CancellationToken ct)
    {
        var token = await _tokenProvider.GetTokenAsync(ct);

        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await _httpClient.SendAsync(request, ct);
    }

    private async Task<HttpResponseMessage> PostAsync(string url, HttpContent? content, CancellationToken ct)
    {
        var token = await _tokenProvider.GetTokenAsync(ct);

        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        request.Content = content;

        return await _httpClient.SendAsync(request, ct);
    }

    private async Task<HttpResponseMessage> DeleteRequestAsync(string url, CancellationToken ct)
    {
        var token = await _tokenProvider.GetTokenAsync(ct);

        using var request = new HttpRequestMessage(HttpMethod.Delete, url);
        request.Headers.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        return await _httpClient.SendAsync(request, ct);
    }

    #endregion

    #region Response DTOs

    private class CreateRichMenuResponse
    {
        public string RichMenuId { get; set; } = "";
    }

    private class GetDefaultRichMenuResponse
    {
        public string RichMenuId { get; set; } = "";
    }

    private class GetUserRichMenuResponse
    {
        public string RichMenuId { get; set; } = "";
    }

    private class GetAllRichMenusResponse
    {
        [JsonPropertyName("richmenus")]
        public List<RichMenuInfo> RichMenus { get; set; } = new();
    }

    #endregion
}
