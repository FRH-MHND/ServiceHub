using System.Threading.Tasks;

namespace ServiceHub.Services
{
    public interface IEmailService
    {
        Task SendVerificationCode(string email, string code);
    }
}
