using System.ComponentModel.DataAnnotations;

namespace AutoVault.Models
{
    public class EngineSpecs
    {
        [Range(1, 16, ErrorMessage = "Cylinders must be between 1 and 16")]
        public int Cylinders { get; set; }

        public bool Turbocharged { get; set; }
        public bool Supercharged { get; set; }

        [Range(0.1, 10.0, ErrorMessage = "Displacement must be realistic (0.1 - 10.0)")]
        public double DisplacementLiters { get; set; }

        [Required]
        [StringLength(20)]
        public string FuelType { get; set; } = "Gasoline";
    }
}