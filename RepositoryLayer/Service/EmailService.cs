using System;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Method to configure the mail service and send the email
        public void SendEmail(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderPassword = _configuration["EmailSettings:SenderPassword"];

            using (var smtpClient = new SmtpClient(smtpServer, smtpPort))
            {
                smtpClient.Credentials = new NetworkCredential(senderEmail, senderPassword);
                smtpClient.EnableSsl = true;

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                mailMessage.ReplyToList.Add(new MailAddress(senderEmail));

                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (Exception ex)
                {
                    // Optional: Log the exception here if you have a logger service
                    throw new Exception("Failed to send email", ex);
                }
            }
        }
    }
}
