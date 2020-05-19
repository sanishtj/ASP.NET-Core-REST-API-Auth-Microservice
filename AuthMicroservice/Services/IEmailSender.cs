using SendGrid;
using System.Threading.Tasks;

namespace AuthMicroservice.Services
{
    public interface IEmailSender
    {
        AuthMessageSenderOptions Options { get; }
        Task<Response> SendEmailAsync(string email, string subject, string message);
        Task<Response> Execute(string apiKey, string subject, string message, string email);

    }
}
