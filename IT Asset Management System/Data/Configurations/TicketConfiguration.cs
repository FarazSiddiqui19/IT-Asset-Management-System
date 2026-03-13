using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using IT_Asset_Management_System.Entities;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.Data.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.UserId)
                .IsRequired();

            builder.Property(t => t.AssignmentId)
                .IsRequired(false); // Nullable

            builder.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(t => t.Description)
                .HasMaxLength(2000)
                .IsRequired(); 

            builder.Property(t => t.Status)
                .IsRequired()
                .HasDefaultValue(TicketStatus.Open);

            builder.Property(t => t.AssignedToUserId)
                .IsRequired(false); // Nullable

            builder.Property(t => t.CreatedAt)
                .IsRequired();
            


            // Navigation properties
            builder.HasOne(t => t.User)
                .WithMany(u => u.CreatedTickets)
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Assignment)
                .WithMany(a => a.Tickets)
                .HasForeignKey(t => t.AssignmentId)
                .OnDelete(DeleteBehavior.SetNull); // If an assignment is deleted, tickets referencing it can have their AssignmentId set to null.

            builder.HasOne(t => t.AssignedToUser)
                .WithMany(u => u.AssignedTickets)
                .HasForeignKey(t => t.AssignedToUserId)
                .OnDelete(DeleteBehavior.SetNull); // If an admin is deleted, tickets assigned to them can have AssignedToUserId set to null.
        }
    }
}
