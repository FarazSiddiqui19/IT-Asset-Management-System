using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(c => c.Name)
                .IsUnique();

            // Navigation properties
            builder.HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); // Business rule: A category cannot be deleted if products exist under it.

            builder.HasMany(c => c.AssignmentRequests)
                .WithOne(ar => ar.Category)
                .HasForeignKey(ar => ar.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}