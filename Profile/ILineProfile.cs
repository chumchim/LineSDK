using LineSDK.Models;

namespace LineSDK.Profile;

/// <summary>
/// LINE Profile Service - จัดการข้อมูล profile
/// </summary>
public interface ILineProfile
{
    #region User Profile

    /// <summary>
    /// ดึง profile ของ user
    /// </summary>
    Task<LineUserProfile?> GetUserProfileAsync(string userId, CancellationToken ct = default);

    #endregion

    #region Group Members

    /// <summary>
    /// ดึง profile ของ member ใน group
    /// </summary>
    Task<LineUserProfile?> GetGroupMemberProfileAsync(string groupId, string userId, CancellationToken ct = default);

    /// <summary>
    /// ดึงข้อมูล group
    /// </summary>
    Task<LineGroupSummary?> GetGroupSummaryAsync(string groupId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member count ของ group
    /// </summary>
    Task<int> GetGroupMemberCountAsync(string groupId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member IDs ของ group (paged)
    /// </summary>
    Task<LineFollowersResult> GetGroupMemberIdsAsync(string groupId, string? continuationToken = null, CancellationToken ct = default);

    #endregion

    #region Room Members

    /// <summary>
    /// ดึง profile ของ member ใน room
    /// </summary>
    Task<LineUserProfile?> GetRoomMemberProfileAsync(string roomId, string userId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member count ของ room
    /// </summary>
    Task<int> GetRoomMemberCountAsync(string roomId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member IDs ของ room (paged)
    /// </summary>
    Task<LineFollowersResult> GetRoomMemberIdsAsync(string roomId, string? continuationToken = null, CancellationToken ct = default);

    #endregion

    #region Followers

    /// <summary>
    /// ดึง follower IDs (paged)
    /// </summary>
    Task<LineFollowersResult> GetFollowerIdsAsync(string? continuationToken = null, CancellationToken ct = default);

    #endregion
}
