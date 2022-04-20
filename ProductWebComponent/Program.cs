using AuthCore;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddAuthenticationForApi(builder.Configuration);

builder.Services.AddAuthorization(o => o.AddPolicy("ProductsPolicy",
                                  b => b.RequireClaim("scope", "api")));

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapGet("/products", [Authorize](IHttpContextAccessor httpContextAccessor) => new[]{ 
    new 
    { 
        Name = "Product 1", 
        Id = 1, 
        Price = 100,
        CreatedBy = httpContextAccessor.HttpContext?.User?.Identity?.Name,
        CreatedAtUtc = DateTime.UtcNow,
        Categories = new[]{ "Category 1" }
    },
    new 
    { 
        Name = "Product 2",
        Id = 2, 
        Price = 200,
        CreatedBy = httpContextAccessor.HttpContext?.User?.Identity?.Name,
        CreatedAtUtc = DateTime.UtcNow,
        Categories = new[]{ "Category 1", "Category 2" }
    }
}).RequireAuthorization("ProductsPolicy");

app.Run();

