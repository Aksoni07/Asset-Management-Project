using System.ComponentModel.DataAnnotations;

namespace AssetManagement.Core.Entities
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Department { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Designation { get; set; }
        
        public bool IsActive { get; set; } = true; // Status (Active/Inactive)
        
        public ICollection<AssetAssignmentHistory> AssetAssignmentHistories { get; set; }
    }
}