using Infrastructure.Settings;
using NetEscapades.Configuration.Validation;

namespace WebApi.Settings;

public class AppSettings : IValidatable
{
    public DbSettings DbSettings { get; set; }
    
    public void Validate()
    {
        DbSettings.Validate();
    }
}