﻿namespace MyApiNetCore8.DTO.Response
{
    public class TaskResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<AccountResponse> Members { get; set; }
        public ChatGroupResponse ChatGroup { get; set; }
    }
}
