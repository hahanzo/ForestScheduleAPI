using ForestSchedule.Application.Interfaces;
using ForestSchedule.Domain.Entities;
using ForestSchedule.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ForestSchedule.Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> UserExistsAsync(string email)
        {
            return await context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task AddUserAsync(User user)
        {
            await context.Users.AddAsync(user);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
