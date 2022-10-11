using NetEscapades.Configuration.Validation;

namespace Microsoft.Extensions.DependencyInjection.Startup.Settings;

public class AppSettings : IValidatable
{
    public DbSettings DbSettings { get; set; }
    
    public void Validate()
    {
        DbSettings.Validate();
    }
}