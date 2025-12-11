using LineSDK.Messages;

namespace LineSDK.Messaging;

/// <summary>
/// LINE Messaging Service - ส่งข้อความพื้นฐาน
/// </summary>
public interface ILineMessaging
{
    #region Push (ส่งหาคน/กลุ่ม)

    /// <summary>
    /// Push text message ไปหา user/group/room
    /// </summary>
    Task PushAsync(string message, string to, CancellationToken ct = default);

    /// <summary>
    /// Push LINE message ไปหา user/group/room
    /// </summary>
    Task PushAsync(ILineMessage message, string to, CancellationToken ct = default);

    /// <summary>
    /// Push หลาย messages ไปหา user/group/room (สูงสุด 5)
    /// </summary>
    Task PushAsync(IEnumerable<ILineMessage> messages, string to, CancellationToken ct = default);

    #endregion

    #region Reply (ตอบกลับ - ฟรี!)

    /// <summary>
    /// Reply text message (ฟรี! ต้องใช้ภายใน 1 นาที)
    /// </summary>
    Task ReplyAsync(string message, string replyToken, CancellationToken ct = default);

    /// <summary>
    /// Reply LINE message (ฟรี! ต้องใช้ภายใน 1 นาที)
    /// </summary>
    Task ReplyAsync(ILineMessage message, string replyToken, CancellationToken ct = default);

    /// <summary>
    /// Reply หลาย messages (ฟรี! สูงสุด 5)
    /// </summary>
    Task ReplyAsync(IEnumerable<ILineMessage> messages, string replyToken, CancellationToken ct = default);

    #endregion

    #region Multicast (ส่งหาหลายคน)

    /// <summary>
    /// Multicast text message ไปหาหลายคน (สูงสุด 500)
    /// </summary>
    Task MulticastAsync(string message, IEnumerable<string> userIds, CancellationToken ct = default);

    /// <summary>
    /// Multicast LINE message ไปหาหลายคน (สูงสุด 500)
    /// </summary>
    Task MulticastAsync(ILineMessage message, IEnumerable<string> userIds, CancellationToken ct = default);

    #endregion

    #region Broadcast (ส่งหาทุกคน)

    /// <summary>
    /// Broadcast text message ไปหาทุกคนที่เป็นเพื่อน
    /// </summary>
    Task BroadcastAsync(string message, CancellationToken ct = default);

    /// <summary>
    /// Broadcast LINE message ไปหาทุกคนที่เป็นเพื่อน
    /// </summary>
    Task BroadcastAsync(ILineMessage message, CancellationToken ct = default);

    #endregion
}
