using FluentValidation;
using FluentValidation.Validators;
using System.Reflection;

namespace LetsGame.Common.Data.Fluent_Validators.PropertyValidators;

internal class BeforeEndDateValidator<T> : PropertyValidator<T, DateTime?>
{
    private readonly string _endDatePropertyName;
    private readonly bool _allowEqualDates;

    public BeforeEndDateValidator(string startDatePropertyName, bool allowEqualDates)
    {
        _endDatePropertyName = startDatePropertyName;
        _allowEqualDates = allowEqualDates;
    }

    public override string Name => "BeforeEndDateValidator";

    public override bool IsValid(ValidationContext<T> context, DateTime? value)
    {
        if (value is null) return true;

        context.MessageFormatter.AppendArgument("EndDatePropertyName", _endDatePropertyName);

        PropertyInfo? propertyInfo = context
            .InstanceToValidate?
            .GetType()
            .GetProperty(_endDatePropertyName);

        if (propertyInfo is null)
        {
            return false;
        }

        object? propertyValue = propertyInfo.GetValue(context.InstanceToValidate, null);

        if (propertyValue is null || !(propertyValue is DateTime))
        {
            return true;
        }

        DateTime startDate = (DateTime)value;
        DateTime endDate = (DateTime)propertyValue;

        if (startDate <= endDate)
        {
            if (_allowEqualDates) return true;
            else if (startDate < endDate) return true;
        }

        return false;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "{PropertyName} must occur before {EndDatePropertyName}";
}
