using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task Send(EmailMessage emailMessage)
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
                await emailClient.ConnectAsync("smtp.sendgrid.net", 587, false);

                // Remove any OAuth functionality as we won't be using it.
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                await emailClient.AuthenticateAsync(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                await emailClient.SendAsync(message);

                await emailClient.DisconnectAsync(true);
            }
        }

        public async Task<List<EmailMessage>> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                await emailClient.ConnectAsync(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);
 
                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
 
                await emailClient.AuthenticateAsync(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);
 
                List<EmailMessage> emails = new List<EmailMessage>();

                for(int i=0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = await emailClient.GetMessageAsync(i);

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
