using ForestSchedule.API.Filters;
using ForestSchedule.Application.DTOs;
using ForestSchedule.Application.DTOs.LessonDtos;
using ForestSchedule.Application.Interfaces;
using ForestSchedule.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForestSchedule.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LessonsController(ILessonService lessonService) : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(CacheResourceFilter))]
        public async Task<ActionResult<IEnumerable<LessonDto>>> GetAll()
        {
            return Ok(await lessonService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<LessonDto>> GetById(int id)
        {
            var dto = await lessonService.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpGet("search")]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<LessonDto>>> GetPaged([FromQuery] LessonQueryParameters parameters)
        {
            return Ok(await lessonService.GetPagedAsync(parameters));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateDaysFilter]
        public async Task<ActionResult<LessonDto>> Create([FromBody] CreateLessonDto dto)
        {
            var createdDto = await lessonService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdDto.Id }, createdDto);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateLessonDto dto)
        {
            await lessonService.UpdateAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await lessonService.DeleteAsync(id);
            return NoContent();
        }
    }
}
