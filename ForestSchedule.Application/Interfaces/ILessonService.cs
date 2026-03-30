using ForestSchedule.Application.DTOs.LessonDtos;
using ForestSchedule.Application.DTOs;

namespace ForestSchedule.Application.Interfaces
{
    public interface ILessonService
    {
        Task<IEnumerable<LessonDto>> GetAllAsync();
        Task<LessonDto?> GetByIdAsync(int id);
        Task<PagedResult<LessonDto>> GetPagedAsync(LessonQueryParameters parameters);
        Task<LessonDto> CreateAsync(CreateLessonDto dto);
        Task UpdateAsync(int id, UpdateLessonDto dto);
        Task DeleteAsync(int id);
    }
}
