namespace ForestSchedule.Domain.Entities
{
    public class Teacher
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        
        public int? UserID { get; set; }
        public User? User { get; set; }
        
        public ICollection<Lesson> Lessons { get; set; } = [];
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = [];
    }
}
