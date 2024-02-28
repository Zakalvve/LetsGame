using FluentValidation;
using FluentValidation.Validators;
using System.Reflection;

namespace LetsGame.Common.Data.Fluent_Validators.PropertyValidators;

internal class AfterStartDateValidator<T> : PropertyValidator<T, DateTime?>
{
    private readonly string _startDatePropertyName;
    private readonly bool _allowEqualDates;

    public AfterStartDateValidator(string startDatePropertyName, bool allowEqualDates)
    {
        _startDatePropertyName = startDatePropertyName;
        _allowEqualDates = allowEqualDates;
    }

    public override string Name => "AfterStartDateValidator";

    public override bool IsValid(ValidationContext<T> context, DateTime? value)
    {
        if (value is null) return true;

        context.MessageFormatter.AppendArgument("StartDatePropertyName", _startDatePropertyName);

        PropertyInfo? propertyInfo = context
            .InstanceToValidate?
            .GetType()
            .GetProperty(_startDatePropertyName);

        if (propertyInfo is null)
        {
            return false;
        }

        object? propertyValue = propertyInfo.GetValue(context.InstanceToValidate, null);

        if (propertyValue is null || !(propertyValue is DateTime))
        {
            return true;
        }

        DateTime startDate = (DateTime)propertyValue;
        DateTime endDate = (DateTime)value;

        if (startDate <= endDate)
        {
            if (_allowEqualDates) return true;
            else if (startDate < endDate) return true;
        }

        return false;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "{PropertyName} must occur after {StartDatePropertyName}";
}
