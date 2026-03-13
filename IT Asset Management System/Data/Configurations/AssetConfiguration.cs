using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.AssetTag)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(a => a.AssetTag)
                .IsUnique();

            builder.Property(a => a.PurchaseDate)
                .IsRequired(); // Made nullable, removed default value

            builder.Property(a => a.ProductId)
                .IsRequired();

            builder.Property(a => a.Status)
                .IsRequired()
                .HasDefaultValue(AssetStatus.Available); // Set default status

            // Navigation properties
            builder.HasOne(a => a.Product)
                .WithMany(p => p.Assets)
                .HasForeignKey(a => a.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Assignments)
                .WithOne(assign => assign.Asset)
                .HasForeignKey(assign => assign.AssetId)
                .OnDelete(DeleteBehavior.Restrict); // Business rule: An Asset cannot be deleted if assignment history exists.
        }
    }
}