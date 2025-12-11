using System.Text.Json.Serialization;

namespace LineSDK.Webhook;

/// <summary>
/// LINE Message (Webhook) - ข้อความที่รับมา
/// </summary>
public record LineMessage
{
    /// <summary>
    /// Message ID
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = "";

    /// <summary>
    /// Message type: text, image, video, audio, file, location, sticker
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "";

    #region Text Message

    [JsonPropertyName("text")]
    public string? Text { get; init; }

    [JsonPropertyName("emojis")]
    public List<LineEmoji>? Emojis { get; init; }

    [JsonPropertyName("mention")]
    public LineMention? Mention { get; init; }

    #endregion

    #region Media Message

    /// <summary>
    /// Content Provider type: line, external
    /// </summary>
    [JsonPropertyName("contentProvider")]
    public LineContentProvider? ContentProvider { get; init; }

    /// <summary>
    /// Duration (สำหรับ video, audio) in milliseconds
    /// </summary>
    [JsonPropertyName("duration")]
    public int? Duration { get; init; }

    #endregion

    #region File Message

    [JsonPropertyName("fileName")]
    public string? FileName { get; init; }

    [JsonPropertyName("fileSize")]
    public long? FileSize { get; init; }

    #endregion

    #region Location Message

    [JsonPropertyName("title")]
    public string? Title { get; init; }

    [JsonPropertyName("address")]
    public string? Address { get; init; }

    [JsonPropertyName("latitude")]
    public double? Latitude { get; init; }

    [JsonPropertyName("longitude")]
    public double? Longitude { get; init; }

    #endregion

    #region Sticker Message

    [JsonPropertyName("packageId")]
    public string? PackageId { get; init; }

    [JsonPropertyName("stickerId")]
    public string? StickerId { get; init; }

    [JsonPropertyName("stickerResourceType")]
    public string? StickerResourceType { get; init; }

    [JsonPropertyName("keywords")]
    public List<string>? Keywords { get; init; }

    #endregion

    #region Helper Properties

    [JsonIgnore]
    public bool IsTextMessage => Type.Equals("text", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsImageMessage => Type.Equals("image", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsVideoMessage => Type.Equals("video", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsAudioMessage => Type.Equals("audio", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsFileMessage => Type.Equals("file", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsLocationMessage => Type.Equals("location", StringComparison.OrdinalIgnoreCase);

    [JsonIgnore]
    public bool IsStickerMessage => Type.Equals("sticker", StringComparison.OrdinalIgnoreCase);

    #endregion
}

/// <summary>
/// LINE Content Provider
/// </summary>
public record LineContentProvider
{
    /// <summary>
    /// Type: line, external
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; init; } = "";

    /// <summary>
    /// Original content URL (ถ้า type = external)
    /// </summary>
    [JsonPropertyName("originalContentUrl")]
    public string? OriginalContentUrl { get; init; }

    /// <summary>
    /// Preview image URL (ถ้า type = external)
    /// </summary>
    [JsonPropertyName("previewImageUrl")]
    public string? PreviewImageUrl { get; init; }
}

/// <summary>
/// LINE Emoji in message
/// </summary>
public record LineEmoji
{
    [JsonPropertyName("index")]
    public int Index { get; init; }

    [JsonPropertyName("length")]
    public int Length { get; init; }

    [JsonPropertyName("productId")]
    public string ProductId { get; init; } = "";

    [JsonPropertyName("emojiId")]
    public string EmojiId { get; init; } = "";
}

/// <summary>
/// LINE Mention in message
/// </summary>
public record LineMention
{
    [JsonPropertyName("mentionees")]
    public List<LineMentionee>? Mentionees { get; init; }
}

/// <summary>
/// LINE Mentionee
/// </summary>
public record LineMentionee
{
    [JsonPropertyName("index")]
    public int Index { get; init; }

    [JsonPropertyName("length")]
    public int Length { get; init; }

    [JsonPropertyName("userId")]
    public string? UserId { get; init; }
}
