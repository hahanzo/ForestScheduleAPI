using ForestSchedule.Application.DTOs.LessonDtos;
using ForestSchedule.Domain.Entities;

namespace ForestSchedule.Application.Interfaces
{
    public interface ILessonRepository
    {
        Task<IEnumerable<Lesson>> GetAllLessonsAsync();
        Task<Lesson?> GetLessonByIdAsync(int id);
        Task AddLessonAsync(Lesson lesson);
        Task UpdateLessonAsync(Lesson lesson);
        Task DeleteLessonAsync(int id);
        Task SaveChangesAsync();
        Task<(IEnumerable<Lesson> Lessons, int TotalCount)> GetLessonsPagedAsync(LessonQueryParameters parameters);
    }
}
