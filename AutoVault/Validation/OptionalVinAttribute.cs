using System.ComponentModel.DataAnnotations;

namespace AutoVault.Validation
{
    public class OptionalVinAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var vin = value as string;

            // if the vin is null, it is validated
            if (string.IsNullOrWhiteSpace(vin) || vin == "0")
            {
                return ValidationResult.Success;
            }

            // if the vin isn't null, it has to be 17 characters
            if (vin.Length != 17)
            {
                return new ValidationResult("VIN must be exactly 17 characters if provided.");
            }

            return ValidationResult.Success;
        }
    }
}