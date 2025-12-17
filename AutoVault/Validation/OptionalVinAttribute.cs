using System.ComponentModel.DataAnnotations;

namespace AutoVault.Validation
{
    public class OptionalVinAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var vin = value as string;

            // Treat null, empty, or "0" as not provided
            if (string.IsNullOrWhiteSpace(vin) || vin == "0")
            {
                return ValidationResult.Success;
            }

            // Enforce exact length if provided
            if (vin.Length != 17)
            {
                return new ValidationResult("VIN must be exactly 17 characters if provided.");
            }

            return ValidationResult.Success;
        }
    }
}