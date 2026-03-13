using System.ComponentModel.DataAnnotations;

namespace IT_Asset_Management_System.DTOs.Asset
{
    public class CreateAssetDto
    {
        [Required]
        [MaxLength(50)]
        public required string AssetTag { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [Required]
        public Guid ProductId { get; set; }
    }
}
