using LineSDK.Client;
using LineSDK.Messaging;
using LineSDK.Notify;
using LineSDK.Options;
using LineSDK.Profile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
    /// services.AddLineClient(options => {
    ///     options.ChannelAccessToken = "your-token";
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

        // Validate options on startup
        services.PostConfigure<LineClientOptions>(options => options.Validate());

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
    /// //   "ChannelAccessToken": "your-token",
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

    private static IServiceCollection AddLineClientCore(this IServiceCollection services)
    {
        // Register HttpClient
        services.AddHttpClient<ILineMessaging, LineMessagingService>();
        services.AddHttpClient<ILineProfile, LineProfileService>();

        // Register services
        services.AddScoped<ILineNotify, LineNotifyService>();
        services.AddScoped<ILineClient, LineClient>();

        return services;
    }
}
