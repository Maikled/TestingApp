using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestingApp.Security
{
    public class JwtTokenSecurity
    {
        public string TokenIssuer = "TestingAppServer";
        public string TokenAudience = "TestingAppClient";

        private string _tokenKey;

        public JwtTokenSecurity()
        {
            var pathToTokenFile = Path.Combine(Environment.CurrentDirectory, "Areas", "Authentication", "Data", "secretToken.txt");
            _tokenKey = LoadSecretTokenKey(pathToTokenFile);
        }

        private string LoadSecretTokenKey(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        public SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));
        }

        public string GenerateToken(string username)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username)
            };

            var jwtToken = new JwtSecurityToken(
                issuer: TokenIssuer,
                audience: TokenAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
