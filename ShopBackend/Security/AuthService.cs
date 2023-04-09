using Microsoft.IdentityModel.Tokens;
using ShopBackend.Dtos;
using ShopBackend.Models;
using ShopBackend.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShopBackend.Security
{
    //Build with inspiration from: https://www.youtube.com/watch?v=iIsaEzNXhoo&ab_channel=TrevoirWilliams
    public class AuthService : IAuthService
    {
        private readonly ICustomerRepository _customerRepository;

        private readonly IConfiguration _configuration;

        private readonly IPasswordAuth _passwordAuth;

        private Customer? _user;

        public AuthService(ICustomerRepository customerRepository, IConfiguration Configuration, IPasswordAuth passwordAuth)
        {
            _customerRepository = customerRepository;
            _configuration = Configuration;
            _passwordAuth = passwordAuth;
        }

        public async Task<bool> AuthenticateUser(LoginDto loginDto)
        {
            _user = await _customerRepository.Get(loginDto.Email);
            return (_user != null && _passwordAuth.VerifyPassword(_user.Password, loginDto.Password));
        }

        public string CreateToken()
        {
            var tokenSigningCredentials = GetTokenSigningCredentials();
            var tokenClaims = GetTokenClaims();
            var jwtToken = CreateJwtToken(tokenSigningCredentials, tokenClaims);
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        private SigningCredentials GetTokenSigningCredentials()
        {
            var key = _configuration.GetSection("Jwt").GetSection("Key");
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.Value!));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private List<Claim> GetTokenClaims()
        {
            var tokenClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, _user!.Email)
            };

            return tokenClaims;
        }

        private JwtSecurityToken CreateJwtToken(SigningCredentials tokenSigningCredentials, List<Claim> tokenClaims)
        {
            var settings = _configuration.GetSection("Jwt");

            var expireDateTime = DateTime.UtcNow.AddMinutes(Convert.ToDouble(settings.GetSection("Expires").Value));

            var jwtToken = new JwtSecurityToken(
                issuer: settings.GetSection("Issuer").Value,
                claims: tokenClaims,
                signingCredentials: tokenSigningCredentials,
                expires: expireDateTime
                );

            return jwtToken;
        }

        public string GetEmailFromToken(ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
        }
    }
}
