using ForestSchedule.Application.DTOs.AuthDtos;
using ForestSchedule.Application.Interfaces;
using ForestSchedule.Domain.Entities;
using ForestSchedule.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForestSchedule.Application.Services
{
    public class AuthService(IUserRepository userRepository, IConfiguration config) : IAuthService
    {
        public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
        {
            if (await userRepository.UserExistsAsync(dto.Email))
                throw new Exception("A user with this email already exists.");

            // Role definition by NLTU domain
            RoleType role;
            if (dto.Email.EndsWith("@nltu.edu.ua"))
            {
                role = RoleType.Teacher;
            }
            else if (dto.Email.EndsWith("@nltu.lviv.ua"))
            {
                role = RoleType.Student;
            }
            else
            {
                throw new Exception("Registration is allowed only for email domains @nltu.edu.ua and @nltu.lviv.ua");
            }

            // Password Hashing
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = role
            };

            await userRepository.AddUserAsync(user);
            await userRepository.SaveChangesAsync();

            var token = GenerateJwtToken(user);
            return new AuthResponseDto(token, user.Role.ToString(), user.Email);
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await userRepository.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null; // Incorrect login or password

            var token = GenerateJwtToken(user);
            return new AuthResponseDto(token, user.Role.ToString(), user.Email);
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()) // The role is added to the token
        };

            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
