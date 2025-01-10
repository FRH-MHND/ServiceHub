using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceHub.Data;
using ServiceHub.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ServiceHub.Services.Interfaces;

namespace ServiceHub.Services.Implementation
{
	public class UserService : IUserService
	{
		private readonly ApplicationDbContext _context;
		private readonly ISmsService _smsService;

		public UserService(ApplicationDbContext context, ISmsService smsService)
		{
			_context = context;
			_smsService = smsService;
		}

		public async Task<bool> UserExists(string phoneNumber)
		{
			return await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber);
		}

		public string HashPassword(string password)
		{
			using var sha256 = SHA256.Create();
			var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
			return Convert.ToBase64String(bytes);
		}

		public string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        public async Task<User?> AuthenticateUser(string identifier, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == identifier);
            if (user == null || user.PasswordHash != HashPassword(password))
            {
                return null;
            }
            return user;
        }

		public async Task<bool> VerifyCodeAsync(string phoneNumber, string code)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
			if (user == null)
			{
				return false;
			}

			return user.VerificationCode == code;
		}

		public async Task CreateUser(User user)
		{
			user.VerificationCode = GenerateVerificationCode();
			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			var messageBody = $"Your verification code is {user.VerificationCode}";
			await _smsService.SendSmsAsync("ServiceHub", user.PhoneNumber, messageBody);
		}
		public async Task<User?> AuthenticateAdmin(string email, string password)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.IsAdmin);
            if (admin == null || admin.PasswordHash != HashPassword(password))
            {
                return null;
            }
            return admin;
        }

        public async Task<string> GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("YourNewSecretKeyYourNewSecretKey12"); // Make sure this is 32 bytes long
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
                }),
                Expires = DateTime.UtcNow.AddMinutes(45), // Session expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<bool> SendPasswordResetCode(string identifier)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.PhoneNumber == identifier);
            if (user == null)
            {
                return false;
            }

            var resetCode = GenerateVerificationCode();
            user.VerificationCode = resetCode;
            await _context.SaveChangesAsync();

           
            await _smsService.SendSmsAsync("ServiceHub", identifier, $"Your verification code is {resetCode}");
            

            return true;
        }

        public async Task<bool> ResetPassword(string identifier, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == identifier || u.PhoneNumber == identifier);
            if (user == null)
            {
                return false;
            }

            user.PasswordHash = HashPassword(newPassword);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
