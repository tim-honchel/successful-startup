using Microsoft.AspNetCore.Identity.UI.Services; // for IEmailSender interface
using Microsoft.Extensions.Configuration; // for accessing connection strings
using System.Net; // for creating NetworkCredential
using System.Net.Mail; // for composing email with MailMessage, MailAddress, and SmtpClient
using System.Runtime.CompilerServices; // for InternalsVisibleto

[assembly: InternalsVisibleTo("SuccessfulStartup.PresentationTests")] // allows tests to access internal members

namespace SuccessfulStartup.Presentation.Services
{
    public class EmailSender : IEmailSender // interface is for sending Identity-related emails
    {
        private static IConfigurationRoot _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build(); // gets the info from appsettings
        public static string _fromMail = _configuration["ConnectionStrings:GmailAddress"]; // gets Gmail account used for sending emails
        public static string _fromPassword = _configuration["ConnectionStrings:GmailPassword"]; // gets app password

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {

            var message = CreateMessage(email, subject, htmlMessage);

            var smtpClient = CreateSmtpClient();

            smtpClient.Send(message);
        }

        public MailMessage CreateMessage(string email, string subject, string htmlMessage) // created separate class for single responsiblity principle and unit testing
        {
            if (String.IsNullOrWhiteSpace(email) || String.IsNullOrWhiteSpace(subject) || String.IsNullOrWhiteSpace(htmlMessage)) { throw new ArgumentNullException(); }

            var message = new MailMessage();
            message.From = new MailAddress(_fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body>" + htmlMessage + "</body></html>";
            message.IsBodyHtml = true;
            return message;
        }

        public SmtpClient CreateSmtpClient() // created separate class for single responsiblity principle and unit testing
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(_fromMail, _fromPassword),
                EnableSsl = true,
            };
            return smtpClient;
        }
    }
}
