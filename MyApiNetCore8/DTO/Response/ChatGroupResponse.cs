namespace MyApiNetCore8.DTO.Response
{
    public class ChatGroupResponse
    {
        public int Id { get; set; }
        public List<AccountResponse> Members { get; set; }
    }
}
