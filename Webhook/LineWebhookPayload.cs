using System.Text.Json.Serialization;

namespace LineSDK.Webhook;

/// <summary>
/// LINE Webhook Payload - โครงสร้างหลักที่ LINE ส่งมา
/// </summary>
public record LineWebhookPayload
{
    /// <summary>
    /// User ID ที่ส่ง webhook นี้
    /// </summary>
    [JsonPropertyName("destination")]
    public string Destination { get; init; } = "";

    /// <summary>
    /// รายการ events
    /// </summary>
    [JsonPropertyName("events")]
    public List<LineEvent> Events { get; init; } = new();
}
