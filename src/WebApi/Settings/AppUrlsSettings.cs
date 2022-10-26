using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace WebApi.Settings;

public class AppUrlsSettings : IValidatable
{
    [Required]
    public string ClientAppUrl { get; set; }

    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}