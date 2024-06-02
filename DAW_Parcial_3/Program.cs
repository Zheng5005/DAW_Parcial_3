using Microsoft.EntityFrameworkCore;
using DAW_Parcial_3.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(Options =>
    {
        Options.LoginPath = "/Login/Index";
    });

builder.Services.AddSession(Options =>
{
    Options.IdleTimeout = TimeSpan.FromMinutes(60);
    Options.Cookie.HttpOnly = true;
    Options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<DAWContext>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DbConnection")
        ));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
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
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
