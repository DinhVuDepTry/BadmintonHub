using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BadmintonHub.Services;

public sealed class SmtpEmailSender(
    IConfiguration configuration,
    ILogger<SmtpEmailSender> logger) : IEmailSender
{
    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        var host = configuration["Email:SmtpHost"];
        var port = configuration.GetValue<int?>("Email:SmtpPort") ?? 587;
        var username = configuration["Email:Username"];
        var password = configuration["Email:Password"];
        var from = configuration["Email:From"] ?? username;

        if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(username) ||
            string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(from))
        {
            logger.LogWarning("Password recovery email was not sent because SMTP is not configured.");
            return;
        }

        using var client = new SmtpClient(host, port)
        {
            EnableSsl = true,
            Credentials = new NetworkCredential(username, password)
        };
        using var message = new MailMessage(from, email, subject, htmlMessage)
        {
            IsBodyHtml = true
        };
        await client.SendMailAsync(message);
    }
}
