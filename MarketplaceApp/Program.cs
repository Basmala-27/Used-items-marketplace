using MarketplaceApp.Data;
using MarketplaceApp.Hubs;
using MarketplaceApp.Models;
using MarketplaceApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();



var app = builder.Build();

app.MapHub<NotificationHub>("/notificationHub");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
// فتح "بوابة" للوصول لخدمات قاعدة البيانات والـ Identity
using (var scope = app.Services.CreateScope())
{
    // استدعاء الأجهزة المسؤولة عن الرتب (Roles) والمستخدمين (Users)
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    // 1. التأكد من أن رتبة "Admin" موجودة في الدفتر (قاعدة البيانات)
    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    // 2. البحث عن المستخدم اللي إيميله كذا عشان نرقّيه
    var adminEmail = "shant1blodm1@gmail.com"; // اكتبي إيميلك الحقيقي هنا
    var user = await userManager.FindByEmailAsync(adminEmail);

    // 3. لو اليوزر موجود ولسه مش أدمن، خليه أدمن فوراً
    if (user != null && !await userManager.IsInRoleAsync(user, "Admin"))
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
}

app.Run();