using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(u => u.Username)
                .IsUnique();

            builder.Property(u => u.Role)
                .IsRequired();

            builder.Property(u => u.CreatedAt)
                .IsRequired();
            
            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);
        
            // Navigation properties
            builder.HasMany(u => u.AssignmentRequests)
                .WithOne(ar => ar.User)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Business rule: User with active assignments cannot be deactivated (implicitly handled by restricting deletion)

          

            builder.HasMany(u => u.CreatedTickets)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the relationship for AssignedToUser in Ticket
            builder.HasMany(u => u.AssignedTickets)
                .WithOne(t => t.AssignedToUser)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull); // If admin is deleted, set AssignedToUserId to null
        }
    }
}
