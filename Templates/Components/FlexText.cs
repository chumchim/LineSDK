using System.Text.Json.Serialization;

namespace LineSDK.Templates.Components;

/// <summary>
/// Flex Text - ข้อความ
/// </summary>
public class FlexText : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "text";

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    #region Styling Properties

    /// <summary>
    /// Size: xxs, xs, sm, md, lg, xl, xxl, 3xl, 4xl, 5xl
    /// </summary>
    [JsonPropertyName("size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Size { get; set; }

    /// <summary>
    /// Weight: regular, bold
    /// </summary>
    [JsonPropertyName("weight")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Weight { get; set; }

    /// <summary>
    /// Color: hex color (#RRGGBB)
    /// </summary>
    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; set; }

    /// <summary>
    /// Align: start, center, end
    /// </summary>
    [JsonPropertyName("align")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Align { get; set; }

    /// <summary>
    /// Gravity: top, center, bottom
    /// </summary>
    [JsonPropertyName("gravity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Gravity { get; set; }

    [JsonPropertyName("flex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Flex { get; set; }

    [JsonPropertyName("margin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Margin { get; set; }

    [JsonPropertyName("wrap")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? Wrap { get; set; }

    [JsonPropertyName("maxLines")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? MaxLines { get; set; }

    /// <summary>
    /// Style: normal, italic
    /// </summary>
    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Style { get; set; }

    /// <summary>
    /// Decoration: none, underline, line-through
    /// </summary>
    [JsonPropertyName("decoration")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Decoration { get; set; }

    #endregion

    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IFlexAction? Action { get; set; }

    public FlexText() { }
    public FlexText(string text) => Text = text;

    public static implicit operator FlexText(string text) => new(text);

    #region Fluent Methods

    public FlexText SetSize(string size) { Size = size; return this; }
    public FlexText SetWeight(string weight) { Weight = weight; return this; }
    public FlexText SetColor(string color) { Color = color; return this; }
    public FlexText SetAlign(string align) { Align = align; return this; }
    public FlexText Bold() { Weight = "bold"; return this; }
    public FlexText Center() { Align = "center"; return this; }
    public FlexText Wrapped() { Wrap = true; return this; }

    #endregion

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["text"] = Text };

        if (!string.IsNullOrEmpty(Size)) obj["size"] = Size;
        if (!string.IsNullOrEmpty(Weight)) obj["weight"] = Weight;
        if (!string.IsNullOrEmpty(Color)) obj["color"] = Color;
        if (!string.IsNullOrEmpty(Align)) obj["align"] = Align;
        if (!string.IsNullOrEmpty(Gravity)) obj["gravity"] = Gravity;
        if (Flex.HasValue) obj["flex"] = Flex.Value;
        if (!string.IsNullOrEmpty(Margin)) obj["margin"] = Margin;
        if (Wrap.HasValue) obj["wrap"] = Wrap.Value;
        if (MaxLines.HasValue) obj["maxLines"] = MaxLines.Value;
        if (!string.IsNullOrEmpty(Style)) obj["style"] = Style;
        if (!string.IsNullOrEmpty(Decoration)) obj["decoration"] = Decoration;
        if (Action != null) obj["action"] = Action.ToJson();

        return obj;
    }
}
