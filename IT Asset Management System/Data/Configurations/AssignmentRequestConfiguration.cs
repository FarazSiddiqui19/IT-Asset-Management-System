using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class AssignmentRequestConfiguration : IEntityTypeConfiguration<AssignmentRequest>
    {
        public void Configure(EntityTypeBuilder<AssignmentRequest> builder)
        {
            builder.HasKey(ar => ar.Id);

            builder.Property(ar => ar.UserId)
                .IsRequired();

            builder.Property(ar => ar.CategoryId)
                .IsRequired();

            builder.Property(ar => ar.CreatedAt)
                .IsRequired();
            
            builder.Property(ar => ar.Description)
                .IsRequired()
                .HasMaxLength(500);
            
  

       

            builder.Property(ar => ar.Status)
                .IsRequired()
                .HasDefaultValue(RequestStatus.Pending); // Request.Status defaults to Pending.

            builder.Property(ar => ar.ProcessedByAdminId)
                .IsRequired(false);

            // Navigation properties
            builder.HasOne(ar => ar.User)
                .WithMany(u => u.AssignmentRequests)
                .HasForeignKey(ar => ar.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.Category)
                .WithMany(c => c.AssignmentRequests)
                .HasForeignKey(ar => ar.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ar => ar.ProcessedByAdmin)
                .WithMany()
                .HasForeignKey(ar => ar.ProcessedByAdminId)
                .OnDelete(DeleteBehavior.SetNull);

            // Configure the one-to-one relationship with Assignment
            builder.HasOne(ar => ar.Assignment) // AssignmentRequest has one Assignment
                .WithOne(a => a.Request)        // Assignment has one AssignmentRequest
                .HasForeignKey<Assignment>(a => a.RequestId) // Assignment.RequestId is the FK
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion
        }
    }
}