using LineSDK.Options;
using Microsoft.Extensions.Options;

namespace LineSDK.Token;

/// <summary>
/// Static token provider - uses long-lived Channel Access Token directly
/// Simple and suitable for development environments
/// </summary>
public class StaticTokenProvider : ILineTokenProvider
{
    private readonly string _token;

    public StaticTokenProvider(IOptions<LineClientOptions> options)
    {
        _token = options.Value.ChannelAccessToken;

        if (string.IsNullOrEmpty(_token))
        {
            throw new InvalidOperationException(
                "ChannelAccessToken is required for Static token mode. " +
                "Please configure Line:ChannelAccessToken in your settings or environment variables.");
        }
    }

    /// <inheritdoc />
    public Task<string> GetTokenAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_token);
    }

    /// <inheritdoc />
    public Task RefreshTokenAsync(CancellationToken ct = default)
    {
        // Static tokens cannot be refreshed - this is a no-op
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    public bool IsTokenValid => !string.IsNullOrEmpty(_token);
}
