using System;
using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Core.Entities
{
    public class AssetAssignmentHistory
    {
        public int Id { get; set; }

        [Required]
        public int AssetId { get; set; }
        public Asset Asset { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public DateTime AssignedDate { get; set; }
        public DateTime? ReturnedDate { get; set; }

        [StringLength(250)]
        public string Notes { get; set; }
    }
}