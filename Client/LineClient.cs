using LineSDK.Messaging;
using LineSDK.Notify;
using LineSDK.Profile;

namespace LineSDK.Client;

/// <summary>
/// LINE Client Implementation
/// </summary>
public class LineClient : ILineClient
{
    public LineClient(
        ILineMessaging messaging,
        ILineNotify notify,
        ILineProfile profile)
    {
        Messaging = messaging;
        Notify = notify;
        Profile = profile;
    }

    /// <inheritdoc />
    public ILineMessaging Messaging { get; }

    /// <inheritdoc />
    public ILineNotify Notify { get; }

    /// <inheritdoc />
    public ILineProfile Profile { get; }
}
