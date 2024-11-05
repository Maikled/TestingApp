using TestingApp.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace TestingApp.Areas.Authentication.Models.Configurations
{
    public class AuthenticationConfiguration : IServiceConfiguration
    {
        public string TokenIssuer = "AutoTestingServer";
        public string TokenAudience = "AutoTestingClient";

        private string _tokenKey;

        public AuthenticationConfiguration()
        {
            var pathToTokenFile = Path.Combine(Environment.CurrentDirectory, "Areas", "Authentication", "Data", "secretToken.txt");
            _tokenKey = LoadSecretTokenKey(pathToTokenFile);
        }

        public void ConfigureService(IServiceCollection services)
        {
            services.AddSingleton<AuthenticationConfiguration>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = TokenIssuer,
                    ValidateAudience = true,
                    ValidAudience = TokenAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true
                };
            });
        }

        public void ConfigureApp(WebApplication app)
        {
            app.UseAuthentication();
        }

        protected SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenKey));
        }

        private string LoadSecretTokenKey(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            {
                return reader.ReadToEnd();
            }
        }

        protected string GenerateToken(string username)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username)
            };

            var jwtToken = new JwtSecurityToken(
                issuer: TokenIssuer,
                audience: TokenAudience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
