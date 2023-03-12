using Fateor;
using Fateor.Database.MySql;
using Fateor.Infrastructure.Services;
using Fateor.Setup;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
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

builder.Services.Configure<CookieTempDataProviderOptions>(options =>
{
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

var binPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
var scriptPath = Path.Combine(binPath ?? throw new Exception("No Bin"), "Scripts");
var dbUpdater = new DBUpdates(builder.Configuration.GetConnectionString("fateor"), scriptPath);
var dbUpdateLog = dbUpdater.Update();

(app.Services.GetService<ILogger<DBUpdates>>() ?? throw new Exception("no log")).LogInformation(dbUpdateLog);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseSession();

app.UseStaticFiles();

app.UseRouting();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
