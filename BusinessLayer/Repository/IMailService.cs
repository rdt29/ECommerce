using Microsoft.AspNetCore.Http;

namespace BusinessLayer.Repository
{
    public interface IMailService
    {
        Task<string> MailSend(string ToEmail, string htmlcontent, string plaincontent, string subject);

        Task<string> MailSendWithAttachment(byte[] file , string ToEmail, string htmlcontent, string plaincontent, string subject , string filename);
    }
}