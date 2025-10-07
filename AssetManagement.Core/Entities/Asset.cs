using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Core.Entities
{
    public class Asset
    {
        public int AssetId { get; set; }

        [Required]
        [StringLength(100)]
        public string AssetName { get; set; }
        
        [Required]
        public string AssetType { get; set; }

        [StringLength(100)]
        public string MakeModel { get; set; }

        [Required]
        [StringLength(50)]
        public string SerialNumber { get; set; }
        
        public DateTime PurchaseDate { get; set; }
        public DateTime WarrantyExpiryDate { get; set; }
        
        public string Condition { get; set; } // New, Good, Needs Repair, Damaged
        public string Status { get; set; } // Available, Assigned, Under Repair, Retired
        
        public bool IsSpare { get; set; }
        
        [StringLength(500)]
        public string Specifications { get; set; }

        public ICollection<AssetAssignmentHistory> AssetAssignmentHistories { get; set; }
    }
}