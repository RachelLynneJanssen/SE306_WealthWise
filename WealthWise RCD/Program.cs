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

    // Additional Advisors
    await CreateUserIfNotExisting(userManager, dbContext, "cd@test.com", "00001", "Test_123", "Advisor", "Charles", "Dickens", "1");
    await CreateUserIfNotExisting(userManager, dbContext, "mt@test.com", "00002", "Test_123", "Advisor", "Mark", "Twain", "1");
    await CreateUserIfNotExisting(userManager, dbContext, "js@test.com", "00003", "Test_123", "Advisor", "John", "Steinbeck", "1");
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
            if(username == "00000")
            {
               await SeedInitBlogPosts(userManager, dbContext, user);
            }
        }
    }
    else // Used for debugging
    {
        //await userManager.DeleteAsync(user);
        
    }
}
async Task SeedInitBlogPosts(UserManager<ApplicationUser> userManager, ApplicationDbContext dbContext, ApplicationUser advisor)
{
    Blog post1 = new() { Id = 1, Title = "Time spent with cats is never wasted.", Topic = "Topic", PublicationDate = DateTime.Now, Content = "Pulled from the database (Quote by Sigmund Freud)!", AdvisorId = advisor.Id };
    Blog post2 = new() { Id = 2, Title = "You can never be truly at home without a cat.", Topic = "Topic", PublicationDate = DateTime.Now, Content = "Pulled from the database (Quote by Mark Twain)!", AdvisorId = advisor.Id };
    Blog post3 = new() {
        Id = 3,
        Title = "The smallest feline is a masterpiece. - Leonardo Da Vinci",
        Topic = "Topic",
        PublicationDate = DateTime.Now,
        Content = @"<p>Soft as twilight, sleek as night,</p>
                    <p>A shadow drifts in silver light.</p>
                    <p>Silent steps on wooden floors,</p>
                    <p>A ghost that slips through open doors.</p>
                    <p><br></p>
                    <p>Eyes like lanterns, gleam and glow,</p>
                    <p>Holding secrets none may know.</p>
                    <p>A fleeting brush, a velvet sigh,</p>
                    <p>Then gone—like wind, like lullabies.</p>
                    <p><br></p>
                    <p>Curled in sunlight, lost in dreams,</p>
                    <p>Of silent hunts by moonlit streams.</p>
                    <p>No chains, no ties—just fleeting grace,</p>
                    <p>A traveler in time and space.</p>
                    <p><br></p>
                    <p>And when you sleep, beneath the stars,</p>
                    <p>A whisper hums from realms afar.</p>
                    <p>A cat’s soft purr, a sacred song,</p>
                    <p>Reminding you—you do belong.</p>",
        AdvisorId = advisor.Id
    };
    dbContext.BlogPosts.Add(post1);
    dbContext.BlogPosts.Add(post2);
    dbContext.BlogPosts.Add(post3);

    
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
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<MonthlyBudgetService>(); 
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