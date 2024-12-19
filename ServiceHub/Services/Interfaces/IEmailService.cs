using System.Threading.Tasks;

namespace ServiceHub.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string v, string subject, string body);
        Task SendVerificationCode(string email, string code);
    }
}
