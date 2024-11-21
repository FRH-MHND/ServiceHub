using System.Threading.Tasks;

namespace ServiceHub.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendVerificationCode(string email, string code)
        {
            // Implement email sending logic here using an external service
        }
    }
}
