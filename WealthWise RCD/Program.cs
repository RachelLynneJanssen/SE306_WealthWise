using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WealthWise_RCD.Models;
using WealthWise_RCD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add database connection to build
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();

//builder.Services.AddControllersWithViews(options =>
//{
//    // Apply a global authorization policy to require authentication
//    // TURNED OFF FOR DEBUGGING
//    var policy = new AuthorizationPolicyBuilder()
//                     .RequireAuthenticatedUser()
//                     .Build();
//    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
//});

builder.Services.AddSingleton<IEmailSender, EmailSender>();


// Add services to the container.
builder.Services.AddRazorPages()
    .WithRazorPagesRoot("/Views");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// TEMPORARILY DISABLED FOR DEBUGGING
//app.UseAuthentication();
//app.UseAuthorization();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute
//    (
//        name: "default",
//        pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}"
//    );
//});
/*app.MapGet("/", async context =>
{
    context.Response.Redirect("/Identity/Account/Login");
});*/

app.MapControllerRoute(
    name: "default",
    pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
