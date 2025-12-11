using LineSDK.Messages;

namespace LineSDK.Notify;

/// <summary>
/// LINE Notify Service - ส่ง notifications ทางธุรกิจ
/// </summary>
public interface ILineNotify
{
    #region Queue Notifications

    /// <summary>
    /// แจ้ง agent ว่ามีลูกค้ารอใน queue
    /// </summary>
    Task NotifyNewQueueAsync(string agentUserId, string customerName, string platform, CancellationToken ct = default);

    /// <summary>
    /// แจ้ง group ว่ามีลูกค้ารอใน queue
    /// </summary>
    Task NotifyNewQueueToGroupAsync(string groupId, string customerName, string platform, CancellationToken ct = default);

    #endregion

    #region Agent Notifications

    /// <summary>
    /// แจ้ง agent ว่าถูก approve แล้ว
    /// </summary>
    Task NotifyAgentApprovedAsync(string agentUserId, CancellationToken ct = default);

    /// <summary>
    /// แจ้ง agent ว่าถูก reject
    /// </summary>
    Task NotifyAgentRejectedAsync(string agentUserId, string? reason = null, CancellationToken ct = default);

    #endregion

    #region Customer Notifications

    /// <summary>
    /// แจ้งลูกค้าว่า agent รับเรื่องแล้ว
    /// </summary>
    Task NotifyAgentAssignedAsync(string customerUserId, string agentName, CancellationToken ct = default);

    /// <summary>
    /// แจ้งลูกค้าว่าการสนทนาจบแล้ว
    /// </summary>
    Task NotifyConversationClosedAsync(string customerUserId, CancellationToken ct = default);

    #endregion

    #region Custom Notifications

    /// <summary>
    /// ส่ง notification ด้วย Flex Message
    /// </summary>
    Task SendFlexNotificationAsync(string to, ILineMessage flexMessage, CancellationToken ct = default);

    #endregion
}
