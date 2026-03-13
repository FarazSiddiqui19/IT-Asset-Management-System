using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Ticket
{
    public class DeleteTicketDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid User { get; set; }  

   
    }
}
