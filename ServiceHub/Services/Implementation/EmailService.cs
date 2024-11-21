using System.Threading.Tasks;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Services.Implementation
{
    public class EmailService : IEmailService
    {
        public async Task SendVerificationCode(string email, string code)
        {
            // Implement email sending logic here using an external service
        }
    }
}
