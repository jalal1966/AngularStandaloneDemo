using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace AngularStandaloneDemo.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // For development purposes, you might want to just log the email
                // In production, replace with actual SMTP implementation
                Console.WriteLine($"Email to: {to}, Subject: {subject}, Body: {body}");

                // Uncomment to use real email sending
                /*
                var smtpSettings = _configuration.GetSection("SmtpSettings");
                var client = new SmtpClient(smtpSettings["Host"])
                {
                    Port = int.Parse(smtpSettings["Port"]),
                    EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
                    Credentials = new NetworkCredential(
                        smtpSettings["Username"], 
                        smtpSettings["Password"])
                };
                
                var message = new MailMessage
                {
                    From = new MailAddress(smtpSettings["FromEmail"], smtpSettings["FromName"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);
                
                await client.SendMailAsync(message);
                */

                return true;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }
    }
}