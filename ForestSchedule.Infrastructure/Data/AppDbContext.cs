using ForestSchedule.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForestSchedule.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<TeacherSubject> TeacherSubjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TeacherSubject>()
                .HasKey(ts => new { ts.TeacherId, ts.SubjectId });

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Teacher)
                .WithMany(t => t.TeacherSubjects)
                .HasForeignKey(ts => ts.TeacherId);

            modelBuilder.Entity<TeacherSubject>()
                .HasOne(ts => ts.Subject)
                .WithMany(s => s.TeacherSubjects)
                .HasForeignKey(ts => ts.SubjectId);

            modelBuilder.Entity<Lesson>().HasIndex(l => l.GroupId);
            modelBuilder.Entity<Lesson>().HasIndex(l => l.TeacherId);
            modelBuilder.Entity<Lesson>().HasIndex(l => l.SubjectId);
            modelBuilder.Entity<Lesson>().HasIndex(l => l.RoomId);
            modelBuilder.Entity<Lesson>().HasIndex(l => l.DayOfWeek);
        }
    }
}