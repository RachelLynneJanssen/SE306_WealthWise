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
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AdvisorEvent>(e =>
            {
                e.HasOne(e => e.Advisor)
                .WithMany(e => e.AdvisorEvents)
                .HasForeignKey(e => e.AdvisorId);
            });

            // Data seeding
            //modelBuilder.Entity<Blog>().HasData(
            //    new Blog { Id = 1, Title = "Blog Post 1", Topic = "Topic", PublicationDate = DateTime.Now, Content = "This is the content of Blog Post 1." },
            //    new Blog { Id = 2, Title = "Blog Post 2", Topic = "Topic", PublicationDate = DateTime.Now, Content = "This is the content of Blog Post 2." },
            //    new Blog { Id = 3, Title = "Blog Post 3", Topic = "Topic", PublicationDate = DateTime.Now, Content = "This is the content of Blog Post 3." }
            //);
        }
    }
}
