using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MyApiNetCore8.Model
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public string UserId { get; set; }
        public User User { get; set; }

        public long ChatGroupId { get; set; }
        public ChatGroup ChatGroup { get; set; }
    }
}
