using System.Text.Json.Serialization;

namespace LineSDK.Models;

/// <summary>
/// LINE User Profile
/// </summary>
public record LineUserProfile
{
    [JsonPropertyName("userId")]
    public string UserId { get; init; } = "";

    [JsonPropertyName("displayName")]
    public string DisplayName { get; init; } = "";

    [JsonPropertyName("pictureUrl")]
    public string? PictureUrl { get; init; }

    [JsonPropertyName("statusMessage")]
    public string? StatusMessage { get; init; }
}

/// <summary>
/// LINE Group Summary
/// </summary>
public record LineGroupSummary
{
    [JsonPropertyName("groupId")]
    public string GroupId { get; init; } = "";

    [JsonPropertyName("groupName")]
    public string? GroupName { get; init; }

    [JsonPropertyName("pictureUrl")]
    public string? PictureUrl { get; init; }
}

/// <summary>
/// LINE Followers Result (for pagination)
/// </summary>
public record LineFollowersResult(
    List<string> UserIds,
    string? NextToken
);
