using ForestSchedule.Domain.Entities;

namespace ForestSchedule.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<bool> UserExistsAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }
}
