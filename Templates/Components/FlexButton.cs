using System.Text.Json.Serialization;

namespace LineSDK.Templates.Components;

/// <summary>
/// Flex Button - ปุ่มกด
/// </summary>
public class FlexButton : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "button";

    [JsonPropertyName("action")]
    public IFlexAction Action { get; set; } = null!;

    #region Styling Properties

    /// <summary>
    /// Style: primary, secondary, link
    /// </summary>
    [JsonPropertyName("style")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Style { get; set; }

    /// <summary>
    /// Height: sm, md
    /// </summary>
    [JsonPropertyName("height")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Height { get; set; }

    [JsonPropertyName("color")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Color { get; set; }

    [JsonPropertyName("flex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Flex { get; set; }

    [JsonPropertyName("margin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Margin { get; set; }

    /// <summary>
    /// Gravity: top, center, bottom
    /// </summary>
    [JsonPropertyName("gravity")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Gravity { get; set; }

    #endregion

    public FlexButton() { }

    public FlexButton(IFlexAction action, string? style = null)
    {
        Action = action;
        Style = style;
    }

    #region Factory Methods

    /// <summary>
    /// สร้างปุ่ม Primary
    /// </summary>
    public static FlexButton Primary(IFlexAction action)
        => new(action, "primary");

    /// <summary>
    /// สร้างปุ่ม Secondary
    /// </summary>
    public static FlexButton Secondary(IFlexAction action)
        => new(action, "secondary");

    /// <summary>
    /// สร้างปุ่ม Link
    /// </summary>
    public static FlexButton Link(IFlexAction action)
        => new(action, "link");

    /// <summary>
    /// สร้างปุ่ม Postback
    /// </summary>
    public static FlexButton Postback(string label, string data, string? style = "primary")
        => new(new PostbackAction(data, label), style);

    /// <summary>
    /// สร้างปุ่ม URI
    /// </summary>
    public static FlexButton Uri(string label, string uri, string? style = "primary")
        => new(new UriAction(uri, label), style);

    #endregion

    #region Fluent Methods

    public FlexButton SetStyle(string style) { Style = style; return this; }
    public FlexButton SetColor(string color) { Color = color; return this; }
    public FlexButton SetHeight(string height) { Height = height; return this; }
    public FlexButton SetMargin(string margin) { Margin = margin; return this; }

    #endregion

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["action"] = Action.ToJson() };

        if (!string.IsNullOrEmpty(Style)) obj["style"] = Style;
        if (!string.IsNullOrEmpty(Height)) obj["height"] = Height;
        if (!string.IsNullOrEmpty(Color)) obj["color"] = Color;
        if (Flex.HasValue) obj["flex"] = Flex.Value;
        if (!string.IsNullOrEmpty(Margin)) obj["margin"] = Margin;
        if (!string.IsNullOrEmpty(Gravity)) obj["gravity"] = Gravity;

        return obj;
    }
}
