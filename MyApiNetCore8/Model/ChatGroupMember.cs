namespace MyApiNetCore8.Model
{
    public class ChatGroupMember
    {
        public int ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }

}
