using Microsoft.Extensions.Configuration;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using System.Net;
using System.Net.Mail;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _gmailEmail;
        private readonly string _gmailAppPassword;

        public EmailService(IConfiguration configuration)
        {
            // Add these two keys to your appsettings.json
            _gmailEmail = configuration["Gmail:Email"];
            _gmailAppPassword = configuration["Gmail:AppPassword"];
        }

        public async Task<int> SendEmailAsync(string to, string subject, string htmlContent)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_gmailEmail, _gmailAppPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_gmailEmail, "Trackify App"),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
            return 1;
        }
    }
}
