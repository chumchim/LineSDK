using System.Text.Json.Serialization;

namespace LineSDK.Templates;

/// <summary>
/// Base interface สำหรับ Flex Action
/// </summary>
public interface IFlexAction
{
    string Type { get; }
    object ToJson();
}

/// <summary>
/// URI Action - เปิด URL
/// </summary>
public class UriAction : IFlexAction
{
    [JsonPropertyName("type")]
    public string Type => "uri";

    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }

    [JsonPropertyName("uri")]
    public string Uri { get; set; } = "";

    public UriAction() { }
    public UriAction(string uri, string? label = null)
    {
        Uri = uri;
        Label = label;
    }

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["uri"] = Uri };
        if (!string.IsNullOrEmpty(Label)) obj["label"] = Label;
        return obj;
    }
}

/// <summary>
/// Message Action - ส่งข้อความ
/// </summary>
public class MessageAction : IFlexAction
{
    [JsonPropertyName("type")]
    public string Type => "message";

    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = "";

    public MessageAction() { }
    public MessageAction(string text, string? label = null)
    {
        Text = text;
        Label = label;
    }

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["text"] = Text };
        if (!string.IsNullOrEmpty(Label)) obj["label"] = Label;
        return obj;
    }
}

/// <summary>
/// Postback Action - ส่ง postback data
/// </summary>
public class PostbackAction : IFlexAction
{
    [JsonPropertyName("type")]
    public string Type => "postback";

    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; } = "";

    [JsonPropertyName("displayText")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? DisplayText { get; set; }

    [JsonPropertyName("text")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Text { get; set; }

    public PostbackAction() { }
    public PostbackAction(string data, string? label = null, string? displayText = null)
    {
        Data = data;
        Label = label;
        DisplayText = displayText;
    }

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["data"] = Data };
        if (!string.IsNullOrEmpty(Label)) obj["label"] = Label;
        if (!string.IsNullOrEmpty(DisplayText)) obj["displayText"] = DisplayText;
        if (!string.IsNullOrEmpty(Text)) obj["text"] = Text;
        return obj;
    }
}

/// <summary>
/// Datetime Picker Action
/// </summary>
public class DatetimePickerAction : IFlexAction
{
    [JsonPropertyName("type")]
    public string Type => "datetimepicker";

    [JsonPropertyName("label")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Label { get; set; }

    [JsonPropertyName("data")]
    public string Data { get; set; } = "";

    /// <summary>
    /// Mode: date, time, datetime
    /// </summary>
    [JsonPropertyName("mode")]
    public string Mode { get; set; } = "datetime";

    [JsonPropertyName("initial")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Initial { get; set; }

    [JsonPropertyName("max")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Max { get; set; }

    [JsonPropertyName("min")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Min { get; set; }

    public object ToJson()
    {
        var obj = new Dictionary<string, object> { ["type"] = Type, ["data"] = Data, ["mode"] = Mode };
        if (!string.IsNullOrEmpty(Label)) obj["label"] = Label;
        if (!string.IsNullOrEmpty(Initial)) obj["initial"] = Initial;
        if (!string.IsNullOrEmpty(Max)) obj["max"] = Max;
        if (!string.IsNullOrEmpty(Min)) obj["min"] = Min;
        return obj;
    }
}
