namespace MyApiNetCore8.DTO.Response
{
    public class ChatGroupDto
    {
        public int Id { get; set; }
        public int TaskModelId { get; set; }
        public string TaskModelName { get; set; }
        public ICollection<ChatGroupMemberDto> ChatGroupMembers { get; set; }
    }
}
