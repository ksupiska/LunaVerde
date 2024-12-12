using Microsoft.EntityFrameworkCore;
using LunaVerde.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<LunaVerdeDBContext>(options =>
    options.UseSqlServer("Server=DESKTOP-VKIG8QK\\SQLEXPRESS; Database=Luna Verde; Trusted_Connection=True;" +
    "TrustServerCertificate=True"));
// Добавь сервисы для работы с сессиями
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Время жизни сессии
    options.Cookie.HttpOnly = true; // Сессии доступны только через HTTP
    options.Cookie.IsEssential = true; // Обязательно для работы сессий
});



// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
