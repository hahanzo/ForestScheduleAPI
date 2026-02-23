namespace ForestSchedule.Domain.Entities
{
    public class Room
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public ICollection<Lesson> Lessons { get; set; } = [];
    }
}
