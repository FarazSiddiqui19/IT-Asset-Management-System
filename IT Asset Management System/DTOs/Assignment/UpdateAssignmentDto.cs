using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.Assignment
{
    public class UpdateAssignmentDto
    {
        [Required]
        public AssignmentStatus Status { get; set; }
    }
}
