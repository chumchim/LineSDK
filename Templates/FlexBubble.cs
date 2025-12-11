using System.Text.Json.Serialization;
using LineSDK.Templates.Components;

namespace LineSDK.Templates;

/// <summary>
/// Flex Bubble - Container แบบกล่องเดียว
/// </summary>
public class FlexBubble : IFlexContainer
{
    [JsonPropertyName("type")]
    public string Type => "bubble";

    /// <summary>
    /// ขนาด: nano, micro, kilo, mega, giga
    /// </summary>
    [JsonPropertyName("size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Size { get; set; }

    /// <summary>
    /// ทิศทาง: ltr (ซ้ายไปขวา), rtl (ขวาไปซ้าย)
    /// </summary>
    [JsonPropertyName("direction")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Direction { get; set; }

    /// <summary>
    /// Header section
    /// </summary>
    [JsonPropertyName("header")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBox? Header { get; set; }

    /// <summary>
    /// Hero section (รูปภาพหลัก)
    /// </summary>
    [JsonPropertyName("hero")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IFlexComponent? Hero { get; set; }

    /// <summary>
    /// Body section (เนื้อหาหลัก)
    /// </summary>
    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBox? Body { get; set; }

    /// <summary>
    /// Footer section (ปุ่ม action)
    /// </summary>
    [JsonPropertyName("footer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBox? Footer { get; set; }

    /// <summary>
    /// Styles
    /// </summary>
    [JsonPropertyName("styles")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBubbleStyles? Styles { get; set; }

    /// <summary>
    /// Action เมื่อกดที่ bubble
    /// </summary>
    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IFlexAction? Action { get; set; }

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type };

        if (!string.IsNullOrEmpty(Size)) obj["size"] = Size;
        if (!string.IsNullOrEmpty(Direction)) obj["direction"] = Direction;
        if (Header != null) obj["header"] = Header.ToJson();
        if (Hero != null) obj["hero"] = Hero.ToJson();
        if (Body != null) obj["body"] = Body.ToJson();
        if (Footer != null) obj["footer"] = Footer.ToJson();
        if (Styles != null) obj["styles"] = Styles.ToJson();
        if (Action != null) obj["action"] = Action.ToJson();

        return obj;
    }
}

/// <summary>
/// Flex Bubble Styles
/// </summary>
public class FlexBubbleStyles
{
    [JsonPropertyName("header")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBlockStyle? Header { get; set; }

    [JsonPropertyName("hero")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBlockStyle? Hero { get; set; }

    [JsonPropertyName("body")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBlockStyle? Body { get; set; }

    [JsonPropertyName("footer")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public FlexBlockStyle? Footer { get; set; }
}

/// <summary>
/// Flex Block Style
/// </summary>
public class FlexBlockStyle
{
    [JsonPropertyName("backgroundColor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BackgroundColor { get; set; }

    [JsonPropertyName("separator")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Separator { get; set; }

    [JsonPropertyName("separatorColor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SeparatorColor { get; set; }

    public object ToJson()
    {
        var obj = new Dictionary<string, object>();
        if (!string.IsNullOrEmpty(BackgroundColor)) obj["backgroundColor"] = BackgroundColor;
        if (Separator.HasValue) obj["separator"] = Separator.Value;
        if (!string.IsNullOrEmpty(SeparatorColor)) obj["separatorColor"] = SeparatorColor;
        return obj;
    }
}

/// <summary>
/// Flex Bubble Styles Extension
/// </summary>
public static class FlexBubbleStylesExtensions
{
    public static object ToJson(this FlexBubbleStyles styles)
    {
        var obj = new Dictionary<string, object>();
        if (styles.Header != null) obj["header"] = styles.Header.ToJson();
        if (styles.Hero != null) obj["hero"] = styles.Hero.ToJson();
        if (styles.Body != null) obj["body"] = styles.Body.ToJson();
        if (styles.Footer != null) obj["footer"] = styles.Footer.ToJson();
        return obj;
    }
}
