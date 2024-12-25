var builder = WebApplication.CreateBuilder(args);

// Razor Pages ve MVC desteði
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Statik dosyalar ve yönlendirme
app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
