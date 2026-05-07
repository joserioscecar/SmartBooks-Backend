using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SmartBooks.Application.Interfaces;
using SmartBooks.Infrastructure.Options;

namespace SmartBooks.Infrastructure.Service;

public class EmailService : IEmailService
{
    private readonly EmailOptions _emailOptions;

    public EmailService(IOptions<EmailOptions> emailOptions)
    {
        _emailOptions = emailOptions.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var email = BuildBaseEmail(to, subject);
        email.Body = new TextPart("html") { Text = htmlBody };

        await SendAsync(email);
    }

    public async Task SendEmailWithAttachmentAsync(
        string to,
        string subject,
        string html,
        byte[] attachmentBytes,
        string attachmentName)
    {
        var email = BuildBaseEmail(to, subject);

        var builder = new BodyBuilder { HtmlBody = html };
        builder.Attachments.Add(attachmentName, attachmentBytes);
        email.Body = builder.ToMessageBody();

        await SendAsync(email);
    }

    private MimeMessage BuildBaseEmail(string to, string subject)
    {
        var email = new MimeMessage();
        email.From.Add(new MailboxAddress(_emailOptions.FromName, _emailOptions.From));
        email.To.Add(MailboxAddress.Parse(to));
        email.Subject = subject;
        return email;
    }

    private async Task SendAsync(MimeMessage email)
    {
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_emailOptions.SmtpHost, _emailOptions.Port, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_emailOptions.User, _emailOptions.Pass);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }
}