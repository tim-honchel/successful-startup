using Microsoft.AspNetCore.Identity.UI.Services; // for IEmailSender interface
using Microsoft.Extensions.Configuration; // for accessing connection strings
using System.Net; // for creating NetworkCredential
using System.Net.Mail; // for composing email with MailMessage, MailAddress, and SmtpClient

namespace SuccessfulStartup.Data.Authentication
{
    public class EmailSender : IEmailSender // interface is for sending Identity-related emails
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); // gets the info from appsettings

            string fromMail = configuration["ConnectionStrings:GmailAddress"]; // gets Gmail account used for sending emails
            string fromPassword = configuration["ConnectionStrings:GmailPassword"]; // gets app password

            var message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body>" + htmlMessage + "</body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,

            };

            smtpClient.Send(message);
        }
    }
}
