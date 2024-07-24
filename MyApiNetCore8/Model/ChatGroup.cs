using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyApiNetCore8.Model
{
    public class ChatGroup
    {
        public int Id { get; set; }
        public int TaskModelId { get; set; }
        public TaskModel TaskModel { get; set; }
        public ICollection<ChatGroupMember> ChatGroupMembers { get; set; }
    }
}
