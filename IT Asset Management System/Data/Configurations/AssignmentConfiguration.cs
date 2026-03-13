using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.RequestId)
                .IsRequired();

            builder.Property(a => a.AssetId)
                .IsRequired();

            builder.Property(a => a.AssignedDate)
                .IsRequired();

            builder.Property(a => a.ReturnDate)
                .IsRequired(false); // Nullable

            builder.Property(a => a.Status)
                .IsRequired()
                .HasDefaultValue(AssignmentStatus.Active);


            // Navigation properties
            // The relationship to Request is now configured from AssignmentRequestConfiguration as a one-to-one,
            // so we remove the WithMany definition from here.
            // The foreign key is defined by AssignmentRequestConfiguration.
            builder.HasOne(a => a.Request)
                .WithOne(ar => ar.Assignment)
                .HasForeignKey<Assignment>(a => a.RequestId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Asset)
                .WithMany(asset => asset.Assignments)
                .HasForeignKey(a => a.AssetId)
                .OnDelete(DeleteBehavior.Restrict); // Business rule: Assignment records cannot be deleted (audit integrity).

            builder.HasMany(a => a.Tickets)
                .WithOne(t => t.Assignment)
                .HasForeignKey(t => t.AssignmentId)
                .OnDelete(DeleteBehavior.SetNull); // If an assignment is deleted, tickets referencing it can have their AssignmentId set to null.
        }
    }
}
