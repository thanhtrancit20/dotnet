namespace MyApiNetCore8.Model
{
    public class ChatMessage
    {
        public int Id { get; set; }
        public int ChatGroupId { get; set; }
        public string UserId { get; set; }
        public string Message { get; set; }
        public DateTime SentAt { get; set; }

        public ChatGroup ChatGroup { get; set; }
        public User User { get; set; }
    }

}
