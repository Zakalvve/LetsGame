using FluentValidation;
using LetsGame.Common.Data.Entities.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsGame.Common.Data.FluentValidators
{
    public abstract class BaseValidator<T> : AbstractValidator<T>
    {
        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<T>.CreateWithOptions((T)model, x => x.IncludeProperties(propertyName)));

            if (result.IsValid) return Array.Empty<string>();

            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
