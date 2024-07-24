namespace MyApiNetCore8.DTO.Request
{
    public class CreateTaskDto
    {
        public string Name { get; set; }
        public List<string> MemberIds { get; set; }
    }

}
