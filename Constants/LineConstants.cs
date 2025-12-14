namespace LineSDK.Constants;

/// <summary>
/// LINE Webhook Event Types
/// </summary>
public static class LineEventTypes
{
    public const string Message = "message";
    public const string Follow = "follow";
    public const string Unfollow = "unfollow";
    public const string Join = "join";
    public const string Leave = "leave";
    public const string Postback = "postback";
    public const string MemberJoined = "memberJoined";
    public const string MemberLeft = "memberLeft";
    public const string Beacon = "beacon";
    public const string AccountLink = "accountLink";
    public const string Things = "things";
}

/// <summary>
/// LINE Message Types
/// </summary>
public static class LineMessageTypes
{
    public const string Text = "text";
    public const string Image = "image";
    public const string Video = "video";
    public const string Audio = "audio";
    public const string File = "file";
    public const string Sticker = "sticker";
    public const string Location = "location";
}

/// <summary>
/// LINE Source Types
/// </summary>
public static class LineSourceTypes
{
    public const string User = "user";
    public const string Group = "group";
    public const string Room = "room";
}

/// <summary>
/// LINE Content Provider Types
/// </summary>
public static class LineContentProviderTypes
{
    public const string Line = "line";
    public const string External = "external";
}

/// <summary>
/// LINE Action Types for Flex Messages and Rich Menu
/// </summary>
public static class LineActionTypes
{
    public const string Postback = "postback";
    public const string Message = "message";
    public const string Uri = "uri";
    public const string DatetimePicker = "datetimepicker";
    public const string Camera = "camera";
    public const string CameraRoll = "cameraRoll";
    public const string Location = "location";
    public const string RichMenuSwitch = "richmenuswitch";
}
