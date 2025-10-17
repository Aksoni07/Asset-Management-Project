using System; // Imports basic .NET features ---> DateTime
using System.ComponentModel.DataAnnotations;  // Imports the library for validation attributes like  --> Required.

namespace AssetManagement.Core.Entities
{
    public class AssetAssignmentHistory
    {
        public int Id { get; set; }

        [Required]
        public int AssetId { get; set; }
        public Asset Asset { get; set; } // link: Asset --> AssetAssignmentHistory

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } // link: Asset --> AssetAssignmentHistory

        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }
    }
}

//Act as Linkage:  Many to Many : Asset <---> Employee