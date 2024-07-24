namespace MyApiNetCore8.Model
{
    public class TaskMember
    {
        public int TaskModelId { get; set; }
        public TaskModel TaskModel { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }
    }

}
