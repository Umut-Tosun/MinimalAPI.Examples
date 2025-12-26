using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Library.API.Services
{
    public sealed class JwtProvider
    {
        public string CreateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("fe1fbfb84daa049f153a9af62d88a7e23d7381050ad63f629d6c6b557ee7c108"));
            JwtSecurityToken jwtSecurityTokentoken = new JwtSecurityToken(issuer: "Umut Tosun",audience: "Umut Tosun",claims: null,notBefore:DateTime.Now,expires:DateTime.Now.AddDays(30),signingCredentials: new SigningCredentials(securityKey,SecurityAlgorithms.HmacSha256));

            JwtSecurityTokenHandler handler = new();
            string token = handler.WriteToken(jwtSecurityTokentoken);

            return token;
        }
    }
}
