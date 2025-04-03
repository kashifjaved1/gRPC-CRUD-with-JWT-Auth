using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcProductService.Protos;
using Microsoft.IdentityModel.Tokens;

namespace GrpcProductService.Services
{
    public class AuthService : AuthProtoService.AuthProtoServiceBase
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task<CreateIdentityResponse> GenerateToken(Empty request, ServerCallContext context)
        {
            var issuer = _configuration["JWT:Issuer"];
            var audiance = _configuration["JWT:Audiance"];
            var key = _configuration["JWT:KEY"];

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, Guid.NewGuid().ToString())
            };

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var symmetricKey = new SymmetricSecurityKey(keyBytes);
            var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddMinutes(10); //TimeSpan.FromDays(1);
            var _token = new JwtSecurityToken(issuer, audiance, claims, expires: expiry, signingCredentials: signingCredentials);

            string token = string.Empty;
            try
            {
                token = new JwtSecurityTokenHandler().WriteToken(_token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return new CreateIdentityResponse
            {
                Token = token
            };
        }
    }
}
