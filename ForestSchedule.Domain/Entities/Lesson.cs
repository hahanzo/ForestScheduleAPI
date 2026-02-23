namespace ForestSchedule.Domain.Entities
{
    public class Lesson
    {
        public int Id { get; set; }

        // Time cordination
        public int DayOfWeek { get; set; } // 1-7
        public int LessonNumber { get; set; } // 1-6
        public string WeekType { get; set; } = string.Empty; // "numerator" / "denominator"
        public string Type { get; set; } = string.Empty; // "lec", "lab", "prac"

        public string TimeStart { get; set; } = string.Empty;
        public string TimeEnd { get; set; } = string.Empty;

        // Foreign keys
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;

        public int SubjectId { get; set; }
        public Subject Subject { get; set; } = null!;

        public int? TeacherId { get; set; }
        public Teacher? Teacher { get; set; }

        public int? RoomId { get; set; }
        public Room? Room { get; set; }
    }
}
