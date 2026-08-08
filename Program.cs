using Auditor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Auditor.Configs;
using Auditor.Services;
using Auditor.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("MailConfig"));

builder.Services.AddDbContext<AuditorDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSqlite")));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFlagNotificationProvider, FlagNotificationProvider>();
builder.Services.AddScoped<IUserManager, UserManager>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.Name = "AuditorToken";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/login/index";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuditorDb>();
    db.Database.Migrate();
}

app.Run();
