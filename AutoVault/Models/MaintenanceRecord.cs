using System.ComponentModel.DataAnnotations;

namespace AutoVault.Models
{
    public class MaintenanceRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(200)]
        public string Description { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
        public int MileageAtService { get; set; }

        [Range(0, 100_000, ErrorMessage = "Cost must be realistic (max 100,000)")]
        public decimal Cost { get; set; }
    }
}