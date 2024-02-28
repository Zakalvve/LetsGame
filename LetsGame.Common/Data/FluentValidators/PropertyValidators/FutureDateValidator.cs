using FluentValidation;
using FluentValidation.Validators;

namespace LetsGame.Common.Data.FluentValidators.PropertyValidators;

internal class FutureDateValidator<T> : PropertyValidator<T, DateTime?>
{
    public override string Name => "FutureDateValidator";

    public override bool IsValid(ValidationContext<T> context, DateTime? value)
    {
        if (value != null && value < DateTime.Now.Date)
        {
            return false;
        }

        return true;
    }

    protected override string GetDefaultMessageTemplate(string errorCode) =>
        "Date must be in the future";
}
