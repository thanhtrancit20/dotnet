using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;

namespace MyApiNetCore8.Services
{
    public interface ITaskService
    {
        Task<ApiResponse<TaskResponse>> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<ApiResponse<IEnumerable<TaskResponse>>> GetAllTasksAsync();
    }
}
