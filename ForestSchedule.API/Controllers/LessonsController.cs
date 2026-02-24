using ForestSchedule.Application.DTOs.LessonDtos;
using ForestSchedule.Application.Interfaces;
using ForestSchedule.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ForestSchedule.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonsController(ILessonRepository repository) : ControllerBase
    {
        // GET: api/lessons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll()
        {
            var lessons = await repository.GetAllLessonsAsync();

            // Mappint Entites and Dto; (HINT) need to refactor, add AutoMapper or similar tools
            var dtos = lessons.Select(l => new LessonDto(
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
            ));

            return Ok(dtos);
        }

        // GET: api/lessons/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetById(int id)
        {
            var l = await repository.GetLessonByIdAsync(id);
            if (l == null) return NotFound();

            var dto = new LessonDto(
                l.Id, l.Subject.Name, l.Teacher?.FullName ?? "Not assigned",
                l.Group.Name, l.Room?.Name ?? "Not assigned", l.DayOfWeek,
                l.LessonNumber, l.TimeStart, l.TimeEnd, l.Type, l.WeekType
            );

            return Ok(dto);
        }

        // POST: api/lessons
        [HttpPost]
        public async Task<ActionResult<LessonDto>> Create([FromBody] CreateLessonDto dto)
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

            return CreatedAtAction(nameof(GetById), new { id = lesson.Id }, null);
        }

        // PUT: api/lessons/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonDto dto)
        {
            var lesson = await repository.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound();

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

            return NoContent();
        }

        // DELETE: api/lessons/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lesson = await repository.GetLessonByIdAsync(id);
            if (lesson == null) return NotFound();

            await repository.DeleteLessonAsync(id);
            await repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
