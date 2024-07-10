using Microsoft.AspNetCore.Identity;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.DTO.Response;

namespace MyApiNetCore8.Repositories
{
    public interface IAccountService
    {
        Task<IdentityResult> SignUpAsync(SignUpModel model);
        Task<(string, string)> SignInAsync(SignInModel model);
        Task<(string, string)> RefreshTokenAsync(string refreshToken);
        Task<AccountResponse> GetMyInfoAsync();
        
        Task<List<AccountResponse>> GetAllCustomersAsync();
        
        Task<List<AccountResponse>> GetAllSystemUsersAsync();

    }
}
