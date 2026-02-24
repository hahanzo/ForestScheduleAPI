using System.Text.Json.Serialization;

namespace ForestSchedule.Application.DTOs.LessonDtos
{
    public record JsonLessonDto(
        [property: JsonPropertyName("groupId")] string GroupId,
        [property: JsonPropertyName("dayOfWeek")] int DayOfWeek,
        [property: JsonPropertyName("weekType")] string WeekType,
        [property: JsonPropertyName("lessonNumber")] int LessonNumber,
        [property: JsonPropertyName("timeStart")] string TimeStart,
        [property: JsonPropertyName("timeEnd")] string TimeEnd,
        [property: JsonPropertyName("subject")] string Subject,
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("teacher")] string Teacher,
        [property: JsonPropertyName("room")] string Room
    );
}
