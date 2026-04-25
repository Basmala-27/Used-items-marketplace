using Microsoft.EntityFrameworkCore; // 1. ??? ??? ?????
using MarketplaceApp.Data;         // 2. ??? ??? ?????? ???? ??? ??? DbContext

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// -----------------------------------------------------------
// ?? ????? ????? ???????? (SQLite)
// -----------------------------------------------------------
// ????? ??? Connection String ???? ??? ?????? ?? appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// ????? ??? DbContext ?? ???? ??? ??????? (Dependency Injection)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
// -----------------------------------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
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