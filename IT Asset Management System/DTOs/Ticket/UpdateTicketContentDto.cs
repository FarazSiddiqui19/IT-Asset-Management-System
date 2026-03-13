using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Ticket
{
    public class UpdateTicketContentDto
    {
        [Required]
        public Guid UserId { get; set; }

        [MaxLength(200)]
        public string? Title { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }
    }
}
