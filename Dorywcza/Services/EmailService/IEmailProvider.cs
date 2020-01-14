using System.Collections.Generic;
using Dorywcza.Services.EmailService.Helpers;

namespace Dorywcza.Services.EmailService
{
    public interface IEmailProvider
    {
        void Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
