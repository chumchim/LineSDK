using System.Text.Json.Serialization;

namespace LineSDK.Templates.Components;

/// <summary>
/// Flex Separator - เส้นคั่น
/// </summary>
public class FlexSeparator : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "separator";

    [JsonPropertyName("margin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Margin { get; set; }

    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; set; }

    public FlexSeparator() { }

    public FlexSeparator(string? margin = null, string? color = null)
    {
        Margin = margin;
        Color = color;
    }

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type };
        if (!string.IsNullOrEmpty(Margin)) obj["margin"] = Margin;
        if (!string.IsNullOrEmpty(Color)) obj["color"] = Color;
        return obj;
    }
}

/// <summary>
/// Flex Filler - ช่องว่างยืดหยุ่น
/// </summary>
public class FlexFiller : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "filler";

    [JsonPropertyName("flex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Flex { get; set; }

    public FlexFiller() { }
    public FlexFiller(int flex) => Flex = flex;

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type };
        if (Flex.HasValue) obj["flex"] = Flex.Value;
        return obj;
    }
}

/// <summary>
/// Flex Spacer - ช่องว่างขนาดคงที่
/// </summary>
public class FlexSpacer : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "spacer";

    /// <summary>
    /// Size: xs, sm, md, lg, xl, xxl
    /// </summary>
    [JsonPropertyName("size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Size { get; set; }

    public FlexSpacer() { }
    public FlexSpacer(string size) => Size = size;

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type };
        if (!string.IsNullOrEmpty(Size)) obj["size"] = Size;
        return obj;
    }
}

/// <summary>
/// Flex Icon - ไอคอน
/// </summary>
public class FlexIcon : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "icon";

    [JsonPropertyName("url")]
    public string Url { get; set; } = "";

    /// <summary>
    /// Size: xxs, xs, sm, md, lg, xl, xxl, 3xl, 4xl, 5xl
    /// </summary>
    [JsonPropertyName("size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Size { get; set; }

    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AspectRatio { get; set; }

    [JsonPropertyName("margin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Margin { get; set; }

    public FlexIcon() { }
    public FlexIcon(string url) => Url = url;

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["url"] = Url };
        if (!string.IsNullOrEmpty(Size)) obj["size"] = Size;
        if (!string.IsNullOrEmpty(AspectRatio)) obj["aspectRatio"] = AspectRatio;
        if (!string.IsNullOrEmpty(Margin)) obj["margin"] = Margin;
        return obj;
    }
}
