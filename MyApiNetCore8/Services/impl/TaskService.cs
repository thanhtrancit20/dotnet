using Microsoft.AspNetCore.Mvc;
using MyApiNetCore8.Data;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Enums;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.Services.impl
{
    public class TaskService : ITaskService
    {
        private readonly MyContext _context;

        public TaskService(MyContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<ApiResponse<TaskResponse>>> CreateTask(TaskCreateDTO taskDto)
        {
            var task = new TaskModel
            {
                Name = taskDto.TaskName,
                DueDate = DateTime.UtcNow, // You can set the due date based on your requirements
                Priority = Priority.LOW, // Default priority
                Status = Status.TODO, // Default status
                Users = new List<User>() // Initialize the list of users
            };

            // Find users by their IDs and add them to the task's Users collection
            foreach (var userId in taskDto.UserIds)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    task.Users.Add(user);
                }
            }

            // Add task to the context and save changes
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return task;
        }
    }
}
