using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Ticket
{
    public class CreateTicketDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        [MaxLength(200)]
        public required string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public required string Description { get; set; }

        public Guid? AssignmentId { get; set; }
    }
}
