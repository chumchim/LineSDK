namespace LineSDK.Options;

/// <summary>
/// Token mode for LINE API authentication
/// </summary>
public enum LineTokenMode
{
    /// <summary>
    /// Use static long-lived Channel Access Token (simple, for development)
    /// </summary>
    Static,

    /// <summary>
    /// Use stateless JWT token with auto-refresh (recommended for production)
    /// </summary>
    Stateless
}

/// <summary>
/// LINE Client Options - ตั้งค่าสำหรับเชื่อมต่อ LINE Messaging API
/// </summary>
public class LineClientOptions
{
    public const string SectionName = "Line";

    /// <summary>
    /// Token mode - Static (long-lived) or Stateless (JWT auto-refresh)
    /// Default: Static for backward compatibility
    /// </summary>
    public LineTokenMode TokenMode { get; set; } = LineTokenMode.Static;

    /// <summary>
    /// Channel ID - required for Stateless token mode
    /// </summary>
    public string ChannelId { get; set; } = "";

    /// <summary>
    /// Channel Access Token - ได้จาก LINE Developers Console
    /// Required for Static token mode
    /// </summary>
    public string ChannelAccessToken { get; set; } = "";

    /// <summary>
    /// Channel Secret - ใช้ verify webhook signature และ issue stateless token
    /// </summary>
    public string ChannelSecret { get; set; } = "";

    /// <summary>
    /// Bot Basic ID - @xxx (optional)
    /// </summary>
    public string? BotBasicId { get; set; }

    /// <summary>
    /// URL สำหรับ Add Friend - https://line.me/R/ti/p/@xxx
    /// </summary>
    public string? AddFriendUrl { get; set; }

    /// <summary>
    /// LIFF URL สำหรับลงทะเบียน
    /// </summary>
    public string? LiffRegisterUrl { get; set; }

    /// <summary>
    /// LINE Messaging API Base URL
    /// </summary>
    public string ApiBaseUrl { get; set; } = "https://api.line.me";

    /// <summary>
    /// LINE OAuth API Base URL (for token issuance)
    /// </summary>
    public string OAuthBaseUrl { get; set; } = "https://api.line.me";

    /// <summary>
    /// Token refresh buffer in minutes (refresh before actual expiry)
    /// Default: 5 minutes before expiry
    /// </summary>
    public int TokenRefreshBufferMinutes { get; set; } = 5;

    /// <summary>
    /// Validate options based on TokenMode
    /// </summary>
    public void Validate()
    {
        if (TokenMode == LineTokenMode.Static)
        {
            if (string.IsNullOrEmpty(ChannelAccessToken))
                throw new ArgumentException("ChannelAccessToken is required for Static token mode", nameof(ChannelAccessToken));
        }
        else if (TokenMode == LineTokenMode.Stateless)
        {
            if (string.IsNullOrEmpty(ChannelId))
                throw new ArgumentException("ChannelId is required for Stateless token mode", nameof(ChannelId));

            if (string.IsNullOrEmpty(ChannelSecret))
                throw new ArgumentException("ChannelSecret is required for Stateless token mode", nameof(ChannelSecret));
        }
    }

    /// <summary>
    /// Check if options are configured
    /// </summary>
    public bool IsConfigured => TokenMode == LineTokenMode.Stateless
        ? !string.IsNullOrEmpty(ChannelId) && !string.IsNullOrEmpty(ChannelSecret)
        : !string.IsNullOrEmpty(ChannelAccessToken);
}
