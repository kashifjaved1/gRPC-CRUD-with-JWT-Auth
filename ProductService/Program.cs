using GrpcProductService.Extensions;
using GrpcProductService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ProjectSettings(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();

app.MapGrpcService<AuthService>();
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
