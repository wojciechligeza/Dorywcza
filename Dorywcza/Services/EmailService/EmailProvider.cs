using System.Collections.Generic;
using System.Linq;
using Dorywcza.Services.EmailService.Helpers;
using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Dorywcza.Services.EmailService
{
    public class EmailProvider : IEmailProvider
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailProvider(IOptions<EmailConfiguration> emailConfiguration)
        {
            _emailConfiguration = emailConfiguration.Value;
        }

        public void Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();

            message.To.AddRange(emailMessage.ToAddresses.Select(a => new MailboxAddress(a.Name, a.Address)));

            message.From.AddRange(emailMessage.FromAddresses.Select(a => new MailboxAddress(a.Name, a.Address)));

            message.Subject = emailMessage.Subject;

            // Body properties support multiple formats, most commonly plain text and HTML
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };
 
            // SmtpClient from MailKit.Net.Smtp
            using (var emailClient = new SmtpClient())
            {
                emailClient.Connect("smtp.office365.com", 587, true);

                // Remove any OAuth functionality as we won't be using it.
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate("wojciech.ligeza@o365.us.edu.pl", "pop5da3hip7new!");

                emailClient.Send(message);

                emailClient.Disconnect(true);
            }
        }

        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);
 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
 
                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);
 
                List<EmailMessage> emails = new List<EmailMessage>();

                for(int i=0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);

                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };

                    emailMessage.ToAddresses.AddRange(message.To.Select(a => (MailboxAddress)a).Select(a => new EmailAddress { Address = a.Address, Name = a.Name }));

                    emailMessage.FromAddresses.AddRange(message.From.Select(a => (MailboxAddress)a).Select(a => new EmailAddress { Address = a.Address, Name = a.Name }));

                    emails.Add(emailMessage);
                }
 
                return emails;
            }
        }
    }
}
