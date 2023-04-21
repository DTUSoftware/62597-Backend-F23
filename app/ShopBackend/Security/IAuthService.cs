using ShopBackend.Dtos;
using System.Security.Claims;

namespace ShopBackend.Security
{
    public interface IAuthService
    {
        Task<bool> AuthenticateUser(LoginDto loginDto);
        string CreateToken();
        string GetEmailFromToken(ClaimsPrincipal claimsPrincipal);
        string GetRoleFromToken(ClaimsPrincipal claimsPrincipal);
    }
}
