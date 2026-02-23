namespace ForestSchedule.Domain.Entities
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public ICollection<Lesson> Lessons { get; set; } = [];
        public ICollection<TeacherSubject> TeacherSubjects { get; set; } = [];
    }
}
