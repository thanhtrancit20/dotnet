using Microsoft.AspNetCore.Mvc;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Services;
using System.Threading.Tasks;

namespace MyApiNetCore8.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskResponse>>> CreateTask(TaskCreateDTO taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdTask = await _taskService.CreateTaskAsync(taskDto);
            return Ok(new ApiResponse<object>(1000, "Success", createdTask));
        }

        // Optionally, you can define a method to retrieve a task by its ID
        [HttpGet("{id}")]
        public IActionResult GetTask(Guid id)
        {
            // Implement logic to retrieve task details by ID
            // This is an example and can be implemented based on your requirements
            return Ok();
        }
    }
}
