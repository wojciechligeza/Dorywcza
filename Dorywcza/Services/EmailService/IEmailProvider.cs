using System.Collections.Generic;
using System.Threading.Tasks;
using Dorywcza.Services.EmailService.Helpers;

namespace Dorywcza.Services.EmailService
{
    public interface IEmailProvider
    {
        Task Send(EmailMessage emailMessage);
        Task<List<EmailMessage>> ReceiveEmail(int maxCount = 10);
    }
}
