using Task_Management_API.Models;
using System.Linq;

namespace TaskManagementAPI.Data
{
    public class SeedData : ISeedData
    {
        private readonly MyDbContext _context;

        public SeedData(MyDbContext context)
        {
            _context = context;
        }

        public void Initialize()
        {
            if (_context.Users.Any()) return;

            var users = new List<User>
            {
                new User { Name = "User", Role = UserRole.User }
            };

            _context.Users.AddRange(users);
            _context.SaveChanges();

            var tasks = new List<TaskModel>
            {
                new TaskModel { Title = "Build API", Description = "Create base API endpoints", AssignedToUserId = users[0].Id },
                new TaskModel { Title = "Test API", Description = "Write unit tests", AssignedToUserId = users[1].Id }
            };

            _context.Tasks.AddRange(tasks);
            _context.SaveChanges();

            var comments = new List<TaskComment>
            {
                new TaskComment { TaskId = tasks[0].Id, UserId = users[1].Id, Content = "Looks good." },
                new TaskComment { TaskId = tasks[1].Id, UserId = users[0].Id, Content = "Add more tests." }
            };

            _context.TaskComments.AddRange(comments);
            _context.SaveChanges();
        }
    }
}
