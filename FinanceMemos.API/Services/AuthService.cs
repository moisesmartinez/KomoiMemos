using FinanceMemos.API.DTOs;
using FinanceMemos.API.Helpers;
using FinanceMemos.API.Models;
using FinanceMemos.API.Repositories.Interfaces;
using FinanceMemos.API.Services.Interfaces;

namespace FinanceMemos.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtTokenService _jwtTokenService;

        public AuthService(IUserRepository userRepository, JwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> RegisterAsync(RegisterDTO model)
        {
            var existingUser = await _userRepository.GetUserByUsernameAsync(model.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Username already exists.");
            }

            var user = new User
            {
                Username = model.Username,
                PasswordHash = PasswordHasher.HashPassword(model.Password)
            };

            await _userRepository.AddUserAsync(user);
            return "User registered successfully.";
        }

        public async Task<string> LoginAsync(LoginDTO model)
        {
            var user = await _userRepository.GetUserByUsernameAsync(model.Username);

            if (user == null || !PasswordHasher.VerifyPassword(model.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            var token = _jwtTokenService.GenerateJwtToken(user.Id.ToString(), user.Username);
            return token;
        }
    }
}
