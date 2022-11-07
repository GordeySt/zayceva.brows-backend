using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace Infrastructure.Settings;

public class IdentitySettings : IValidatable
{
    public IdentityPasswordSettings PasswordSettings { get; set; }
    
    public JwtSettings JwtSettings { get; set; }
    
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);

        PasswordSettings.Validate();
        JwtSettings.Validate();
    }
}