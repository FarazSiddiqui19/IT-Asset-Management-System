using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class CommentConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.UserId)
                .IsRequired();

            builder.Property(c => c.TicketId)
                .IsRequired(false);

            builder.Property(c => c.AssignmentRequestId)
                .IsRequired(false);

            builder.Property(c => c.Type)
                .IsRequired();

            builder.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(2000); // Assuming a reasonable max length for comment content

            builder.Property(c => c.CreatedAt)
                .IsRequired();

          

            // Relationships
            builder.HasOne(c => c.User)
                .WithMany(u => u.Comments) // User can have many comments
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Restrict deletion of user if comments exist

            builder.HasOne(c => c.Ticket)
                .WithMany(t => t.Comments) // Ticket can have many comments
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade); // If ticket is deleted, delete associated comments

            builder.HasOne(c => c.AssignmentRequest)
                .WithMany(ar => ar.Comments)
                .HasForeignKey(c => c.AssignmentRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}