using System.Text.Json.Serialization;

namespace LineSDK.Webhook;

/// <summary>
/// LINE Source - แหล่งที่มาของ event
/// </summary>
public record LineSource
{
    /// <summary>
    /// Type: user, group, room
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "";

    /// <summary>
    /// User ID (มีเสมอ ยกเว้นบาง event)
    /// </summary>
    [JsonPropertyName("userId")]
    public string? UserId { get; init; }

    /// <summary>
    /// Group ID (ถ้า type = group)
    /// </summary>
    [JsonPropertyName("groupId")]
    public string? GroupId { get; init; }

    /// <summary>
    /// Room ID (ถ้า type = room)
    /// </summary>
    [JsonPropertyName("roomId")]
    public string? RoomId { get; init; }

    #region Helper Properties

    [JsonIgnore]
    public bool IsUser => Type.Equals("user", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsGroup => Type.Equals("group", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsRoom => Type.Equals("room", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Chat ID - UserId, GroupId, หรือ RoomId ขึ้นอยู่กับ type
    /// </summary>
    [JsonIgnore]
    public string? ChatId => GroupId ?? RoomId ?? UserId;

    #endregion
}
