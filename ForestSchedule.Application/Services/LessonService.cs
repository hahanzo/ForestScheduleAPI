
using ForestSchedule.Application.DTOs.LessonDtos;
using ForestSchedule.Application.DTOs;
using ForestSchedule.Application.Interfaces;
using ForestSchedule.Domain.Entities;
using ForestSchedule.Application.Exceptions;

namespace ForestSchedule.Application.Services
{
    public class LessonService(ILessonRepository repository) : ILessonService
    {
        public async Task<IEnumerable<LessonDto>> GetAllAsync()
        {
            var lessons = await repository.GetAllLessonsAsync();
            return lessons.Select(MapToDto);
        }

        public async Task<LessonDto?> GetByIdAsync(int id)
        {
            var lesson = await repository.GetLessonByIdAsync(id);
            return lesson == null ? null : MapToDto(lesson);
        }

        public async Task<PagedResult<LessonDto>> GetPagedAsync(LessonQueryParameters p)
        {
            var (lessons, totalCount) = await repository.GetLessonsPagedAsync(p);
            var dtos = lessons.Select(MapToDto).ToList();
            return new PagedResult<LessonDto>(dtos, totalCount, p.PageNumber, p.PageSize);
        }

        public async Task<LessonDto> CreateAsync(CreateLessonDto dto)
        {
            var lesson = new Lesson
            {
                GroupId = dto.GroupId,
                SubjectId = dto.SubjectId,
                TeacherId = dto.TeacherId,
                RoomId = dto.RoomId,
                DayOfWeek = dto.DayOfWeek,
                LessonNumber = dto.LessonNumber,
                WeekType = dto.WeekType,
                Type = dto.Type,
                TimeStart = dto.TimeStart,
                TimeEnd = dto.TimeEnd
            };

            await repository.AddLessonAsync(lesson);
            await repository.SaveChangesAsync();

            var createdLesson = await repository.GetLessonByIdAsync(lesson.Id);
            return MapToDto(createdLesson!);
        }

        public async Task UpdateAsync(int id, UpdateLessonDto dto)
        {
            var lesson = await repository.GetLessonByIdAsync(id)
                ?? throw new NotFoundException($"Lesson with ID {id} not found.");

            lesson.GroupId = dto.GroupId;
            lesson.SubjectId = dto.SubjectId;
            lesson.TeacherId = dto.TeacherId;
            lesson.RoomId = dto.RoomId;
            lesson.DayOfWeek = dto.DayOfWeek;
            lesson.LessonNumber = dto.LessonNumber;
            lesson.WeekType = dto.WeekType;
            lesson.Type = dto.Type;
            lesson.TimeStart = dto.TimeStart;
            lesson.TimeEnd = dto.TimeEnd;

            await repository.UpdateLessonAsync(lesson);
            await repository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var lesson = await repository.GetLessonByIdAsync(id)
                ?? throw new NotFoundException($"Lesson with ID {id} not found.");

            await repository.DeleteLessonAsync(id);
            await repository.SaveChangesAsync();
        }

        private static LessonDto MapToDto(Lesson l) => new(
            l.Id,
            l.Subject.Name,
            l.Teacher?.FullName ?? "Not assigned",
            l.Group.Name,
            l.Room?.Name ?? "Not assigned",
            l.DayOfWeek,
            l.LessonNumber,
            l.TimeStart,
            l.TimeEnd,
            l.Type,
            l.WeekType
        );
    }
}
