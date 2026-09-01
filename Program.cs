using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Auditor.Data;
using Auditor.Configs;
using Auditor.Services;
using Auditor.Services.Interfaces;
using Microsoft.AspNetCore.HttpLogging;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<EmailConfig>(builder.Configuration.GetSection("MailConfig"));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddHttpLogging(logging =>
{
    logging.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath | HttpLoggingFields.ResponseStatusCode;
});

builder.Services.AddDbContext<AuditorDb>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultSqlite")));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFlagNotificationProvider, FlagNotificationProvider>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IBoardManager, BoardManager>();
builder.Services.AddControllers();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.Cookie.Name = "AuditorToken";
    options.Cookie.HttpOnly = true;
    options.LoginPath = "/login/index";
    options.LogoutPath = "/api/signout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/dashboard");
    options.Conventions.AddFolderRouteModelConvention("/dashboard", model =>
    {
        foreach (var selector in model.Selectors)
        {
            var template = selector.AttributeRouteModel?.Template;
            var pageName = template?.Substring("dashboard".Length).TrimStart('/');

            selector.AttributeRouteModel?.Template = $"{{DashboardId:int}}/{pageName}";
        }
    });
});


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHttpLogging();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AuditorDb>();
    db.Database.Migrate();
}

app.Run();
