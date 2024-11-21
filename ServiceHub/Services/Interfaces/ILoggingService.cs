using System.Threading.Tasks;

namespace ServiceHub.Services.Interfaces
{
    public interface ILoggingService
    {
        Task LogAdminAction(string adminName, string action);
    }
}
