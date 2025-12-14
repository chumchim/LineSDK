namespace LineSDK.Messaging;

/// <summary>
/// Binary content ที่ได้จาก LINE message (image, video, audio, file)
/// </summary>
public class MessageContent : IDisposable
{
    /// <summary>
    /// Binary content stream
    /// </summary>
    public Stream Content { get; }

    /// <summary>
    /// Content type (e.g., image/jpeg, video/mp4, audio/m4a)
    /// </summary>
    public string ContentType { get; }

    /// <summary>
    /// File name (ถ้ามี)
    /// </summary>
    public string? FileName { get; }

    /// <summary>
    /// Content length in bytes
    /// </summary>
    public long? ContentLength { get; }

    public MessageContent(Stream content, string contentType, string? fileName = null, long? contentLength = null)
    {
        Content = content;
        ContentType = contentType;
        FileName = fileName;
        ContentLength = contentLength;
    }

    /// <summary>
    /// อ่าน content ทั้งหมดเป็น byte array
    /// </summary>
    public async Task<byte[]> ReadAllBytesAsync(CancellationToken ct = default)
    {
        using var memoryStream = new MemoryStream();
        await Content.CopyToAsync(memoryStream, ct);
        return memoryStream.ToArray();
    }

    /// <summary>
    /// เช็คว่าเป็น image หรือไม่
    /// </summary>
    public bool IsImage => ContentType.StartsWith("image/");

    /// <summary>
    /// เช็คว่าเป็น video หรือไม่
    /// </summary>
    public bool IsVideo => ContentType.StartsWith("video/");

    /// <summary>
    /// เช็คว่าเป็น audio หรือไม่
    /// </summary>
    public bool IsAudio => ContentType.StartsWith("audio/");

    public void Dispose()
    {
        Content?.Dispose();
    }
}
