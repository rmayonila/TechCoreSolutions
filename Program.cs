using TechCoreSolutions.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container.
builder.Services.AddControllersWithViews();

// 2. Add DbContext - Configured specifically for SQL Server
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // This tells EF Core to use the SQL Server provider and the string from appsettings.json
    options.UseSqlServer(connectionString, sqlOptions => 
        sqlOptions.EnableRetryOnFailure());
});

var app = builder.Build();

// 3. Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();