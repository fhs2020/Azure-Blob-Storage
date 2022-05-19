using AzureFHSFunc.Interfaces;
using AzureFHSFunc.Models;

using MailKit.Net.Smtp;

using Microsoft.Extensions.Options;

using MimeKit;

using System;
using System.IO;
using System.Threading.Tasks;

namespace AzureFHSFunc.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(_mailSettings.Mail));
            email.Subject = _mailSettings.Mail;
            var builder = new BodyBuilder();

            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();

                        }

                        builder.Attachments.Add(file.Name, fileBytes, ContentType.Parse(file.ContentType));
                    }
                }

            }

            builder.HtmlBody = mailRequest.Body;
            email.Body = builder.ToMessageBody();

            using (SmtpClient client = new SmtpClient())
            {
                try
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.CheckCertificateRevocation = false;
                    await client.ConnectAsync(_mailSettings.Host, _mailSettings.Port, false);
                    await client.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                    await client.SendAsync(email);
                }
                catch (Exception ex)
                {
                    string a = ex.Message;

                }
            }

        }
    }
}
