using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ASP.NETAuthITStep.Auth.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ASP.NETAuthITStep.Auth
{
    public class JwtService
    {
        private JwtOptions _options;

        public JwtService(IOptions<JwtOptions> options) => _options = options.Value;

        public string CreateToken(ClaimsPrincipal principal)
        {
            var now = DateTime.Now;

            var jwt = new JwtSecurityToken(
                notBefore: now,
                expires: now.AddMilliseconds(_options.TokenLifetime),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                )
            );
            
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt; 
        }
    }
}