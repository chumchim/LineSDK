using System.Text.Json.Serialization;

namespace LineSDK.Webhook;

/// <summary>
/// LINE Event - Event ที่เกิดขึ้น
/// </summary>
public record LineEvent
{
    /// <summary>
    /// Event type: message, follow, unfollow, join, leave, memberJoined, memberLeft, postback, etc.
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "";

    /// <summary>
    /// Timestamp (milliseconds)
    /// </summary>
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; init; }

    /// <summary>
    /// Source ของ event (user, group, room)
    /// </summary>
    [JsonPropertyName("source")]
    public LineSource? Source { get; init; }

    /// <summary>
    /// Reply token (ใช้ได้ภายใน 1 นาที)
    /// </summary>
    [JsonPropertyName("replyToken")]
    public string? ReplyToken { get; init; }

    /// <summary>
    /// Message (สำหรับ message event)
    /// </summary>
    [JsonPropertyName("message")]
    public LineMessage? Message { get; init; }

    /// <summary>
    /// Postback data (สำหรับ postback event)
    /// </summary>
    [JsonPropertyName("postback")]
    public LinePostback? Postback { get; init; }

    /// <summary>
    /// Joined members (สำหรับ memberJoined event)
    /// </summary>
    [JsonPropertyName("joined")]
    public LineJoinedMembers? Joined { get; init; }

    /// <summary>
    /// Left members (สำหรับ memberLeft event)
    /// </summary>
    [JsonPropertyName("left")]
    public LineLeftMembers? Left { get; init; }

    #region Helper Properties

    [JsonIgnore]
    public bool IsMessageEvent => Type.Equals("message", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsFollowEvent => Type.Equals("follow", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsUnfollowEvent => Type.Equals("unfollow", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsJoinEvent => Type.Equals("join", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsLeaveEvent => Type.Equals("leave", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsMemberJoinedEvent => Type.Equals("memberJoined", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsMemberLeftEvent => Type.Equals("memberLeft", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsPostbackEvent => Type.Equals("postback", StringComparison.OrdinalIgnoreCase);

    #endregion
}

/// <summary>
/// LINE Postback Data
/// </summary>
public record LinePostback
{
    [JsonPropertyName("data")]
    public string Data { get; init; } = "";

    [JsonPropertyName("params")]
    public LinePostbackParams? Params { get; init; }
}

/// <summary>
/// LINE Postback Params (สำหรับ datetime picker)
/// </summary>
public record LinePostbackParams
{
    [JsonPropertyName("date")]
    public string? Date { get; init; }

    [JsonPropertyName("time")]
    public string? Time { get; init; }

    [JsonPropertyName("datetime")]
    public string? Datetime { get; init; }
}

/// <summary>
/// LINE Joined Members
/// </summary>
public record LineJoinedMembers
{
    [JsonPropertyName("members")]
    public List<LineMember> Members { get; init; } = new();
}

/// <summary>
/// LINE Left Members
/// </summary>
public record LineLeftMembers
{
    [JsonPropertyName("members")]
    public List<LineMember> Members { get; init; } = new();
}

/// <summary>
/// LINE Member
/// </summary>
public record LineMember
{
    [JsonPropertyName("type")]
    public string Type { get; init; } = "user";

    [JsonPropertyName("userId")]
    public string? UserId { get; init; }
}
