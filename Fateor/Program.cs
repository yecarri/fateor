using Fateor.Infrastructure.Services;
using Fateor.Setup;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(o =>
    {
        o.LoginPath = "/Login";
        o.LogoutPath ="/Logout";
        o.ExpireTimeSpan=TimeSpan.FromMinutes(20);
    });

var assemblies = AppDomain.CurrentDomain.GetAssemblies().OrderBy(a => a.FullName);

builder.Logging.AddFile(o => o.RootPath = builder.Environment.ContentRootPath);

builder.Services.AddMappers();

builder.Services.AddSingleton<IEncryptionService, AesEncryptionService>();

builder.AddSettings();

builder.AddRepositories();

builder.AddServices();

builder.Services.AddSession();

builder.Services.Configure<CookieTempDataProviderOptions>(options => {
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
