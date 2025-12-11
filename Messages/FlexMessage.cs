using LineSDK.Templates;

namespace LineSDK.Messages;

/// <summary>
/// Flex Message - ข้อความแบบ custom layout
/// </summary>
public class FlexMessage : ILineMessage
{
    public string Type => "flex";

    /// <summary>
    /// Alt text - แสดงใน notification และเครื่องที่ไม่รองรับ Flex
    /// </summary>
    public string AltText { get; set; } = "";

    /// <summary>
    /// Flex content - Bubble หรือ Carousel
    /// </summary>
    public IFlexContainer Contents { get; set; } = null!;

    public FlexMessage() { }

    public FlexMessage(string altText, IFlexContainer contents)
    {
        AltText = altText;
        Contents = contents;
    }

    /// <summary>
    /// สร้าง Flex Message จาก Bubble
    /// </summary>
    public static FlexMessage FromBubble(string altText, FlexBubble bubble)
        => new(altText, bubble);

    /// <summary>
    /// สร้าง Flex Message จาก Carousel
    /// </summary>
    public static FlexMessage FromCarousel(string altText, FlexCarousel carousel)
        => new(altText, carousel);

    public object ToJson() => new { type = Type, altText = AltText, contents = Contents.ToJson() };
}
