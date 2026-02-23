using ForestSchedule.Application.Interfaces;
using ForestSchedule.Domain.Entities;
using ForestSchedule.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ForestSchedule.Infrastructure.Repositories
{
    public class LessonRepository(AppDbContext context) : ILessonRepository
    {
        public async Task<IEnumerable<Lesson>> GetAllLessonsAsync()
        {
            return await context.Lessons
                .Include(l => l.Group)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.Room)
                .ToListAsync();
        }

        public async Task<Lesson?> GetLessonByIdAsync(int id)
        {
            return await context.Lessons
                .Include(l => l.Group)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.Room)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task AddLessonAsync(Lesson lesson)
        {
            await context.Lessons.AddAsync(lesson);
        }

        public async Task UpdateLessonAsync(Lesson lesson)
        {
            context.Lessons.Update(lesson);
            await Task.CompletedTask;
        }

        public async Task DeleteLessonAsync(int id)
        {
            var lesson = await context.Lessons.FindAsync(id);
            if (lesson != null)
            {
                context.Lessons.Remove(lesson);
            }
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
