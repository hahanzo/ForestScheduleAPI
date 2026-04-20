using ForestSchedule.Application.DTOs.LessonDtos;
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

        public async Task<(IEnumerable<Lesson> Lessons, int TotalCount)> GetLessonsPagedAsync(LessonQueryParameters p)
        {
            var query = context.Lessons
                .AsNoTracking()
                .AsSplitQuery()
                .Include(l => l.Group)
                .Include(l => l.Teacher)
                .Include(l => l.Subject)
                .Include(l => l.Room)
                .AsQueryable();

            // Filtration
            if (p.GroupId.HasValue)
                query = query.Where(l => l.GroupId == p.GroupId.Value);

            if (p.TeacherId.HasValue)
                query = query.Where(l => l.TeacherId == p.TeacherId.Value);

            //Searching
            if (!string.IsNullOrWhiteSpace(p.SearchTerm))
            {
                var search = $"{p.SearchTerm}%";

                query = query.Where(l =>
                    EF.Functions.Like(l.Subject.Name, search) ||
                    (l.Teacher != null && EF.Functions.Like(l.Teacher.FullName, search)) ||
                    (l.Room != null && EF.Functions.Like(l.Room.Name, search)));
            }

            // Sorting
            query = p.SortBy?.ToLower() switch
            {
                "subject" => p.SortDescending ? query.OrderByDescending(l => l.Subject.Name) : query.OrderBy(l => l.Subject.Name),
                "day" => p.SortDescending
                    ? query.OrderByDescending(l => l.DayOfWeek).ThenByDescending(l => l.LessonNumber)
                    : query.OrderBy(l => l.DayOfWeek).ThenBy(l => l.LessonNumber),
                _ => p.SortDescending ? query.OrderByDescending(l => l.Id) : query.OrderBy(l => l.Id)
            };

            var totalCount = await query.CountAsync();

            // Pagination
            var lessons = await query
                .Skip((p.PageNumber - 1) * p.PageSize)
                .Take(p.PageSize)
                .ToListAsync();

            return (lessons, totalCount);
        }
    }
}
