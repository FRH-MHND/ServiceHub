using System.Threading.Tasks;

namespace ServiceHub.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendVerificationCode(string email, string code);
    }
}
