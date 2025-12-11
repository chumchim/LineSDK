namespace LineSDK.Options;

/// <summary>
/// LINE Client Options - ตั้งค่าสำหรับเชื่อมต่อ LINE Messaging API
/// </summary>
public class LineClientOptions
{
    public const string SectionName = "Line";

    /// <summary>
    /// Channel Access Token - ได้จาก LINE Developers Console
    /// </summary>
    public string ChannelAccessToken { get; set; } = "";

    /// <summary>
    /// Channel Secret - ใช้ verify webhook signature
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
    /// Validate options
    /// </summary>
    public void Validate()
    {
        if (string.IsNullOrEmpty(ChannelAccessToken))
            throw new ArgumentException("ChannelAccessToken is required", nameof(ChannelAccessToken));

        if (string.IsNullOrEmpty(ChannelSecret))
            throw new ArgumentException("ChannelSecret is required", nameof(ChannelSecret));
    }

    /// <summary>
    /// Check if options are configured (for dev/simulator mode)
    /// </summary>
    public bool IsConfigured => !string.IsNullOrEmpty(ChannelAccessToken);
}
