namespace LineSDK.RichMenu;

/// <summary>
/// LINE Rich Menu Service - จัดการ Rich Menu
///
/// Rich Menu คือเมนูที่แสดงด้านล่างห้องแชท
/// สามารถสร้าง Rich Menu หลายแบบและสลับให้ User ต่างๆ ได้
/// </summary>
public interface ILineRichMenu
{
    #region Create/Delete

    /// <summary>
    /// สร้าง Rich Menu ใหม่ (ยังไม่มีรูปภาพ)
    /// </summary>
    /// <returns>Rich Menu ID</returns>
    Task<string> CreateAsync(RichMenuRequest request, CancellationToken ct = default);

    /// <summary>
    /// อัพโหลดรูปภาพให้ Rich Menu
    /// </summary>
    Task UploadImageAsync(string richMenuId, byte[] imageData, string contentType = "image/png", CancellationToken ct = default);

    /// <summary>
    /// ลบ Rich Menu
    /// </summary>
    Task DeleteAsync(string richMenuId, CancellationToken ct = default);

    #endregion

    #region Link/Unlink

    /// <summary>
    /// Link Rich Menu ให้ User (User จะเห็น Rich Menu นี้)
    /// </summary>
    Task LinkToUserAsync(string richMenuId, string userId, CancellationToken ct = default);

    /// <summary>
    /// Unlink Rich Menu จาก User (User จะไม่เห็น Rich Menu)
    /// </summary>
    Task UnlinkFromUserAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// Link Rich Menu ให้หลาย Users พร้อมกัน (สูงสุด 500)
    /// </summary>
    Task LinkToMultipleUsersAsync(string richMenuId, IEnumerable<string> userIds, CancellationToken ct = default);

    /// <summary>
    /// Unlink Rich Menu จากหลาย Users พร้อมกัน (สูงสุด 500)
    /// </summary>
    Task UnlinkFromMultipleUsersAsync(IEnumerable<string> userIds, CancellationToken ct = default);

    #endregion

    #region Default

    /// <summary>
    /// ตั้ง Rich Menu เป็น default (แสดงให้ทุก User ที่ไม่มี Rich Menu ส่วนตัว)
    /// </summary>
    Task SetDefaultAsync(string richMenuId, CancellationToken ct = default);

    /// <summary>
    /// ดึง default Rich Menu ID
    /// </summary>
    Task<string?> GetDefaultIdAsync(CancellationToken ct = default);

    /// <summary>
    /// ยกเลิก default Rich Menu
    /// </summary>
    Task CancelDefaultAsync(CancellationToken ct = default);

    #endregion

    #region Query

    /// <summary>
    /// ดึง Rich Menu ที่ User นี้ใช้อยู่
    /// </summary>
    Task<string?> GetUserRichMenuIdAsync(string userId, CancellationToken ct = default);

    /// <summary>
    /// ดึงรายการ Rich Menu ทั้งหมด
    /// </summary>
    Task<IEnumerable<RichMenuInfo>> GetAllAsync(CancellationToken ct = default);

    /// <summary>
    /// ดึงข้อมูล Rich Menu จาก ID
    /// </summary>
    Task<RichMenuInfo?> GetAsync(string richMenuId, CancellationToken ct = default);

    #endregion
}

/// <summary>
/// ข้อมูลสำหรับสร้าง Rich Menu
/// </summary>
public class RichMenuRequest
{
    /// <summary>
    /// Size ของ Rich Menu (width x height)
    /// </summary>
    public RichMenuSize Size { get; set; } = new() { Width = 2500, Height = 1686 };

    /// <summary>
    /// แสดง Rich Menu โดย default เมื่อเปิดแชทหรือไม่
    /// </summary>
    public bool Selected { get; set; } = true;

    /// <summary>
    /// ชื่อ Rich Menu (แสดงใน LINE Official Account Manager)
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// ข้อความแสดงใน Chat Bar
    /// </summary>
    public string ChatBarText { get; set; } = "เมนู";

    /// <summary>
    /// พื้นที่กดได้ใน Rich Menu
    /// </summary>
    public List<RichMenuArea> Areas { get; set; } = new();
}

/// <summary>
/// Size ของ Rich Menu
/// </summary>
public class RichMenuSize
{
    public int Width { get; set; }
    public int Height { get; set; }
}

/// <summary>
/// พื้นที่กดได้ใน Rich Menu
/// </summary>
public class RichMenuArea
{
    /// <summary>
    /// ขอบเขตพื้นที่ (x, y, width, height)
    /// </summary>
    public RichMenuBounds Bounds { get; set; } = new();

    /// <summary>
    /// Action เมื่อกดพื้นที่นี้
    /// </summary>
    public RichMenuAction Action { get; set; } = new();
}

/// <summary>
/// ขอบเขตพื้นที่กด
/// </summary>
public class RichMenuBounds
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}

/// <summary>
/// Action เมื่อกดพื้นที่ใน Rich Menu
/// </summary>
public class RichMenuAction
{
    /// <summary>
    /// ประเภท Action: postback, message, uri, richmenuswitch
    /// </summary>
    public string Type { get; set; } = "postback";

    /// <summary>
    /// Label (สำหรับ accessibility)
    /// </summary>
    public string? Label { get; set; }

    /// <summary>
    /// Data (สำหรับ postback)
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// Text (สำหรับ message type)
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Uri (สำหรับ uri type)
    /// </summary>
    public string? Uri { get; set; }

    /// <summary>
    /// Rich Menu Alias ID (สำหรับ richmenuswitch type)
    /// </summary>
    public string? RichMenuAliasId { get; set; }
}

/// <summary>
/// ข้อมูล Rich Menu (จาก API response)
/// </summary>
public class RichMenuInfo
{
    public string RichMenuId { get; set; } = "";
    public RichMenuSize Size { get; set; } = new();
    public bool Selected { get; set; }
    public string Name { get; set; } = "";
    public string ChatBarText { get; set; } = "";
    public List<RichMenuArea> Areas { get; set; } = new();
}
