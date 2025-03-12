using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WealthWise_RCD.Models.DatabaseModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WealthWise_RCD.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Advice> Advice { get; set; }
        public DbSet<AdvisorEvent> AdvisorEvents { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Blog> BlogPosts { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<MonthlyBudget> MonthlyBudgets { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Reference> References { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL("server=localhost;port=3306;database=wealthwise;user=root;password=rcd306");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string testAdvId = "7d948a2b-f258-4cf8-b8c8-913806968f8f";
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AdvisorEvent>(e =>
            {
                e.HasOne(e => e.Advisor)
                .WithMany(e => e.AdvisorEvents)
                .HasForeignKey(e => e.AdvisorId);
            });

            // Data seeding
            modelBuilder.Entity<Blog>().HasData(
                new Blog { Id = 1, Title = "Time spent with cats is never wasted.", Topic = "Topic", PublicationDate = DateTime.Now, Content = "Pulled from the database (Quote by Sigmund Freud)!", AdvisorId = testAdvId },
                new Blog { Id = 2, Title = "You can never be truly at home without a cat.", Topic = "Topic", PublicationDate = DateTime.Now, Content = "Pulled from the database (Quote by Mark Twain)!", AdvisorId = testAdvId },
                new Blog
                {
                    Id = 3,
                    Title = "The smallest feline is a masterpiece. - Leonardo Da Vinci",
                    Topic = "Topic",
                    PublicationDate = DateTime.Now,
                    Content = @"Soft as twilight, sleek as night,\n
                                       A shadow drifts in silver light.\n
                                       Silent steps on wooden floors,\n
                                       A ghost that slips through open doors.\n
                                       \n
                                       Eyes like lanterns, gleam and glow,\n
                                       Holding secrets none may know.\n
                                       A fleeting brush, a velvet sigh,\n
                                       Then gone—like wind, like lullabies.\n
                                       \n
                                       Curled in sunlight, lost in dreams,\n
                                       Of silent hunts by moonlit streams.\n
                                       No chains, no ties—just fleeting grace,\n
                                       A traveler in time and space.\n
                                       \n
                                       And when you sleep, beneath the stars,\n
                                       A whisper hums from realms afar.\n
                                       A cat’s soft purr, a sacred song,\n
                                       Reminding you—you do belong.",
                    AdvisorId = testAdvId
                }
            );
        }
    }
}
