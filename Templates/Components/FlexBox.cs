using System.Text.Json.Serialization;

namespace LineSDK.Templates.Components;

/// <summary>
/// Flex Box - Container สำหรับจัด layout
/// </summary>
public class FlexBox : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "box";

    /// <summary>
    /// Layout: horizontal, vertical, baseline
    /// </summary>
    [JsonPropertyName("layout")]
    public string Layout { get; set; } = "vertical";

    /// <summary>
    /// Contents
    /// </summary>
    [JsonPropertyName("contents")]
    public List<IFlexComponent> Contents { get; set; } = new();

    #region Styling Properties

    [JsonPropertyName("backgroundColor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BackgroundColor { get; set; }

    [JsonPropertyName("borderColor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BorderColor { get; set; }

    [JsonPropertyName("borderWidth")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BorderWidth { get; set; }

    [JsonPropertyName("cornerRadius")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CornerRadius { get; set; }

    [JsonPropertyName("width")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Width { get; set; }

    [JsonPropertyName("height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Height { get; set; }

    [JsonPropertyName("flex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Flex { get; set; }

    [JsonPropertyName("spacing")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Spacing { get; set; }

    [JsonPropertyName("margin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Margin { get; set; }

    [JsonPropertyName("paddingAll")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PaddingAll { get; set; }

    [JsonPropertyName("paddingTop")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PaddingTop { get; set; }

    [JsonPropertyName("paddingBottom")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PaddingBottom { get; set; }

    [JsonPropertyName("paddingStart")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PaddingStart { get; set; }

    [JsonPropertyName("paddingEnd")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? PaddingEnd { get; set; }

    [JsonPropertyName("justifyContent")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? JustifyContent { get; set; }

    [JsonPropertyName("alignItems")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AlignItems { get; set; }

    #endregion

    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IFlexAction? Action { get; set; }

    public FlexBox() { }

    public FlexBox(string layout, params IFlexComponent[] contents)
    {
        Layout = layout;
        Contents = contents.ToList();
    }

    /// <summary>
    /// สร้าง Vertical Box
    /// </summary>
    public static FlexBox Vertical(params IFlexComponent[] contents)
        => new("vertical", contents);

    /// <summary>
    /// สร้าง Horizontal Box
    /// </summary>
    public static FlexBox Horizontal(params IFlexComponent[] contents)
        => new("horizontal", contents);

    /// <summary>
    /// สร้าง Baseline Box
    /// </summary>
    public static FlexBox Baseline(params IFlexComponent[] contents)
        => new("baseline", contents);

    /// <summary>
    /// เพิ่ม component
    /// </summary>
    public FlexBox Add(IFlexComponent component)
    {
        Contents.Add(component);
        return this;
    }

    public object ToJson()
    {
        var obj = new Dictionary<string, object>
        {
            ["type"] = Type,
            ["layout"] = Layout,
            ["contents"] = Contents.Select(c => c.ToJson()).ToList()
        };

        if (!string.IsNullOrEmpty(BackgroundColor)) obj["backgroundColor"] = BackgroundColor;
        if (!string.IsNullOrEmpty(BorderColor)) obj["borderColor"] = BorderColor;
        if (!string.IsNullOrEmpty(BorderWidth)) obj["borderWidth"] = BorderWidth;
        if (!string.IsNullOrEmpty(CornerRadius)) obj["cornerRadius"] = CornerRadius;
        if (!string.IsNullOrEmpty(Width)) obj["width"] = Width;
        if (!string.IsNullOrEmpty(Height)) obj["height"] = Height;
        if (Flex.HasValue) obj["flex"] = Flex.Value;
        if (!string.IsNullOrEmpty(Spacing)) obj["spacing"] = Spacing;
        if (!string.IsNullOrEmpty(Margin)) obj["margin"] = Margin;
        if (!string.IsNullOrEmpty(PaddingAll)) obj["paddingAll"] = PaddingAll;
        if (!string.IsNullOrEmpty(PaddingTop)) obj["paddingTop"] = PaddingTop;
        if (!string.IsNullOrEmpty(PaddingBottom)) obj["paddingBottom"] = PaddingBottom;
        if (!string.IsNullOrEmpty(PaddingStart)) obj["paddingStart"] = PaddingStart;
        if (!string.IsNullOrEmpty(PaddingEnd)) obj["paddingEnd"] = PaddingEnd;
        if (!string.IsNullOrEmpty(JustifyContent)) obj["justifyContent"] = JustifyContent;
        if (!string.IsNullOrEmpty(AlignItems)) obj["alignItems"] = AlignItems;
        if (Action != null) obj["action"] = Action.ToJson();

        return obj;
    }
}
