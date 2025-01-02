using System.Text;
using System.Security.Claims;
using EsyaStore.Data.Entity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace EsyaStoreApi.Extensions
{
     public class TokenGenerator
    {
        private readonly IConfiguration configuration;
        public TokenGenerator(IConfiguration _configuration) {
            configuration = _configuration;
        }
        public string CreateToken(string Name,string Email,int Id,string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.NameIdentifier,Id.ToString()),
                new Claim("UserType",role)
            };

            string Jwtsecret = configuration["Jwt:Secret"];
            var securityKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Jwtsecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token=new JwtSecurityToken (
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                expires:DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credentials,
                claims:claims
            );

            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }  
    }
}
