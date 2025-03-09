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
    await CreateUserIfNotExisting(userManager, dbContext, "admin@admin.com", "admin@admin.com", "RCD_se306", "Admin", "Admin", "User", "306");
    await CreateUserIfNotExisting(userManager, dbContext, "testAdvisor@advisor.com", "0000000", "testAdvisor_123", "Advisor", "Test", "Advisor", "1");
    await CreateUserIfNotExisting(userManager, dbContext, "testUser@user.com", "testUser@user.com", "testUser_123", "User", "Test", "User", "1");
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
}

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
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
    // Apply a global authorization policy to require authentication
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter(policy));
});

// Add email sender service
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
//app.UseSession();     Commented out for auth testing

app.UseAuthentication();
app.UseAuthorization();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await CreateRolesandUsers(services);
}

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
        pattern: "{area=User}/{controller=Home}/{action=Index}/{id?}"
    );
});


app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        var user = context.User;

        if (!user.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/Identity/Account/Login");
            return;
        }
        else if (user.IsInRole("Advisor"))
        {
            context.Response.Redirect("/Advisor/Home/Index");
            return;
        }
        else if (user.IsInRole("User") || user.IsInRole("Admin"))
        {
            context.Response.Redirect("/User/Home/Index");
            return;
        }
        else
        {
            context.Response.Redirect("/Shared/Home"); // Default fallback
            return;
        }
    }
    await next();
});

app.MapRazorPages();

app.Run();