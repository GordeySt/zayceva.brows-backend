using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace Infrastructure.Settings;

public class JwtSettings : IValidatable
{
    [Required]
    public bool ValidateIssuer { get; set; }
    
    [Required]
    public string ValidIssuer { get; set; }
    
    [Required]
    public bool ValidateAudience { get; set; }
    
    [Required]
    public bool ValidateLifeTime { get; set; }
    
    [Required]
    public string ValidAudience { get; set; }
    
    [Required]
    public bool ValidateIssuerSigningKey { get; set; }
    
    [Required]
    public string IssuerSigningKey { get; set; }

    [Required]
    public int ExpirationMinutes { get; set; }
    
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}