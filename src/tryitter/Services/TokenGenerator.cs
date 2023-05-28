using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using tryitter.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using tryitter.Constants;


namespace tryitter.Services
{
    public class TokenGenerator
    {
        public string Generate(Student student)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = AddClaims(student),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenConstants.Secret)), SecurityAlgorithms.HmacSha256Signature),
                Expires = DateTime.Now.AddDays(3)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        private ClaimsIdentity AddClaims(Student student)
        {
            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.Name, student.Name));
            claims.AddClaim(new Claim(ClaimTypes.Email, student.Email));
            // claims.AddClaim(new Claim(ClaimTypes.UserData, client.IsCompany ? "PessoaJuridica" : "PessoaFisica"));

            return claims;
        }
    }
}