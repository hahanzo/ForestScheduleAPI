namespace ForestSchedule.Application.DTOs.LessonDtos
{
    public record LessonDto
    (
        int Id,
        string SubjectName,
        string TeacherName,
        string GroupName,
        string RoomName,
        int DayOfWeek,
        int LessonNumber,
        string TimeStart,
        string TimeEnd,
        string Type,
        string WeekType
    );
}
