using System.Text.Json.Serialization;

namespace LineSDK.Templates;

/// <summary>
/// Flex Carousel - Container แบบหลาย Bubble เลื่อนได้
/// </summary>
public class FlexCarousel : IFlexContainer
{
    [JsonPropertyName("type")]
    public string Type => "carousel";

    /// <summary>
    /// รายการ Bubbles (สูงสุด 12 bubbles)
    /// </summary>
    [JsonPropertyName("contents")]
    public List<FlexBubble> Contents { get; set; } = new();

    public FlexCarousel() { }

    public FlexCarousel(params FlexBubble[] bubbles)
    {
        Contents = bubbles.ToList();
    }

    public FlexCarousel(IEnumerable<FlexBubble> bubbles)
    {
        Contents = bubbles.ToList();
    }

    /// <summary>
    /// เพิ่ม Bubble
    /// </summary>
    public FlexCarousel Add(FlexBubble bubble)
    {
        Contents.Add(bubble);
        return this;
    }

    public object ToJson() => new
    {
        type = Type,
        contents = Contents.Select(c => c.ToJson())
    };
}
