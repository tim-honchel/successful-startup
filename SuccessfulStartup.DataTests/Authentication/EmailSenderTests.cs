using Shouldly; // for assertion
using SuccessfulStartup.Data.Authentication;
using System.Net;
using System.Net.Mail; // for MailMessage, MailAddress, MailAddressCollection

namespace SuccessfulStartup.DataTests.Authentication
{
    [TestFixture]
    public class EmailSenderTests
    {
        private EmailSender _sender;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _sender = new EmailSender();
        }

        [Test]
        public void GetsFromEmailFromAppSettings()
        {
            EmailSender._fromMail.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public void GetsEmailPasswordFromAppSettings()
        {
            EmailSender._fromPassword.ShouldNotBeNullOrWhiteSpace();
        }

        [Test]
        public void CreateMessage_ReturnsMailMessage_GivenValidInput()
        {
            var message = _sender.CreateMessage("email@gmail.com", "subject", "htmlMessage");
            message.ShouldBeOfType<MailMessage>();
        }

        [Test]
        public void CreateMessage_ReturnsValidMailMessage_GivenValidInput()
        {
            var message = _sender.CreateMessage("email@gmail.com", "subject", "htmlMessage");

            message.ShouldSatisfyAllConditions(
                () => message.From.ShouldBeEquivalentTo(new MailAddress(EmailSender._fromMail)),
                () => message.Subject.ShouldBe("subject"),
                () => message.To.ShouldBeEquivalentTo(new MailAddressCollection { new MailAddress("email@gmail.com") }), // To is a MailAddressCollection because it could include multiple addresses
                () => message.Body.ShouldBe("<html><body>htmlMessage</body></html>")
                ); 
        }

        [TestCase("", "subject", "htmlMessage")]
        [TestCase("email@gmail.com", null, "htmlMessage")]
        [TestCase("email@gmail.com", "subject", "    ")]
        public void CreateMessage_ThrowsArgumentNullException_GivenNullOrEmptyInput(string email, string subject, string htmlMessage)
        {
         Should.Throw<ArgumentNullException>( () => _sender.CreateMessage(email, subject, htmlMessage));       
        }

        [Test]
        public void CreateMessage_ThrowsInvalidFormatException_GivenInvalidEmail()
        {
            Should.Throw<FormatException>(() => _sender.CreateMessage("emailWithoutRequiredCharacters", "subject", "htmlMessage"));
        }

        [Test]
        public void CreateSmtpClient_ReturnsSmtpClient()
        {
            var client = _sender.CreateSmtpClient();
            client.ShouldBeOfType<SmtpClient>();
        }

        [Test]
        public void CreateSmtpClient_ReturnsValidSmtpClient()
        {
            var client = _sender.CreateSmtpClient();
            client.ShouldSatisfyAllConditions(
                () => client.Host.ShouldBe("smtp.gmail.com"),
                () => client.Port.ShouldBe(587),
                () => client.Credentials.ShouldBeEquivalentTo(new NetworkCredential(EmailSender._fromMail, EmailSender._fromPassword)),
                () => client.EnableSsl.ShouldBeTrue()
                );
        }
    }
}
