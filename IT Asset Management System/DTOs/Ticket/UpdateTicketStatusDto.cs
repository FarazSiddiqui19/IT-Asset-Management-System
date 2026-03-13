using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.Ticket
{
    public class UpdateTicketStatusDto
    {
        [Required]
        public TicketStatus Status { get; set; }

        [Required]
        public Guid AdminId { get; set; }
    }
}
