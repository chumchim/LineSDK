using LineSDK.Models;

namespace LineSDK.Profile;

/// <summary>
/// LINE Profile Service - จัดการข้อมูล profile
/// </summary>
/// <remarks>
/// <para><b>Account Type Requirements:</b></para>
/// <para>Some APIs require Verified (Blue) or Premium (Green) account.</para>
/// <para>See README.md for full feature matrix.</para>
/// </remarks>
public interface ILineProfile
{
    #region User Profile

    /// <summary>
    /// ดึง profile ของ user
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> All (Grey/Blue/Green)</para>
    /// <para><b>Note:</b> Only works for users who have added your bot as friend.</para>
    /// <para><b>Warning:</b> Returns null if user has blocked or unfollowed.</para>
    /// </remarks>
    /// <param name="userId">LINE user ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>User profile or null if not found/blocked</returns>
    Task<LineUserProfile?> GetUserProfileAsync(string userId, CancellationToken ct = default);

    #endregion

    #region Group Members

    /// <summary>
    /// ดึง profile ของ member ใน group
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// <para>See: https://developers.line.biz/en/reference/messaging-api/#get-group-member-profile</para>
    /// </remarks>
    /// <param name="groupId">LINE group ID</param>
    /// <param name="userId">LINE user ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Member profile or null</returns>
    Task<LineUserProfile?> GetGroupMemberProfileAsync(string groupId, string userId, CancellationToken ct = default);

    /// <summary>
    /// ดึงข้อมูล group (ชื่อ, รูปภาพ)
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> All (Grey/Blue/Green)</para>
    /// <para>See: https://developers.line.biz/en/reference/messaging-api/#get-group-summary</para>
    /// </remarks>
    /// <param name="groupId">LINE group ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Group summary or null</returns>
    Task<LineGroupSummary?> GetGroupSummaryAsync(string groupId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member count ของ group
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// <para>See: https://developers.line.biz/en/reference/messaging-api/#get-group-member-count</para>
    /// </remarks>
    /// <param name="groupId">LINE group ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Member count or 0 if error</returns>
    Task<int> GetGroupMemberCountAsync(string groupId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member IDs ของ group (paged)
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// <para><b>Pagination:</b> Use continuationToken from previous result for next page</para>
    /// <para>See: https://developers.line.biz/en/reference/messaging-api/#get-group-member-user-ids</para>
    /// </remarks>
    /// <param name="groupId">LINE group ID</param>
    /// <param name="continuationToken">Token for next page (null for first page)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of member IDs with optional continuation token</returns>
    Task<LineFollowersResult> GetGroupMemberIdsAsync(string groupId, string? continuationToken = null, CancellationToken ct = default);

    #endregion

    #region Room Members

    /// <summary>
    /// ดึง profile ของ member ใน room
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// </remarks>
    /// <param name="roomId">LINE room ID</param>
    /// <param name="userId">LINE user ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Member profile or null</returns>
    Task<LineUserProfile?> GetRoomMemberProfileAsync(string roomId, string userId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member count ของ room
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// </remarks>
    /// <param name="roomId">LINE room ID</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Member count or 0 if error</returns>
    Task<int> GetRoomMemberCountAsync(string roomId, CancellationToken ct = default);

    /// <summary>
    /// ดึง member IDs ของ room (paged)
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// <para><b>Pagination:</b> Use continuationToken from previous result for next page</para>
    /// </remarks>
    /// <param name="roomId">LINE room ID</param>
    /// <param name="continuationToken">Token for next page (null for first page)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of member IDs with optional continuation token</returns>
    Task<LineFollowersResult> GetRoomMemberIdsAsync(string roomId, string? continuationToken = null, CancellationToken ct = default);

    #endregion

    #region Followers

    /// <summary>
    /// ดึง follower IDs (paged)
    /// </summary>
    /// <remarks>
    /// <para><b>Account Type:</b> Verified (Blue) or Premium (Green) ONLY</para>
    /// <para><b>Error 403:</b> Returns Forbidden if using Unverified (Grey) account</para>
    /// <para><b>Pagination:</b> Use continuationToken from previous result for next page</para>
    /// <para>See: https://developers.line.biz/en/reference/messaging-api/#get-follower-ids</para>
    /// </remarks>
    /// <param name="continuationToken">Token for next page (null for first page)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of follower IDs with optional continuation token</returns>
    Task<LineFollowersResult> GetFollowerIdsAsync(string? continuationToken = null, CancellationToken ct = default);

    #endregion
}
