/*
 TEST ACCOUNTS:
    - ADMIN:   admin@admin.com
    - ADVISOR: 00000
    - USER:    user@test.com
    Password for all is Test_123
 */

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NuGet.Packaging;
using WealthWise_RCD.Models;
using WealthWise_RCD.Models.DatabaseModels;
using WealthWise_RCD.Services;

var builder = WebApplication.CreateBuilder(args);

// Add database connection to build
builder.Services.AddDbContext<ApplicationDbContext>(options => {
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {options.SignIn.RequireConfirmedEmail = false; })   //disable email confirmation
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
async Task CreateRolesandUsers(IServiceProvider serviceProvider)    // Role creation
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

    string[] roleNames = { "Admin", "User", "Advisor" };

    foreach (var roleName in roleNames)
    {
        var roleExists = await roleManager.RoleExistsAsync(roleName);
        if (!roleExists)
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Deafault Users
    await CreateUserIfNotExisting(userManager, dbContext, "admin@test.com", "admin@admin.com", "Test_123", "Admin", "Admin", "User", "306");
    await CreateUserIfNotExisting(userManager, dbContext, "advisor@test.com", "00000", "Test_123", "Advisor", "Test", "Advisor", "1");
    await CreateUserIfNotExisting(userManager, dbContext, "user@test.com", "user@test.com", "Test_123", "User", "Test", "User", "1");
}
async Task CreateUserIfNotExisting(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, string email, string username, string password,
                                   string role, string firstName, string lastName, string age)
{
    var user = await userManager.FindByEmailAsync(email);
    if (user == null)
    {
        var address = new Address
        {
            StreetName = "1451 Stadium Rd",
            City = "Brookings",
            State = AddressState.SD,
            ZipCode = "57007"
        };
        dbContext.Addresses.Add(address);
        await dbContext.SaveChangesAsync();

        user = new ApplicationUser
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true,

            FirstName = firstName,
            LastName = lastName,
            Age = age,
            AddressId = address.Id,
            Address = address
        };
        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
    else // Used for debugging
    {
        //await userManager.DeleteAsync(user);
        
    }
}
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.Cookie.HttpOnly = true;
    options.Cookie.Expiration = null; // Expire on browsing close
    options.SlidingExpiration = false; // Prevent extending session
    options.Cookie.IsEssential = true; // Ensure it is deleted on logout
});

// Add session for temporary blog data (Commented out for testing auth)
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30);
//    options.Cookie.HttpOnly = true;
//});

// Add controllers and views
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews(options =>
{
var policy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser()
                 .Build();
options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
}).AddRazorPagesOptions(options =>
{
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Login");
    options.Conventions.AllowAnonymousToPage("/Identity/Account/Register");
});


#region Services
// Add email sender service
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<BlogService>();
#endregion

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
//app.UseSession();     Commented out for auth testing

app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRolesandUsers(services);
}

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/Identity/Account/AccessDenied" && context.User.Identity.IsAuthenticated)
    {
        if (context.User.IsInRole("Advisor"))
        {
            context.Response.Redirect("/Advisor/Home/Index");
            return;
        }
        else if (context.User.IsInRole("User") || context.User.IsInRole("Admin"))
        {
            context.Response.Redirect("/User/Home/Index");
            return;
        }
    }
    await next();
});

app.UseEndpoints(endpoints =>
{
    // Identity routes (Ensures login/register work)
    endpoints.MapRazorPages();

    // Advisor Area
    endpoints.MapControllerRoute(
        name: "advisor",
        pattern: "{area=Advisor}/{controller=Home}/{action=Index}/{id?}"
    );

    // User Area
    endpoints.MapControllerRoute(
        name: "user",
        pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}"
    );

    // Default route
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{area=Identity}/{controller=Account}/{action=Login}/{id?}"
    );
});




app.MapRazorPages();

app.Run();