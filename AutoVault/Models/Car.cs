using AutoVault.Validation;
using System.ComponentModel.DataAnnotations;

namespace AutoVault.Models
{
    public class Car
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Make is required")]
        [StringLength(50)]
        public string Make { get; set; } = string.Empty;

        [Required(ErrorMessage = "Model is required")]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1886, 2100, ErrorMessage = "Year must be between 1886 and 2026")]
        public int Year { get; set; }

        [Required]
        [StringLength(30)]
        public string Color { get; set; } = string.Empty;

        [StringLength(30)]
        public string Trim { get; set; } = string.Empty;

        [OptionalVin]
        public string Vin { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = "Mileage cannot be negative")]
        public int Mileage { get; set; }

        [Range(0, 50_000_000, ErrorMessage = "Price must be positive (max 50,000,000)")]
        public decimal Price { get; set; }

        [Range(0, 2_000, ErrorMessage = "Torque must be realistic (0-2000)")]
        public int Torque { get; set; }

        [Range(0, 200, ErrorMessage = "MPG must be realistic (0-200)")]
        public double Mpg { get; set; }

        [Required]
        [StringLength(20)]
        public string Transmission { get; set; } = "Automatic";

        public bool HasCarbonFiber { get; set; }

        [Url(ErrorMessage = "Image path must be a valid URL")]
        public string ImagePath { get; set; } = string.Empty;

        [Required]
        public EngineSpecs Engine { get; set; } = new();

        [Required]
        public PerformanceSpecs Performance { get; set; } = new();

        public List<MaintenanceRecord> Maintenance { get; set; } = new();
    }
}