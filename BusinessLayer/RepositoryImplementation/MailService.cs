using BusinessLayer.Repository;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail.Model;
using Microsoft.AspNetCore.Http;
using Azure;

namespace BusinessLayer.RepositoryImplementation
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;

        public MailService(IConfiguration configuration, ISendGridClient sendGridClient)
        {
            _configuration = configuration;
            _sendGridClient = sendGridClient;
        }

        public async Task<string> MailSend(string ToEmail, string htmlcontent, string plaincontent, string subject)
        {
            string fromEmail = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                HtmlContent = htmlcontent,
                PlainTextContent = plaincontent,
            };
            msg.AddTo(ToEmail);

            var response = await _sendGridClient.SendEmailAsync(msg);
            string message = response.IsSuccessStatusCode ? "Email Send Successfully" :
            "Email Sending Failed";
            return message;
        }

        public async Task<string> MailSendWithAttachment(byte[] filebyte, string ToEmail, string htmlcontent, string plaincontent, string subject , string filename)
        {
            //; generate byte to file for mail
            var byteToFile = new MemoryStream(filebyte);
            
            var file = new FormFile(byteToFile, 0, filebyte.Length, null, filename);



            string fromEmail = _configuration.GetSection("SendGridEmailSettings")
       .GetValue<string>("FromEmail");

            string fromName = _configuration.GetSection("SendGridEmailSettings")
            .GetValue<string>("FromName");

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                HtmlContent = htmlcontent,
                PlainTextContent = plaincontent,
            };

            var ContentType = "application/pdf";
            await msg.AddAttachmentAsync(
              file.FileName,
              file.OpenReadStream(),
              ContentType,
              "attachment"
          );
            msg.AddTo(ToEmail);

            var Mailresponse = await _sendGridClient.SendEmailAsync(msg);
            string message = Mailresponse.IsSuccessStatusCode ? "Invoice Send Successfully" :
            "Invoice  Sending Failed";
            return message;
        }
    }
}