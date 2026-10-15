using Microsoft.AspNetCore.Mvc;
using ReyadConsultancy.Application;
using ReyadConsultancy.Infrastructure;
using ReyadConsultancy.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllersWithViews(options =>
{
    // Every POST must carry an anti-forgery token (all forms in this project include one).
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/admin/Account/Login";
    options.AccessDeniedPath = "/admin/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromHours(8);
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error/500");
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/error/{0}");
app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    context.Response.Headers["X-Content-Type-Options"] = "nosniff";
    context.Response.Headers["X-Frame-Options"] = "SAMEORIGIN";
    context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
    await next();
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Admin panel lives under /admin/...
app.MapControllerRoute(
    name: "admin",
    pattern: "admin/{controller=Dashboard}/{action=Index}/{id?}",
    defaults: new { area = "Admin" });

// Public website uses attribute routes (/, /about, /services/{slug}, /blog/{slug} ...)
app.MapControllers();

await DbInitializer.InitializeAsync(app.Services);

app.Run();
