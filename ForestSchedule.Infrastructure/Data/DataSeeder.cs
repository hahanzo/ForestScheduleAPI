using ForestSchedule.Application.DTOs.LessonDtos;
using ForestSchedule.Domain.Entities;
using ForestSchedule.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;


namespace ForestSchedule.Infrastructure.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            if (!await context.Users.AnyAsync(u => u.Role == RoleType.Admin))
            {
                context.Users.Add(new User
                {
                    Email = "superadmin@nltu.edu.ua",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("AdminPass123!"),
                    Role = RoleType.Admin
                });
                await context.SaveChangesAsync();
            }

            if (await context.Lessons.AnyAsync())
            {
                Console.WriteLine("Database is full. Skip seeding");
                return;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "schedule.json");
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File {filePath} is not found!");
                return;
            }

            var jsonString = await File.ReadAllTextAsync(filePath);
            var jsonData = JsonSerializer.Deserialize<List<JsonLessonDto>>(jsonString);

            if (jsonData == null || jsonData.Count == 0) return;

            Console.WriteLine($"Find {jsonData.Count} lessons in JSON. Start loadding...");

            foreach (var item in jsonData)
            {
                //  Grop
                var group = await context.Groups.FirstOrDefaultAsync(g => g.Name == item.GroupId);
                if (group == null)
                {
                    group = new Group { Name = item.GroupId };
                    context.Groups.Add(group);
                    await context.SaveChangesAsync();
                }

                // Teacher
                var teacher = await context.Teachers.FirstOrDefaultAsync(t => t.FullName == item.Teacher);
                if (teacher == null)
                {
                    teacher = new Teacher { FullName = item.Teacher };
                    context.Teachers.Add(teacher);
                    await context.SaveChangesAsync();
                }

                // Subject
                var subject = await context.Subjects.FirstOrDefaultAsync(s => s.Name == item.Subject);
                if (subject == null)
                {
                    subject = new Subject { Name = item.Subject };
                    context.Subjects.Add(subject);
                    await context.SaveChangesAsync();
                }

                // Room
                var room = await context.Rooms.FirstOrDefaultAsync(r => r.Name == item.Room);
                if (room == null)
                {
                    room = new Room { Name = item.Room };
                    context.Rooms.Add(room);
                    await context.SaveChangesAsync();
                }

                // Connection Teacher-Subject
                var teacherSubject = await context.TeacherSubjects
                    .FirstOrDefaultAsync(ts => ts.TeacherId == teacher.Id && ts.SubjectId == subject.Id);

                if (teacherSubject == null)
                {
                    context.TeacherSubjects.Add(new TeacherSubject
                    {
                        TeacherId = teacher.Id,
                        SubjectId = subject.Id
                    });
                    await context.SaveChangesAsync();
                }

                // Lesson
                var lesson = new Lesson
                {
                    GroupId = group.Id,
                    TeacherId = teacher.Id,
                    SubjectId = subject.Id,
                    RoomId = room.Id,
                    DayOfWeek = item.DayOfWeek,
                    LessonNumber = item.LessonNumber,
                    WeekType = item.WeekType,
                    TimeStart = item.TimeStart,
                    TimeEnd = item.TimeEnd,
                    Type = item.Type
                };

                context.Lessons.Add(lesson);
            }

            await context.SaveChangesAsync();
            Console.WriteLine("Database successfully seeded!");
        }
    }
}
