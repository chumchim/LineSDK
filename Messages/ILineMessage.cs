namespace LineSDK.Messages;

/// <summary>
/// Base interface สำหรับ LINE Message ทุกประเภท
/// </summary>
public interface ILineMessage
{
    /// <summary>
    /// Message type: text, sticker, image, video, audio, location, flex, template
    /// </summary>
    string Type { get; }

    /// <summary>
    /// Convert to JSON object for API
    /// </summary>
    object ToJson();
}

/// <summary>
/// Text Message - ข้อความ text ธรรมดา
/// </summary>
public class TextMessage : ILineMessage
{
    public string Type => "text";
    public string Text { get; set; } = "";

    public TextMessage() { }
    public TextMessage(string text) => Text = text;

    public static implicit operator TextMessage(string text) => new(text);

    public object ToJson() => new { type = Type, text = Text };
}

/// <summary>
/// Sticker Message - สติกเกอร์
/// </summary>
public class StickerMessage : ILineMessage
{
    public string Type => "sticker";
    public string PackageId { get; set; } = "";
    public string StickerId { get; set; } = "";

    public StickerMessage() { }
    public StickerMessage(string packageId, string stickerId)
    {
        PackageId = packageId;
        StickerId = stickerId;
    }

    public object ToJson() => new { type = Type, packageId = PackageId, stickerId = StickerId };
}

/// <summary>
/// Image Message - รูปภาพ
/// </summary>
public class ImageMessage : ILineMessage
{
    public string Type => "image";
    public string OriginalContentUrl { get; set; } = "";
    public string PreviewImageUrl { get; set; } = "";

    public ImageMessage() { }
    public ImageMessage(string originalUrl, string? previewUrl = null)
    {
        OriginalContentUrl = originalUrl;
        PreviewImageUrl = previewUrl ?? originalUrl;
    }

    public object ToJson() => new { type = Type, originalContentUrl = OriginalContentUrl, previewImageUrl = PreviewImageUrl };
}

/// <summary>
/// Video Message - วิดีโอ
/// </summary>
public class VideoMessage : ILineMessage
{
    public string Type => "video";
    public string OriginalContentUrl { get; set; } = "";
    public string PreviewImageUrl { get; set; } = "";

    public VideoMessage() { }
    public VideoMessage(string originalUrl, string previewUrl)
    {
        OriginalContentUrl = originalUrl;
        PreviewImageUrl = previewUrl;
    }

    public object ToJson() => new { type = Type, originalContentUrl = OriginalContentUrl, previewImageUrl = PreviewImageUrl };
}

/// <summary>
/// Audio Message - เสียง
/// </summary>
public class AudioMessage : ILineMessage
{
    public string Type => "audio";
    public string OriginalContentUrl { get; set; } = "";
    public int Duration { get; set; }

    public AudioMessage() { }
    public AudioMessage(string originalUrl, int durationMs)
    {
        OriginalContentUrl = originalUrl;
        Duration = durationMs;
    }

    public object ToJson() => new { type = Type, originalContentUrl = OriginalContentUrl, duration = Duration };
}

/// <summary>
/// Location Message - ตำแหน่ง
/// </summary>
public class LocationMessage : ILineMessage
{
    public string Type => "location";
    public string Title { get; set; } = "";
    public string Address { get; set; } = "";
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public LocationMessage() { }
    public LocationMessage(string title, string address, double latitude, double longitude)
    {
        Title = title;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
    }

    public object ToJson() => new { type = Type, title = Title, address = Address, latitude = Latitude, longitude = Longitude };
}
