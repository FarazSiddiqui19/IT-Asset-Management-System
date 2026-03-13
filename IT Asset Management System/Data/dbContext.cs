using Microsoft.EntityFrameworkCore;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Data.Configurations; // Include namespace for configurations

namespace IT_Asset_Management_System.Data
{
    public class dbContext : DbContext
    {
        public dbContext(DbContextOptions<dbContext> options) : base(options)
        {
            Users = Set<User>();
            Categories = Set<Category>();
            Products = Set<Product>();
            Assets = Set<Asset>();
            AssignmentRequests = Set<AssignmentRequest>();
            Assignments = Set<Assignment>();
            Tickets = Set<Ticket>();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AssignmentRequest> AssignmentRequests { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; } // Added DbSet for Comment

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new AssetConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentRequestConfiguration());
            modelBuilder.ApplyConfiguration(new AssignmentConfiguration());
            modelBuilder.ApplyConfiguration(new TicketConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration()); // Applied CommentConfiguration
        }
    }
}


