using Microsoft.EntityFrameworkCore;
using Tutoring_Platform.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Tutoring_Platform.CustomModels;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// add services to the container.
builder.Services.AddControllersWithViews();
/**string domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("read:messages", policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain)));
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();*/

builder.Services.AddDbContext<tutoringContext>(options =>
                options.UseSqlServer("Data Source=tutoringplatform-199.database.windows.net,1433;Initial Catalog=tutoring;User ID=amandeep_kaur;Password=Aman543!"));
var app = builder.Build();

// configure the http request pipeline.
if (!app.Environment.IsDevelopment())
{
    
}

app.UseStaticFiles();
app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
