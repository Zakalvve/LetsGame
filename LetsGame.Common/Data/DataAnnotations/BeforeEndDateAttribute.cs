using System.ComponentModel.DataAnnotations;
using System.Reflection;
using LetsGame.Common.Data.Entities.Events;

namespace LetsGame.Common.Data.DataAnnotations
{
    internal class BeforeEndDateAttribute : ValidationAttribute
    {
        public string EndDatePropertyName { get; set; } = String.Empty;
        public bool AllowEqualDates { get; set; } = false;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {

            if (value is null || !(value is DateTime)) return ValidationResult.Success;

            PropertyInfo? propertyInfo =
                validationContext
                .ObjectType
                .GetProperty(EndDatePropertyName);

            if (propertyInfo is null)
            {
                return new ValidationResult($"Could not find property with name {EndDatePropertyName}.");
            }


            object? propertyValue = propertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (propertyValue is null || !(propertyValue is DateTime))
            {
                return ValidationResult.Success;
            }

            DateTime startDate = (DateTime)value;
            DateTime endDate = (DateTime)propertyValue;

            if (startDate <= endDate)
            {
                if (AllowEqualDates) return ValidationResult.Success;
                else if (startDate < endDate) return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
