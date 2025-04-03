using System.Text;
using GrpcProductService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;

namespace GrpcProductService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ProjectSettings(this IServiceCollection services, IConfiguration configuration)
        {
            // Add services to the container.
            services
                .AddGrpc()
                .AddJsonTranscoding(); // RPC JSON transcoding is an extension for ASP.NET Core that creates RESTful JSON APIs for gRPC services. Once configured, transcoding allows apps to call gRPC services with familiar HTTP concepts: HTTP verbs. URL parameter binding. https://learn.microsoft.com/en-us/aspnet/core/grpc/json-transcoding?view=aspnetcore-8.0.

            services.AddScoped<AuthService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var issuer = configuration["JWT:Issuer"];
                    var audiance = configuration["JWT:Audiance"];
                    var key = configuration["JWT:KEY"];
                    var keyBytes = Encoding.UTF8.GetBytes(key);
                    var symmetricKey = new SymmetricSecurityKey(keyBytes);

                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = issuer,
                        ValidAudience = audiance,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = symmetricKey
                    };
                });

            services.AddAuthorization();
        }
    }
}
