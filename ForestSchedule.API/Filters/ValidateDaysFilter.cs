using ForestSchedule.Application.DTOs.LessonDtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ForestSchedule.API.Filters
{
    public class ValidateDaysFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var dtoParam = context.ActionArguments.Values.FirstOrDefault(v => v is CreateLessonDto);

            if (dtoParam is CreateLessonDto dto)
            {
                if (dto.DayOfWeek == 7)
                {
                    context.Result = new BadRequestObjectResult(new { Message = "Creation of lessons on Sunday is prohibited!" });
                }
            }
        }
    }
}
