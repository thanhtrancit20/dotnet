using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore8.Data;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Model;
using MyApiNetCore8.Services;

namespace MyApiNetCore8.Services.impl
{
    public class TaskService : ITaskService
    {
        private readonly MyContext _context;
        private readonly IMapper _mapper;

        public TaskService(MyContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResponse<TaskResponse>> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var taskModel = new TaskModel
            {
                Name = createTaskDto.Name,
                TaskMembers = createTaskDto.MemberIds.Select(id => new TaskMember
                {
                    UserId = id
                }).ToList()
            };

            var chatGroup = new ChatGroup
            {
                ChatGroupMembers = createTaskDto.MemberIds.Select(id => new ChatGroupMember
                {
                    UserId = id
                }).ToList()
            };

            taskModel.ChatGroup = chatGroup;

            _context.TaskModels.Add(taskModel);
            await _context.SaveChangesAsync();
            var task = await _context.TaskModels
                .Include(t => t.TaskMembers)
                .ThenInclude(tm => tm.User)
                .Include(t => t.ChatGroup)
                .ThenInclude(cg => cg.ChatGroupMembers)
                .ThenInclude(cgm => cgm.User)
                .FirstOrDefaultAsync(t => t.Id == taskModel.Id);
            var taskDto = _mapper.Map<TaskResponse>(taskModel);
            return new ApiResponse<TaskResponse>(1000, "Task created successfully", taskDto);
        }

        public async Task<ApiResponse<IEnumerable<TaskResponse>>> GetAllTasksAsync()
        {
            var tasks = await _context.TaskModels
                .Include(t => t.TaskMembers)
                .ThenInclude(tm => tm.User)
                .Include(t => t.ChatGroup)
                .ThenInclude(cg => cg.ChatGroupMembers)
                .ThenInclude(cgm => cgm.User)
                .ToListAsync();

            var taskDtos = _mapper.Map<IEnumerable<TaskResponse>>(tasks);
            return new ApiResponse<IEnumerable<TaskResponse>>(1000, "Tasks retrieved successfully", taskDtos);
        }
    }
}