using System.Text.Json.Serialization;

namespace LineSDK.Templates.Components;

/// <summary>
/// Flex Image - รูปภาพ
/// </summary>
public class FlexImage : IFlexComponent
{
    [JsonPropertyName("type")]
    public string Type => "image";

    [JsonPropertyName("url")]
    public string Url { get; set; } = "";

    #region Styling Properties

    /// <summary>
    /// Size: xxs, xs, sm, md, lg, xl, xxl, 3xl, 4xl, 5xl, full
    /// </summary>
    [JsonPropertyName("size")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Size { get; set; }

    /// <summary>
    /// Aspect Ratio: W:H (e.g., "20:13", "1:1")
    /// </summary>
    [JsonPropertyName("aspectRatio")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AspectRatio { get; set; }

    /// <summary>
    /// Aspect Mode: cover, fit
    /// </summary>
    [JsonPropertyName("aspectMode")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AspectMode { get; set; }

    [JsonPropertyName("backgroundColor")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? BackgroundColor { get; set; }

    [JsonPropertyName("flex")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Flex { get; set; }

    [JsonPropertyName("margin")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Margin { get; set; }

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

    #endregion

    [JsonPropertyName("action")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IFlexAction? Action { get; set; }

    public FlexImage() { }
    public FlexImage(string url) => Url = url;

    #region Fluent Methods

    public FlexImage SetSize(string size) { Size = size; return this; }
    public FlexImage SetAspectRatio(string ratio) { AspectRatio = ratio; return this; }
    public FlexImage SetAspectMode(string mode) { AspectMode = mode; return this; }
    public FlexImage Cover() { AspectMode = "cover"; return this; }
    public FlexImage Fit() { AspectMode = "fit"; return this; }

    #endregion

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["url"] = Url };

        if (!string.IsNullOrEmpty(Size)) obj["size"] = Size;
        if (!string.IsNullOrEmpty(AspectRatio)) obj["aspectRatio"] = AspectRatio;
        if (!string.IsNullOrEmpty(AspectMode)) obj["aspectMode"] = AspectMode;
        if (!string.IsNullOrEmpty(BackgroundColor)) obj["backgroundColor"] = BackgroundColor;
        if (Flex.HasValue) obj["flex"] = Flex.Value;
        if (!string.IsNullOrEmpty(Margin)) obj["margin"] = Margin;
        if (!string.IsNullOrEmpty(Align)) obj["align"] = Align;
        if (!string.IsNullOrEmpty(Gravity)) obj["gravity"] = Gravity;
        if (Action != null) obj["action"] = Action.ToJson();

        return obj;
    }
}
