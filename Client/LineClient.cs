using LineSDK.Messaging;
using LineSDK.Notify;
using LineSDK.Profile;
using LineSDK.RichMenu;

namespace LineSDK.Client;

/// <summary>
/// LINE Client Implementation
/// </summary>
public class LineClient : ILineClient
{
    public LineClient(
        ILineMessaging messaging,
        ILineNotify notify,
        ILineProfile profile,
        ILineRichMenu richMenu)
    {
        Messaging = messaging;
        Notify = notify;
        Profile = profile;
        RichMenu = richMenu;
    }

    /// <inheritdoc />
    public ILineMessaging Messaging { get; }

    /// <inheritdoc />
    public ILineNotify Notify { get; }

    /// <inheritdoc />
    public ILineProfile Profile { get; }

    /// <inheritdoc />
    public ILineRichMenu RichMenu { get; }
}
