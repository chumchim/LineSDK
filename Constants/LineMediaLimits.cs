namespace LineSDK.Constants;

/// <summary>
/// LINE Messaging API Media File Size Limits and Validation
/// Based on LINE Messaging API Documentation (2025)
/// https://developers.line.biz/en/reference/messaging-api/
/// </summary>
public static class LineMediaLimits
{
    #region File Size Limits (in bytes)

    /// <summary>Image maximum file size: 10 MB</summary>
    public const long ImageMaxSize = 10 * 1024 * 1024;

    /// <summary>Video maximum file size: 200 MB</summary>
    public const long VideoMaxSize = 200 * 1024 * 1024;

    /// <summary>Audio maximum file size: 200 MB</summary>
    public const long AudioMaxSize = 200 * 1024 * 1024;

    #endregion

    #region Allowed MIME Types

    /// <summary>Allowed image MIME types for LINE Messaging API</summary>
    public static readonly string[] AllowedImageTypes =
    {
        "image/jpeg",
        "image/jpg",
        "image/png"
    };

    /// <summary>Allowed video MIME types for LINE Messaging API</summary>
    public static readonly string[] AllowedVideoTypes =
    {
        "video/mp4"
    };

    /// <summary>Allowed audio MIME types for LINE Messaging API</summary>
    public static readonly string[] AllowedAudioTypes =
    {
        "audio/x-m4a",
        "audio/m4a",
        "audio/mp4"
    };

    #endregion

    #region Validation Methods

    /// <summary>
    /// Get maximum allowed file size for a content type
    /// </summary>
    /// <param name="contentType">MIME type (e.g., "image/jpeg")</param>
    /// <returns>Maximum size in bytes, or null if content type is not recognized</returns>
    public static long? GetMaxSizeForType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return null;

        var normalizedType = contentType.ToLowerInvariant().Trim();

        if (Array.Exists(AllowedImageTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)))
            return ImageMaxSize;

        if (Array.Exists(AllowedVideoTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)))
            return VideoMaxSize;

        if (Array.Exists(AllowedAudioTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)))
            return AudioMaxSize;

        return null;
    }

    /// <summary>
    /// Validate file size and content type for LINE Messaging API
    /// </summary>
    /// <param name="contentType">MIME type</param>
    /// <param name="fileSize">File size in bytes</param>
    /// <returns>Validation result with error message if invalid</returns>
    public static MediaValidationResult Validate(string contentType, long fileSize)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return MediaValidationResult.Fail("Content type is required");

        if (fileSize <= 0)
            return MediaValidationResult.Fail("File size must be greater than 0");

        var maxSize = GetMaxSizeForType(contentType);

        if (maxSize == null)
            return MediaValidationResult.Fail($"Content type '{contentType}' is not supported by LINE Messaging API");

        if (fileSize > maxSize.Value)
        {
            var maxSizeMB = FormatFileSize(maxSize.Value);
            var actualSizeMB = FormatFileSize(fileSize);
            return MediaValidationResult.Fail(
                $"File size {actualSizeMB} exceeds LINE limit of {maxSizeMB} for {contentType}",
                maxSize.Value);
        }

        return MediaValidationResult.Ok(maxSize.Value);
    }

    /// <summary>
    /// Check if a content type is supported by LINE Messaging API
    /// </summary>
    public static bool IsContentTypeSupported(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return false;

        var normalizedType = contentType.ToLowerInvariant().Trim();

        return Array.Exists(AllowedImageTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)) ||
               Array.Exists(AllowedVideoTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)) ||
               Array.Exists(AllowedAudioTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Get media category from content type
    /// </summary>
    public static string GetMediaCategory(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            return "unknown";

        var normalizedType = contentType.ToLowerInvariant().Trim();

        if (Array.Exists(AllowedImageTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)))
            return "image";
        if (Array.Exists(AllowedVideoTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)))
            return "video";
        if (Array.Exists(AllowedAudioTypes, t => t.Equals(normalizedType, StringComparison.OrdinalIgnoreCase)))
            return "audio";

        return "unknown";
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Format file size in human-readable format (KB, MB, GB)
    /// </summary>
    public static string FormatFileSize(long bytes)
    {
        if (bytes < 1024)
            return $"{bytes} B";

        if (bytes < 1024 * 1024)
            return $"{bytes / 1024.0:F1} KB";

        if (bytes < 1024 * 1024 * 1024)
            return $"{bytes / (1024.0 * 1024.0):F1} MB";

        return $"{bytes / (1024.0 * 1024.0 * 1024.0):F1} GB";
    }

    #endregion
}

/// <summary>
/// Validation result for media file validation
/// </summary>
public readonly struct MediaValidationResult
{
    /// <summary>Whether the validation passed</summary>
    public bool IsValid { get; }

    /// <summary>Error message if validation failed</summary>
    public string? Error { get; }

    /// <summary>Maximum allowed size for the content type</summary>
    public long? MaxAllowedSize { get; }

    private MediaValidationResult(bool isValid, string? error, long? maxAllowedSize)
    {
        IsValid = isValid;
        Error = error;
        MaxAllowedSize = maxAllowedSize;
    }

    /// <summary>Create a successful validation result</summary>
    public static MediaValidationResult Ok(long? maxAllowedSize = null) => new(true, null, maxAllowedSize);

    /// <summary>Create a failed validation result</summary>
    public static MediaValidationResult Fail(string error, long? maxAllowedSize = null) => new(false, error, maxAllowedSize);

    /// <summary>Throw ArgumentException if validation failed</summary>
    public void ThrowIfInvalid()
    {
        if (!IsValid)
            throw new ArgumentException(Error);
    }
}
