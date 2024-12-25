var builder = WebApplication.CreateBuilder(args);

// Razor Pages ve MVC deste�i
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Statik dosyalar ve y�nlendirme
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
