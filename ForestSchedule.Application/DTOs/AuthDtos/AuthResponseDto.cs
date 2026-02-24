namespace ForestSchedule.Application.DTOs.AuthDtos
{
    public record AuthResponseDto
    (
        string Token, 
        string Role, 
        string Email
    );
}
