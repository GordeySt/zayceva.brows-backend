namespace Application.Common.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string emailToSend, string subject, string message);
}