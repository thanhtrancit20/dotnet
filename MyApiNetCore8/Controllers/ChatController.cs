using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApiNetCore8.Data;
using MyApiNetCore8.DTO.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyApiNetCore8.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ChatsController : ControllerBase
    {
        private readonly MyContext _myContext;

        public ChatsController(MyContext myContext)
        {
            _myContext = myContext;
        }

        // Endpoint to get all chat groups
        [HttpGet("chatgroups")]
        public async Task<IActionResult> GetChatGroups(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            var chatGroups = await _myContext.ChatGroups
                .Include(cg => cg.TaskModel)
                .Include(cg => cg.ChatGroupMembers)
                .Where(cg => cg.ChatGroupMembers.Any(cgm => cgm.UserId == userId))
                .ToListAsync();

            var chatGroupDtos = chatGroups.Select(cg => new
            {
                cg.Id,
                cg.TaskModelId,
                TaskModelName = cg.TaskModel.Name,
                ChatGroupMembers = cg.ChatGroupMembers.Select(cgm => new
                {
                    cgm.UserId,
                }).ToList()
            });

            return Ok(new ApiResponse<IEnumerable<object>>(1000, "Chat groups retrieved successfully", chatGroupDtos));
        }

        [HttpGet("{chatGroupId}/messages")]
        public async Task<IActionResult> GetMessages(int chatGroupId)
        {
            var messages = await _myContext.ChatMessages
                .Where(cm => cm.ChatGroupId == chatGroupId)
                .OrderBy(cm => cm.SentAt)
                .Select(cm => new
                {
                    cm.UserId,
                    cm.Message,
                    cm.SentAt,
                    Username = cm.User.UserName,
                    Avatar = cm.User.Avatar
                })
                .ToListAsync();

            return Ok(new ApiResponse<IEnumerable<object>>(1000, "Messages retrieved successfully", messages));
        }
    }
}
