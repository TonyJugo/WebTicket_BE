using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using WebTicket.Application.Abstracts;
using WebTicket.Application.Contracts;
using WebTicket.Infrastructure.Options;



namespace WebTicket.Infrastructure.Contracts
{
    public class GmailService : IMailService
    {
        private readonly GmailOptions _gmailOptions;
        public GmailService(IOptions<GmailOptions> gmailOptions) {
            _gmailOptions = gmailOptions.Value;
        }
        public async Task SendEmailAsync(SendEmailRequest sendEmailRequest)
        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_gmailOptions.Email),
                Subject = sendEmailRequest.Subject,
                Body = sendEmailRequest.Body,
                IsBodyHtml = sendEmailRequest.isHtml
            };
            mailMessage.To.Add(sendEmailRequest.Recipient);
            //dùng using để tự động giải phóng tài nguyên sau khi xài
            using var smtpClient = new SmtpClient();
            smtpClient.Host = _gmailOptions.Host;
            smtpClient.Port = _gmailOptions.Port;
            smtpClient.Credentials = new NetworkCredential(_gmailOptions.Email, _gmailOptions.Password); 
            smtpClient.EnableSsl = true; // Enable SSL for secure connection
            await smtpClient.SendMailAsync(mailMessage);

        }
    }
}
