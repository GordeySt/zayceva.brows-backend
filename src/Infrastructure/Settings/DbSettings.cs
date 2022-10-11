using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace Infrastructure.Settings;

public class DbSettings : IValidatable
{
    [Required]
    public string Host { get; set; }

    [Required]
    public int Port { get; set; }

    [Required]
    public string DatabaseName { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string Password { get; set; }

    public string ConnectionString => $"Server={Host};Port={Port};Database={DatabaseName};User Id={UserId};Password={Password};";

    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}