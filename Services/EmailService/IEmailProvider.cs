using System.Collections.Generic;

namespace Dorywcza.Services.EmailService
{
    public interface IEmailProvider
    {
        void Send(EmailMessage emailMessage);
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
