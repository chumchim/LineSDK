using System.Net.Http.Json;
using LineSDK.Messaging;
using LineSDK.Models;
using LineSDK.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LineSDK.Profile;

/// <summary>
/// LINE Profile Service Implementation
/// </summary>
public class LineProfileService : ILineProfile
{
    private const string BaseUrl = "https://api.line.me/v2/bot";

    private readonly HttpClient _httpClient;
    private readonly LineClientOptions _options;
    private readonly ILogger<LineProfileService>? _logger;

    public LineProfileService(
        HttpClient httpClient,
        IOptions<LineClientOptions> options,
        ILogger<LineProfileService>? logger = null)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;

        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _options.ChannelAccessToken);
    }

    #region User Profile

    public async Task<LineUserProfile?> GetUserProfileAsync(string userId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/profile/{userId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to get profile for user {UserId}: {StatusCode}", userId, response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LineUserProfile>(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting profile for user {UserId}", userId);
            return null;
        }
    }

    #endregion

    #region Group Members

    public async Task<LineUserProfile?> GetGroupMemberProfileAsync(string groupId, string userId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/group/{groupId}/member/{userId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to get group member profile: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LineUserProfile>(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting group member profile");
            return null;
        }
    }

    public async Task<LineGroupSummary?> GetGroupSummaryAsync(string groupId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/group/{groupId}/summary", ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to get group summary: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LineGroupSummary>(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting group summary");
            return null;
        }
    }

    public async Task<int> GetGroupMemberCountAsync(string groupId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/group/{groupId}/members/count", ct);

            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            var result = await response.Content.ReadFromJsonAsync<CountResponse>(ct);
            return result?.Count ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<LineFollowersResult> GetGroupMemberIdsAsync(string groupId, string? continuationToken = null, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}/group/{groupId}/members/ids";
        if (!string.IsNullOrEmpty(continuationToken))
        {
            url += $"?start={continuationToken}";
        }

        return await GetMemberIdsAsync(url, ct);
    }

    #endregion

    #region Room Members

    public async Task<LineUserProfile?> GetRoomMemberProfileAsync(string roomId, string userId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/room/{roomId}/member/{userId}", ct);

            if (!response.IsSuccessStatusCode)
            {
                _logger?.LogWarning("Failed to get room member profile: {StatusCode}", response.StatusCode);
                return null;
            }

            return await response.Content.ReadFromJsonAsync<LineUserProfile>(ct);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Error getting room member profile");
            return null;
        }
    }

    public async Task<int> GetRoomMemberCountAsync(string roomId, CancellationToken ct = default)
    {
        try
        {
            var response = await _httpClient.GetAsync($"{BaseUrl}/room/{roomId}/members/count", ct);

            if (!response.IsSuccessStatusCode)
            {
                return 0;
            }

            var result = await response.Content.ReadFromJsonAsync<CountResponse>(ct);
            return result?.Count ?? 0;
        }
        catch
        {
            return 0;
        }
    }

    public async Task<LineFollowersResult> GetRoomMemberIdsAsync(string roomId, string? continuationToken = null, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}/room/{roomId}/members/ids";
        if (!string.IsNullOrEmpty(continuationToken))
        {
            url += $"?start={continuationToken}";
        }

        return await GetMemberIdsAsync(url, ct);
    }

    #endregion

    #region Followers

    public async Task<LineFollowersResult> GetFollowerIdsAsync(string? continuationToken = null, CancellationToken ct = default)
    {
        var url = $"{BaseUrl}/followers/ids";
        if (!string.IsNullOrEmpty(continuationToken))
        {
            url += $"?start={continuationToken}";
        }

        return await GetMemberIdsAsync(url, ct);
    }

    #endregion

    #region Private Methods

    private async Task<LineFollowersResult> GetMemberIdsAsync(string url, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                return new LineFollowersResult(new List<string>(), null);
            }

            var result = await response.Content.ReadFromJsonAsync<MemberIdsResponse>(ct);
            return new LineFollowersResult(
                result?.MemberIds ?? new List<string>(),
                result?.Next
            );
        }
        catch
        {
            return new LineFollowersResult(new List<string>(), null);
        }
    }

    #endregion

    #region Response Models

    private record CountResponse(int Count);
    private record MemberIdsResponse(List<string> MemberIds, string? Next);

    #endregion
}
