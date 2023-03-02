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
using MimeKit;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;

namespace BusinessLayer.RepositoryImplementation
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        private readonly ISendGridClient _sendGridClient;
        private EmailSettings _emailSettings = null;

        public MailService(IConfiguration configuration, ISendGridClient sendGridClient, IOptions<EmailSettings> options)
        {
            _emailSettings = options.Value;
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

        public async Task<string> MailSendWithAttachment(byte[] filebyte, string ToEmail, string htmlcontent, string plaincontent, string subject, string filename)
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

        public async Task<string> SMTPMail(byte[] file, string ToEmail, string htmlcontent, string plaincontent, string subject, string filename)
        {
            try
            {
                var EmailToName = "";
                var Filename = "Invoice";
                var type = "application/pdf";
                MimeMessage emailMessage = new MimeMessage();

                MailboxAddress emailFrom = new MailboxAddress(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);

                MailboxAddress emailTo = new MailboxAddress(EmailToName, ToEmail);
                emailMessage.To.Add(emailTo);

                emailMessage.Subject = subject;

                BodyBuilder emailBodyBuilder = new BodyBuilder();

                //if (emailData.EmailAttachments != null)
                //{
                //byte[] attachmentFileByteArray;
                //foreach (IFormFile attachmentFile in emailData.EmailAttachments)
                //{
                //    if (attachmentFile.Length > 0)
                //    {
                //        using (MemoryStream memoryStream = new MemoryStream())
                //        {
                //            attachmentFile.CopyTo(memoryStream);
                //            attachmentFileByteArray = memoryStream.ToArray();
                //        }
                //        emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
                //    }
                //}
                //}

                emailBodyBuilder.Attachments.Add(Filename, file, ContentType.Parse(type));
                emailBodyBuilder.HtmlBody = htmlcontent;
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new SmtpClient();
                await emailClient.ConnectAsync(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                await emailClient.AuthenticateAsync(_emailSettings.EmailId, _emailSettings.Password);
                await emailClient.SendAsync(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();

                return ("Sent");
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}