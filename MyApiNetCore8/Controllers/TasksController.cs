using Microsoft.AspNetCore.Mvc;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Services;

namespace MyApiNetCore8.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto createTaskDto)
        {
            if (createTaskDto == null || createTaskDto.MemberIds == null || !createTaskDto.MemberIds.Any())
            {
                return BadRequest(new ApiResponse<string>(4000, "Invalid task data", null));
            }

            var response = await _taskService.CreateTaskAsync(createTaskDto);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var response = await _taskService.GetAllTasksAsync();
            return Ok(response);
        }
    }
}
