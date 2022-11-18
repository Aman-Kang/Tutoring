using Microsoft.EntityFrameworkCore;
using Tutoring_Platform.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<tutoringContext>(options =>
                options.UseSqlServer("Data Source=tutoringplatform-199.database.windows.net,1433;Initial Catalog=tutoring;User ID=amandeep_kaur;Password=Aman543!;MultipleActiveResultSets=true"));
builder.Services.AddControllers();

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();
