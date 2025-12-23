# LineSDK - LINE Messaging API Wrapper

Custom SDK for LINE Messaging API integration.

---

## API Requirements & Account Types

### LINE Account Types (Shield Icons)

| Account Type | Icon | Verification | Features Available |
|--------------|:----:|--------------|-------------------|
| **Unverified** | 🔘 Grey | None | Basic messaging only |
| **Verified** | 🔵 Blue | Business verified | Group Member APIs |
| **Premium** | 🟢 Green | Premium plan | All features |

### Feature Availability by Account Type

| API / Feature | 🔘 Grey | 🔵 Blue | 🟢 Green | Notes |
|---------------|:-------:|:-------:|:--------:|-------|
| Push Message | ✅ | ✅ | ✅ | Basic feature |
| Reply Message | ✅ | ✅ | ✅ | Basic feature |
| Multicast | ✅ | ✅ | ✅ | Up to 500 users |
| Broadcast | ❌ | ✅ | ✅ | All followers |
| Get User Profile | ✅ | ✅ | ✅ | 1:1 chat only |
| Get Group Summary | ✅ | ✅ | ✅ | Group name, picture |
| **Get Group Member IDs** | ❌ | ✅ | ✅ | **Verified+ required!** |
| **Get Group Member Profile** | ❌ | ✅ | ✅ | **Verified+ required!** |
| **Get Group Member Count** | ❌ | ✅ | ✅ | **Verified+ required!** |
| Get Follower IDs | ❌ | ✅ | ✅ | Verified+ required |
| Rich Menu | ✅ | ✅ | ✅ | Basic feature |

### Important Behaviors

#### Push to Blocked User (Silent Failure)

```
⚠️ WARNING: Push to blocked user returns 200 OK (no error)
```

When you push a message to a user who has blocked your bot:
- API returns `200 OK` (success)
- No error thrown
- Message is **NOT** delivered
- **You cannot detect this failure!**

**Workaround**: Check follow status before pushing:
```csharp
// Check if user is following before push
var profile = await lineClient.Profile.GetUserProfileAsync(userId);
if (profile == null)
{
    // User may have blocked or unfollowed
    logger.LogWarning("Cannot reach user {UserId}", userId);
}
```

#### Group Limitations

| Limitation | Value | Notes |
|------------|-------|-------|
| Max bots per group | 1 | Cannot add multiple bots |
| Group member limit | 500 | LINE group limit |
| Message length | 5,000 chars | Per message |

---

## Service Reference

### ILineProfile (Profile Service)

```csharp
public interface ILineProfile
{
    // ✅ Available for all account types
    Task<LineUserProfile?> GetUserProfileAsync(string userId);
    Task<LineGroupSummary?> GetGroupSummaryAsync(string groupId);

    // ⚠️ VERIFIED+ ACCOUNT REQUIRED
    Task<LineUserProfile?> GetGroupMemberProfileAsync(string groupId, string userId);
    Task<int> GetGroupMemberCountAsync(string groupId);
    Task<LineFollowersResult> GetGroupMemberIdsAsync(string groupId, string? continuationToken = null);

    // ⚠️ VERIFIED+ ACCOUNT REQUIRED
    Task<LineFollowersResult> GetFollowerIdsAsync(string? continuationToken = null);
}
```

### ILineMessaging (Messaging Service)

```csharp
public interface ILineMessaging
{
    // ✅ Available for all account types
    Task PushAsync(string message, string to);
    Task PushAsync(ILineMessage message, string to);
    Task ReplyAsync(string message, string replyToken);
    Task MulticastAsync(string message, IEnumerable<string> userIds);

    // ⚠️ VERIFIED+ ACCOUNT REQUIRED
    Task BroadcastAsync(string message);
    Task BroadcastAsync(ILineMessage message);
}
```

---

## Error Handling

### Common Error Codes

| Status | Code | Meaning |
|--------|------|---------|
| 400 | Invalid request | Check request body format |
| 401 | Unauthorized | Invalid channel access token |
| 403 | Forbidden | **Feature requires Verified+ account** |
| 404 | Not found | User/group not found or not reachable |
| 429 | Rate limited | Too many requests |

### Handling 403 Forbidden

```csharp
try
{
    var memberIds = await lineClient.Profile.GetGroupMemberIdsAsync(groupId);
}
catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.Forbidden)
{
    // This API requires Verified (Blue) or Premium (Green) account
    logger.LogError("GetGroupMemberIds requires Verified+ account. " +
        "Apply for verification at LINE Developers Console.");
}
```

---

## Rate Limits

| API Type | Limit | Notes |
|----------|-------|-------|
| Push/Reply | 100,000/min | Per channel |
| Multicast | 100,000/min | Per channel |
| Broadcast | 100,000/min | Verified+ only |
| Profile APIs | 2,000/min | Per channel |

---

## Official Documentation

- [LINE Messaging API](https://developers.line.biz/en/docs/messaging-api/)
- [Account Types](https://developers.line.biz/en/docs/messaging-api/getting-started/#account-types)
- [Get Group Member IDs](https://developers.line.biz/en/reference/messaging-api/#get-group-member-user-ids)
- [Rate Limits](https://developers.line.biz/en/reference/messaging-api/#rate-limits)

---

## Version History

| Version | Date | Changes |
|---------|------|---------|
| 1.1.0 | 2025-12-23 | Added API requirements documentation |
| 1.0.0 | 2025-12-01 | Initial release |
