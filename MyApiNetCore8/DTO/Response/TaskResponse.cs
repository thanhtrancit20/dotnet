using MyApiNetCore8.Enums;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.DTO.Response
{
    public class TaskResponse : BaseEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Priority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public Status Status { get; set; }
        public ICollection<string> Labels { get; set; }

        public AccountResponse Account { get; set; }
    }
}
