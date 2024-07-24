using MyApiNetCore8.Enums;
using MyApiNetCore8.Model;

namespace MyApiNetCore8.DTO.Response;

public class AccountResponse
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Avatar { get; set; }
    public IList<string> Roles { get; set; }
}