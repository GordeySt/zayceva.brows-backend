using Application.Common.Interfaces;
using Infrastructure.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;

namespace Infrastructure.Services.Email;

public class EmailService : IEmailService
{
    private readonly SmtpClientSettings _smtpClientSettings;

    public EmailService(SmtpClientSettings smtpClientSettings)
    {
        _smtpClientSettings = smtpClientSettings;
    }

    public async Task SendEmailAsync(string emailToSend, string subject, string message)
    {
        var email = new MimeMessage();
        
        email.From.Add(new MailboxAddress(
            _smtpClientSettings.EmailName,
            _smtpClientSettings.EmailAddress));
        email.To.Add(new MailboxAddress(string.Empty, emailToSend));
        email.Subject = subject;
        email.Body = new TextPart(TextFormat.Html)
        {
            Text = message
        };

        using var smtpClient = new SmtpClient();

        await smtpClient.ConnectAsync(
            _smtpClientSettings.Host,
            _smtpClientSettings.Port,
            _smtpClientSettings.UseSsl);

        await smtpClient.AuthenticateAsync(
            _smtpClientSettings.EmailAddress,
            _smtpClientSettings.Password);

        await smtpClient.SendAsync(email);

        await smtpClient.DisconnectAsync(true);
    }
}