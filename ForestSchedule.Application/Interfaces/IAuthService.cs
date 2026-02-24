using ForestSchedule.Application.DTOs.AuthDtos;

namespace ForestSchedule.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
