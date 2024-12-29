//using App.Core.Interface;
//using Microsoft.Extensions.Configuration;
//using SendGrid;
//using SendGrid.Helpers.Mail;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Text;
//using System.Threading.Tasks;

//namespace Infrastructure.Services
//{
//    public class EmailService : IEmailService
//    {
//        private readonly IConfiguration _configuration;

//        public EmailService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }
//        public async Task<bool> SendEmailAsync(string toE, string name, string subject, string message)
//        {
//            var apiKey = _configuration["SENDGRID_API_KEY:Key"];
//            //Console.WriteLine(apiKey);

//            var client = new SendGridClient(apiKey); // for sendgrid used package

//            var from = new EmailAddress("chetanmundlesd@gmail.com", "Mayur Ramdham");
//            var to = new EmailAddress(toE, name);
//            var plaintext = message;
//            var htmlcontent = "";
//            var msg = MailHelper.CreateSingleEmail(from, to, subject, plaintext, htmlcontent);
//            var response = await client.SendEmailAsync(msg);








//            return true;
//        }
//    }
//}


using App.Core.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendEmailAsync(string toE, string name, string subject, string message)
        {
            // Fetch SMTP configuration from appsettings.json
            var smtpHost = _configuration["SMTP:Host"]; // e.g., smtp.gmail.com
            var smtpPort = int.Parse(_configuration["SMTP:Port"]); // e.g., 587
            var smtpUser = _configuration["SMTP:Username"]; // Your SMTP username
            var smtpPass = _configuration["SMTP:Password"]; // Your SMTP password
            var fromEmail = _configuration["SMTP:FromEmail"]; // Your sender email
            var fromName = _configuration["SMTP:FromName"]; // Your sender name

            try
            {
                // Configure the email message
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail, fromName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true // Change to true if the message contains HTML content
                };

                mailMessage.To.Add(new MailAddress(toE, name));

                // Configure the SMTP client
                using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
                {
                    smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                    smtpClient.EnableSsl = true; // Enable SSL/TLS

                    // Send the email asynchronously
                    await smtpClient.SendMailAsync(mailMessage);
                }

                return true; // Email sent successfully
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false; // Email sending failed
            }
        }
    }
}
