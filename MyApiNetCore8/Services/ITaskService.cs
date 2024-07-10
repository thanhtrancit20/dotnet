using Microsoft.AspNetCore.Mvc;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.Services
{
    public interface ITaskService
    {
        Task<ActionResult<ApiResponse<TaskResponse>>> CreateTask(TaskCreateDTO taskDto);
    }
}
