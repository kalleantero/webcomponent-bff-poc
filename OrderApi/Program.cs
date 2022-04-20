using AuthCore;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddAuthenticationForApi(builder.Configuration);

builder.Services.AddAuthorization(o => o.AddPolicy("OrdersPolicy",
                                  b => b.RequireClaim("scope", "api")));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/orders", [Authorize](IHttpContextAccessor httpContextAccessor) => new[]{
    new
    {
        Id = 1,
        TotalPrice = 100,
        CreatedBy = httpContextAccessor.HttpContext?.User?.Identity?.Name,
        CreatedAtUtc = DateTime.UtcNow,
         Products = new[]{
            new {
                Id = 1
            }
        }
    },
    new
    {
        Id = 2,
        TotalPrice = 300,
        CreatedBy = httpContextAccessor.HttpContext?.User?.Identity?.Name,
        CreatedAtUtc = DateTime.UtcNow,
        Products = new[]{
            new { 
                Id = 1 
            },
            new {
                Id = 2
            }
        }
    }
}).RequireAuthorization("OrdersPolicy");

app.Run();