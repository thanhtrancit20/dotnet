using MyApiNetCore8.Enums;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.DTO.Response;

public class AccountResponse : BaseEntity
{
    public string id { get; set; }
    public string username { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public DateTime dateOfBirth { get; set; }
    public Gender gender { get; set; }
    public string phoneNumber { get; set; }
    public string address { get; set; }
    public string email { get; set; }
    public string avatar { get; set; }
    public IList<String> roles { get; set; }
}