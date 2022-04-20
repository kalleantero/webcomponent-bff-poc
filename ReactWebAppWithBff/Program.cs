using AuthCore;
using BffCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// 1. Add reverse proxy configuration (YARP)
builder.Services.AddReverseProxy(builder.Configuration);

// 2. Add authentication configuration
builder.Services.AddAuthenticationForWeb(builder.Configuration);

builder.Services.AddHttpClient(); // Add HttpClient here

// 3. Add authorization configuration
builder.Services.AddAuthorization(options =>
{
    // This is a default authorization policy which requires authentication
    options.AddPolicy("RequireAuthenticatedUserPolicy", policy =>
    {
        policy.RequireAuthenticatedUser();
    });
});

var app = builder.Build();

// 4. Enable exception handler middleware in pipeline
app.UseExceptionHandler("/?error");

// 5. Enable HSTS middleware in pipeline
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
// 6. Enable https redirection middleware in pipeline
app.UseHttpsRedirection();

// 7. Enable routing middleware in pipeline
app.UseRouting();

// 8. Enable authentication middleware in pipeline
app.UseAuthentication();

// 9. Enable routing middleware in pipeline
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    // 10. Enable YARP reverse proxy middleware in pipeline
    endpoints.MapReverseProxy();
    // 11. Enable BFF login, logout and userinfo endpoints
    endpoints.AddBffEndpoints(builder.Configuration);
});

app.MapFallbackToFile("index.html"); ;

app.Run();
