using LineSDK.Client;
using LineSDK.Messaging;
using LineSDK.Notify;
using LineSDK.Options;
using LineSDK.Profile;
using LineSDK.RichMenu;
using LineSDK.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace LineSDK.Extensions;

/// <summary>
/// DI Extensions for LINE SDK
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// เพิ่ม LINE Client เข้า DI Container
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configure">Configure options</param>
    /// <returns>IServiceCollection</returns>
    /// <example>
    /// <code>
    /// // Static token mode (default)
    /// services.AddLineClient(options => {
    ///     options.ChannelAccessToken = "your-token";
    ///     options.ChannelSecret = "your-secret";
    /// });
    ///
    /// // Stateless token mode (recommended for production)
    /// services.AddLineClient(options => {
    ///     options.TokenMode = LineTokenMode.Stateless;
    ///     options.ChannelId = "your-channel-id";
    ///     options.ChannelSecret = "your-secret";
    /// });
    /// </code>
    /// </example>
    public static IServiceCollection AddLineClient(
        this IServiceCollection services,
        Action<LineClientOptions> configure)
    {
        // Configure options
        services.Configure(configure);

        // Get options to determine token mode
        var options = new LineClientOptions();
        configure(options);

        // Register token provider based on mode
        services.AddLineTokenProvider(options.TokenMode);

        // Validate options on startup
        services.PostConfigure<LineClientOptions>(opt => opt.Validate());

        return services.AddLineClientCore();
    }

    /// <summary>
    /// เพิ่ม LINE Client เข้า DI Container จาก Configuration
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration section</param>
    /// <returns>IServiceCollection</returns>
    /// <example>
    /// <code>
    /// // appsettings.json:
    /// // "Line": {
    /// //   "TokenMode": "Static",  // or "Stateless"
    /// //   "ChannelAccessToken": "your-token",  // for Static mode
    /// //   "ChannelId": "your-channel-id",      // for Stateless mode
    /// //   "ChannelSecret": "your-secret"
    /// // }
    ///
    /// services.AddLineClient(configuration.GetSection("Line"));
    /// </code>
    /// </example>
    public static IServiceCollection AddLineClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Bind from configuration
        services.Configure<LineClientOptions>(configuration);

        // Get token mode from configuration
        var tokenModeStr = configuration.GetValue<string>("TokenMode");
        var tokenMode = Enum.TryParse<LineTokenMode>(tokenModeStr, true, out var mode)
            ? mode
            : LineTokenMode.Static; // Default to Static for backward compatibility

        // Register token provider based on mode
        services.AddLineTokenProvider(tokenMode);

        // Validate options on startup
        services.PostConfigure<LineClientOptions>(options => options.Validate());

        return services.AddLineClientCore();
    }

    /// <summary>
    /// เพิ่ม LINE Client เข้า DI Container จาก Configuration Section Name
    /// </summary>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Full configuration</param>
    /// <param name="sectionName">Section name (default: "Line")</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddLineClient(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = LineClientOptions.SectionName)
    {
        return services.AddLineClient(configuration.GetSection(sectionName));
    }

    /// <summary>
    /// เพิ่ม LINE Client with custom token provider
    /// Use this when you need a custom token management strategy
    /// </summary>
    /// <typeparam name="TTokenProvider">Custom token provider type</typeparam>
    /// <param name="services">Service collection</param>
    /// <param name="configuration">Configuration section</param>
    /// <returns>IServiceCollection</returns>
    public static IServiceCollection AddLineClient<TTokenProvider>(
        this IServiceCollection services,
        IConfiguration configuration)
        where TTokenProvider : class, ILineTokenProvider
    {
        // Bind from configuration
        services.Configure<LineClientOptions>(configuration);

        // Register custom token provider
        services.AddSingleton<ILineTokenProvider, TTokenProvider>();

        // Validate options on startup
        services.PostConfigure<LineClientOptions>(options => options.Validate());

        return services.AddLineClientCore();
    }

    /// <summary>
    /// Register token provider based on token mode
    /// </summary>
    private static IServiceCollection AddLineTokenProvider(
        this IServiceCollection services,
        LineTokenMode tokenMode)
    {
        switch (tokenMode)
        {
            case LineTokenMode.Stateless:
                // Stateless provider needs HttpClient for token issuance
                services.AddHttpClient<ILineTokenProvider, StatelessTokenProvider>();
                break;

            case LineTokenMode.Static:
            default:
                // Static provider doesn't need HttpClient
                services.AddSingleton<ILineTokenProvider, StaticTokenProvider>();
                break;
        }

        return services;
    }

    private static IServiceCollection AddLineClientCore(this IServiceCollection services)
    {
        // Register HttpClient for services
        services.AddHttpClient<ILineMessaging, LineMessagingService>();
        services.AddHttpClient<ILineProfile, LineProfileService>();
        services.AddHttpClient<ILineRichMenu, LineRichMenuService>();

        // Register services
        services.AddScoped<ILineNotify, LineNotifyService>();
        services.AddScoped<ILineClient, LineClient>();

        return services;
    }
}
