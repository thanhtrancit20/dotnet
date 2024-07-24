using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyApiNetCore8.Data;
using MyApiNetCore8.DTO.Request;
using MyApiNetCore8.Helper;
using MyApiNetCore8.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using MyApiNetCore8.DTO.Response;
using MyApiNetCore8.Enums;
using Microsoft.EntityFrameworkCore;

namespace MyApiNetCore8.Repositories.impl
{
    public class AccountService : IAccountService
    {
        private readonly MyContext context;
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IConfiguration configuration,
            MyContext context,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper
        )
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.roleManager = roleManager;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }

        public async Task<(string, string)> SignInAsync(SignInModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.password))
            {
                return (string.Empty, string.Empty);
            }

            var result = await signInManager.PasswordSignInAsync(model.username, model.password, false, false);
            if (!result.Succeeded)
            {
                return (string.Empty, string.Empty);
            }

            var token = await GenerateTokenAsync(user, model.username);
            var refreshToken = GenerateRefreshToken(user);
            await StoreRefreshTokenAsync(refreshToken);

            return (token, refreshToken);
        }

        public async Task<IdentityResult> SignUpAsync(SignUpModel model)
        {
            var user = new User
            {
                Email = model.email,
                UserName = model.userName,
                Avatar = model.Avatar
            };

            var result = await userManager.CreateAsync(user, model.password);
            if (result.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(AppRole.User))
                {
                    await roleManager.CreateAsync(new IdentityRole(AppRole.User));
                }

                await userManager.AddToRoleAsync(user, AppRole.User);
            }

            return result;
        }

        private async Task<string> GenerateTokenAsync(User user, string userName)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole.ToString()));
            }

            var authenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authenKey, SecurityAlgorithms.HmacSha512Signature)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("purpose", "refresh_token"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var refreshToken = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddMonths(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(refreshToken);
        }

        public async Task<(string, string)> RefreshTokenAsync(string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(refreshToken);
            if (principal == null)
            {
                return (string.Empty, string.Empty);
            }

            var userName = principal.Identity.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return (string.Empty, string.Empty);
            }

            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return (string.Empty, string.Empty);
            }

            var storedRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == refreshToken);
            if (storedRefreshToken == null || storedRefreshToken.Expires <= DateTime.Now)
            {
                return (string.Empty, string.Empty);
            }

            context.RefreshTokens.Remove(storedRefreshToken);
            await context.SaveChangesAsync();

            var newRefreshToken = GenerateRefreshToken(user);
            var newToken = await GenerateTokenAsync(user, userName);

            if (string.IsNullOrEmpty(newToken) || string.IsNullOrEmpty(newRefreshToken))
            {
                return (string.Empty, string.Empty);
            }

            var refreshTokenEntity = new RefreshToken
            {
                Token = newRefreshToken,

                Expires = DateTime.Now.AddMonths(3)
            };

            await context.RefreshTokens.AddAsync(refreshTokenEntity);
            await context.SaveChangesAsync();

            return (newToken, newRefreshToken);
        }

        private async Task StoreRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.Now.AddMonths(3)
            };

            await context.RefreshTokens.AddAsync(refreshTokenEntity);
            await context.SaveChangesAsync();
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"])),
                ValidateLifetime = false // Allow expired tokens to be validated
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            if (securityToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512Signature,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                return principal;
            }

            throw new SecurityTokenException("Invalid token");
        }
        public async Task<AccountResponse> GetMyInfoAsync()
        {
            var username = httpContextAccessor.HttpContext.User.Identity.Name;
            var user = await userManager.FindByNameAsync(username);
            var roles = await userManager.GetRolesAsync(user);
            var userResponse = mapper.Map<AccountResponse>(user);
            userResponse.Roles = roles.ToList();
            return userResponse;
        }

        public async Task<List<AccountResponse>> GetAllCustomersAsync()
        {
            var users = userManager.Users.ToList();
            var customers = new List<AccountResponse>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains(AppRole.User))
                {
                    var customer = mapper.Map<AccountResponse>(user);
                    customers.Add(customer);
                }
            }

            return customers;
        }

        public async Task<List<AccountResponse>> GetAllSystemUsersAsync()
        {
            var users = userManager.Users.ToList();
            var customers = new List<AccountResponse>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Contains(AppRole.Admin) || roles.Contains(AppRole.Staff))
                {
                    var customer = mapper.Map<AccountResponse>(user);
                    customers.Add(customer);
                }
            }

            return customers;
        }



    }
}