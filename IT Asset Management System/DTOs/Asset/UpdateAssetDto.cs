using System.ComponentModel.DataAnnotations;
using IT_Asset_Management_System.Entities.Enums;

namespace IT_Asset_Management_System.DTOs.Asset
{
    public class UpdateAssetDto
    {
        [Required]
        public AssetStatus Status { get; set; }
    }
}
