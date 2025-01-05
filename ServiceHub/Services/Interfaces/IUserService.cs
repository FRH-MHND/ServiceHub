#nullable enable

using System.Threading.Tasks;
using ServiceHub.Models;

namespace ServiceHub.Services.Interfaces
{
	public interface IUserService
	{
		Task<bool> UserExists(string email, string phoneNumber);
		string HashPassword(string password);
		string GenerateVerificationCode();
		Task CreateUser(User user);
		Task<User?> AuthenticateUser(string identifier, string password);
		Task<User?> AuthenticateAdmin(string email, string password);
		Task<string> GenerateJwtToken(User user);
		Task<bool> SendPasswordResetCode(string identifier);
		Task<bool> ResetPassword(string identifier, string newPassword);
	}
}
