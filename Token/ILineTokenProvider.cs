namespace LineSDK.Token;

/// <summary>
/// Interface for LINE token provider
/// Allows different token strategies (static, stateless JWT, custom)
/// </summary>
public interface ILineTokenProvider
{
    /// <summary>
    /// Get a valid access token for LINE API calls
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Valid access token</returns>
    Task<string> GetTokenAsync(CancellationToken ct = default);

    /// <summary>
    /// Force refresh the token (if supported)
    /// For static tokens, this is a no-op
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    Task RefreshTokenAsync(CancellationToken ct = default);

    /// <summary>
    /// Check if the current token is valid/not expired
    /// </summary>
    bool IsTokenValid { get; }
}
