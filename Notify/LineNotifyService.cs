using LineSDK.Messages;
using LineSDK.Messaging;
using LineSDK.Options;
using Microsoft.Extensions.Options;

namespace LineSDK.Notify;

/// <summary>
/// LINE Notify Service Implementation
/// </summary>
public class LineNotifyService : ILineNotify
{
    private readonly ILineMessaging _messaging;
    private readonly LineClientOptions _options;

    public LineNotifyService(
        ILineMessaging messaging,
        IOptions<LineClientOptions> options)
    {
        _messaging = messaging;
        _options = options.Value;
    }

    #region Queue Notifications

    public async Task NotifyNewQueueAsync(string agentUserId, string customerName, string platform, CancellationToken ct = default)
    {
        var message = $"📥 ลูกค้าใหม่รอใน Queue\n\n" +
                      $"👤 ชื่อ: {customerName}\n" +
                      $"📱 Platform: {platform}\n\n" +
                      $"กรุณารับเรื่องที่ระบบ Call Center";

        await _messaging.PushAsync(message, agentUserId, ct);
    }

    public async Task NotifyNewQueueToGroupAsync(string groupId, string customerName, string platform, CancellationToken ct = default)
    {
        var message = $"📥 ลูกค้าใหม่รอใน Queue\n\n" +
                      $"👤 ชื่อ: {customerName}\n" +
                      $"📱 Platform: {platform}\n\n" +
                      $"Agent ท่านใดสะดวก กรุณารับเรื่องด้วยครับ";

        await _messaging.PushAsync(message, groupId, ct);
    }

    #endregion

    #region Agent Notifications

    public async Task NotifyAgentApprovedAsync(string agentUserId, CancellationToken ct = default)
    {
        var message = "✅ ยินดีด้วย! บัญชี Agent ของคุณได้รับการอนุมัติแล้ว\n\n" +
                      "คุณสามารถเข้าใช้งานระบบ Call Center ได้ทันที";

        // เพิ่ม LIFF URL ถ้ามี
        if (!string.IsNullOrEmpty(_options.LiffRegisterUrl))
        {
            message += $"\n\n🔗 เข้าสู่ระบบ: {_options.LiffRegisterUrl}";
        }

        await _messaging.PushAsync(message, agentUserId, ct);
    }

    public async Task NotifyAgentRejectedAsync(string agentUserId, string? reason = null, CancellationToken ct = default)
    {
        var message = "❌ ขออภัย บัญชี Agent ของคุณไม่ได้รับการอนุมัติ";

        if (!string.IsNullOrEmpty(reason))
        {
            message += $"\n\nเหตุผล: {reason}";
        }

        message += "\n\nหากมีข้อสงสัย กรุณาติดต่อผู้ดูแลระบบ";

        await _messaging.PushAsync(message, agentUserId, ct);
    }

    #endregion

    #region Customer Notifications

    public async Task NotifyAgentAssignedAsync(string customerUserId, string agentName, CancellationToken ct = default)
    {
        var message = $"✨ สวัสดีครับ/ค่ะ\n\n" +
                      $"ขณะนี้เจ้าหน้าที่ {agentName} รับเรื่องของคุณแล้ว\n" +
                      $"กรุณารอสักครู่ เจ้าหน้าที่จะติดต่อกลับในเร็ว ๆ นี้";

        await _messaging.PushAsync(message, customerUserId, ct);
    }

    public async Task NotifyConversationClosedAsync(string customerUserId, CancellationToken ct = default)
    {
        var message = "✅ การสนทนาจบลงแล้ว\n\n" +
                      "ขอบคุณที่ใช้บริการครับ/ค่ะ\n" +
                      "หากมีข้อสงสัยเพิ่มเติม สามารถส่งข้อความมาได้เลย";

        await _messaging.PushAsync(message, customerUserId, ct);
    }

    #endregion

    #region Custom Notifications

    public Task SendFlexNotificationAsync(string to, ILineMessage flexMessage, CancellationToken ct = default)
        => _messaging.PushAsync(flexMessage, to, ct);

    #endregion
}
