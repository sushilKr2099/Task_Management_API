using System.ComponentModel.DataAnnotations;
using Task_Management_API.Models;

namespace Task_Management_API.Models
{
    public enum UserRole
    {
        Admin,
        User
    }

    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public UserRole Role { get; set; }
        public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
        public ICollection<TaskComment> Comments { get; set; } = new List<TaskComment>();
    }
}

