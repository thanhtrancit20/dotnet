namespace MyApiNetCore8.DTO.Request
{
    public class TaskCreateDTO
    {
        public string TaskName { get; set; }
        public List<string> UserIds { get; set; }
    }
}
