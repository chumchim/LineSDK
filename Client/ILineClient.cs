using LineSDK.Messaging;
using LineSDK.Notify;
using LineSDK.Profile;

namespace LineSDK.Client;

/// <summary>
/// LINE Client - Entry point สำหรับใช้งาน LINE SDK
/// </summary>
/// <remarks>
/// ใช้งาน:
/// <code>
/// // DI
/// services.AddLineClient(options => {
///     options.ChannelAccessToken = "...";
///     options.ChannelSecret = "...";
/// });
///
/// // Inject
/// public class MyService(ILineClient line)
/// {
///     await line.Messaging.PushAsync("Hello", userId);
///     await line.Notify.NotifyNewQueueAsync(agentId, "Customer", "LINE");
///     var profile = await line.Profile.GetUserProfileAsync(userId);
/// }
/// </code>
/// </remarks>
public interface ILineClient
{
    /// <summary>
    /// Messaging - ส่งข้อความพื้นฐาน (Push, Reply, Multicast, Broadcast)
    /// </summary>
    ILineMessaging Messaging { get; }

    /// <summary>
    /// Notify - ส่ง notifications ทางธุรกิจ
    /// </summary>
    ILineNotify Notify { get; }

    /// <summary>
    /// Profile - จัดการข้อมูล profile
    /// </summary>
    ILineProfile Profile { get; }
}
