using System.Threading.Tasks;

namespace ServiceHub.Services
{
    public interface ILoggingService
    {
        Task LogAdminAction(string adminName, string action);
    }
}
