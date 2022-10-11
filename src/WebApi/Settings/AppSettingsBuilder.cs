using Infrastructure.Settings;

namespace WebApi.Settings;

public static class AppSettingsBuilder
{
    public static AppSettings ReadAppSettings(IConfiguration configuration)
    {
        var dbSettings = configuration
            .GetSection(nameof(AppSettings.DbSettings))
            .Get<DbSettings>();

        var identitySettings = configuration
            .GetSection(nameof(AppSettings.IdentitySettings))
            .Get<IdentitySettings>();

        return new AppSettings
        {
            DbSettings = dbSettings,
            IdentitySettings = identitySettings
        };
    }
}