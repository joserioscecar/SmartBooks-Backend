using System.Threading.Tasks;

namespace SmartBooks.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string html);
        Task SendEmailWithAttachmentAsync(string to, string subject, string html, byte[] attachment, string attachmentName);
    }
}
