using Infrastructure.Settings;
using NetEscapades.Configuration.Validation;

namespace WebApi.Settings;

public class AppSettings : IValidatable
{
    public DbSettings DbSettings { get; set; }
    public IdentitySettings IdentitySettings { get; set; }
    public SmtpClientSettings SmtpClientSettings { get; set; }
    
    public void Validate()
    {
        DbSettings.Validate();
        IdentitySettings.Validate();
    }
}