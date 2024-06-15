using HaveFun.Models;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HaveFun.Common
{
    public class Jwt
    {
        IConfiguration _iconfiguration;
        public Jwt(IConfiguration iconfiguration)
        {
            _iconfiguration = iconfiguration;
        }

        public string GenerateJWTToken(UserInfo userinfo)
        {
            var jwtSecret = _iconfiguration["Jwt:secret"];
            if (string.IsNullOrEmpty(jwtSecret))
            {
                throw new Exception("JWT secret沒找到");
            }
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, userinfo.Id.ToString()),
                new Claim(ClaimTypes.Name, userinfo.Account),
            };
            var jwtToken = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(jwtSecret)
                        ),
                    SecurityAlgorithms.HmacSha256Signature)
                );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
