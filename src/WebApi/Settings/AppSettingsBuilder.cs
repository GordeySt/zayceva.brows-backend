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

        var smtpClientSettings = configuration
            .GetSection(nameof(AppSettings.SmtpClientSettings))
            .Get<SmtpClientSettings>();

        var appUrlsSettings = configuration
            .GetSection(nameof(AppSettings.AppUrlsSettings))
            .Get<AppUrlsSettings>();

        smtpClientSettings.Password = configuration["SmtpClientSettings:Password"];

        return new AppSettings
        {
            DbSettings = dbSettings,
            IdentitySettings = identitySettings,
            SmtpClientSettings = smtpClientSettings,
            AppUrlsSettings = appUrlsSettings
        };
    }
}