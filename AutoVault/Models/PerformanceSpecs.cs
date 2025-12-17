using System.ComponentModel.DataAnnotations;

namespace AutoVault.Models
{
    public class PerformanceSpecs
    {
        [Range(1, 3000, ErrorMessage = "Horsepower must be realistic (max 3000)")]
        public int Horsepower { get; set; }

        [Range(0.1, 30, ErrorMessage = "0–60 time must be realistic (0.2 - 30")]
        public double ZeroToSixty { get; set; }

        [Range(30, 400, ErrorMessage = "Top speed must be realistic (30 - 400)")]
        public int TopSpeed { get; set; }

        [Required]
        [StringLength(10)]
        public string Drivetrain { get; set; } = "FWD";
    }
}