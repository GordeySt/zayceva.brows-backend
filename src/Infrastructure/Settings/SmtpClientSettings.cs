using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace Infrastructure.Settings;

public class SmtpClientSettings: IValidatable
{
    [Required]
    public string Host { get; set; }
    
    [Required]
    public int Port { get; set; }
    
    [Required]
    public string EmailName { get; set; }
    
    [Required]
    public string EmailAddress { get; set; }
    
    public string Password { get; set; }
    
    public bool UseSsl { get; set; }
    
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}