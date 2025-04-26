using Microsoft.EntityFrameworkCore;
using Task_Management_API.Models;
using Task_Management_API.Services.Interfaces;
using TaskManagementAPI.Data;

namespace Task_Management_API.Services
{
    public class TaskService : ITaskService
    {
        private readonly MyDbContext _context;

        public TaskService(MyDbContext context)
        {
            _context = context;
        }

        // Create a new task
        public async Task<TaskModel> CreateTaskAsync(TaskModel task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));

            // Validate AssignedToUserId
            var assignedUserExists = await _context.Users.AnyAsync(u => u.Id == task.AssignedToUserId);
            if (!assignedUserExists)
                throw new InvalidOperationException($"User with ID {task.AssignedToUserId} does not exist.");

            // Validate UserId in TaskComments
            if (task.Comments != null)
            {
                foreach (var comment in task.Comments)
                {
                    var commentUserExists = await _context.Users.AnyAsync(u => u.Id == comment.UserId);
                    if (!commentUserExists)
                        throw new InvalidOperationException($"User with ID {comment.UserId} in a comment does not exist.");
                }
            }

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        // Get task by ID
        public async Task<TaskModel> GetTaskByIdAsync(int id)
        {
            var task = await _context.Tasks
                                      .Include(t => t.AssignedToUser)
                                      .Include(t => t.Comments)
                                      .FirstOrDefaultAsync(t => t.Id == id);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {id} not found");

            return task;
        }

        // Get tasks by user ID
        public async Task<List<TaskModel>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _context.Tasks
                                      .Where(t => t.AssignedToUserId == userId)
                                      .ToListAsync();

            if (!tasks.Any())
                throw new KeyNotFoundException($"No tasks found for user with ID {userId}");

            return tasks;
        }

        // Add a comment to a task
        public async Task<TaskComment> AddCommentAsync(int taskId, TaskComment comment)
        {
            if (comment == null)
                throw new ArgumentNullException(nameof(comment));

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            comment.TaskId = taskId;
            _context.TaskComments.Add(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        // Delete a task
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        // Update a task
        public async Task<TaskModel> UpdateTaskAsync(int taskId, TaskModel updatedTask)
        {
            if (updatedTask == null)
                throw new ArgumentNullException(nameof(updatedTask));

            var task = await _context.Tasks.FindAsync(taskId);
            if (task == null)
                throw new KeyNotFoundException($"Task with ID {taskId} not found");

            // Update task properties
            task.Title = updatedTask.Title;
            task.Description = updatedTask.Description;
            task.AssignedToUserId = updatedTask.AssignedToUserId;

            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}