namespace ForestSchedule.Application.DTOs.LessonDtos
{
    public record UpdateLessonDto
    (
        int GroupId,
        int SubjectId,
        int TeacherId,
        int RoomId,
        int DayOfWeek,
        int LessonNumber,
        string WeekType,
        string Type,
        string TimeStart,
        string TimeEnd
    );
}
