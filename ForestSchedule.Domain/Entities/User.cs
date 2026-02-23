using ForestSchedule.Domain.Enums;

namespace ForestSchedule.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public RoleType Role { get; set; }
    }
}
