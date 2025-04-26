using Task_Management_API.Models;

namespace Task_Management_API.Services.Interfaces
{
    public interface ITaskService
    {
        Task<TaskModel> CreateTaskAsync(TaskModel task);
        Task<TaskModel> GetTaskByIdAsync(int id);
        Task<List<TaskModel>> GetTasksByUserIdAsync(int userId);
        Task<TaskComment> AddCommentAsync(int taskId, TaskComment comment);
        Task<bool> DeleteTaskAsync(int taskId);
        Task<TaskModel> UpdateTaskAsync(int taskId, TaskModel updatedTask);
    }
}
