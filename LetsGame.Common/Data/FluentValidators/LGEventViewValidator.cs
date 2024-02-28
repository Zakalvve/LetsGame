using FluentValidation;
using LetsGame.Common.Data.DataAnnotations;
using LetsGame.Common.Data.Entities.Events;
using LetsGame.Common.Data.Fluent_Validators.PropertyValidators;
using LetsGame.Common.Data.FluentValidators.PropertyValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LetsGame.Common.Data.FluentValidators
{
    public class LGEventViewValidator : BaseValidator<LGEventView>
    {
        public LGEventViewValidator()
        {
            // Title
            RuleFor(ev => ev.Title)
                .NotEmpty()
                .MaximumLength(255);

            // Description
            RuleFor(ev => ev.Description)
                .MaximumLength(3000);


            // Start date
            RuleFor(ev => ev.StartDate)
                .SetValidator(new FutureDateValidator<LGEventView>())
                .SetValidator(new BeforeEndDateValidator<LGEventView>("EndDate", true));

            // End date
            RuleFor(ev => ev.EndDate)
                .SetValidator(new FutureDateValidator<LGEventView>())
                .SetValidator(new AfterStartDateValidator<LGEventView>("StartDate", true));
        }

        //public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        //{
        //    var result = await ValidateAsync(ValidationContext<LGEventView>.CreateWithOptions((LGEventView)model, x => x.IncludeProperties(propertyName)));

        //    if (result.IsValid) return Array.Empty<string>();

        //    return result.Errors.Select(e => e.ErrorMessage);
        //};

        public class DetailsValidator : AbstractValidator<LGEventView>
        {
            public DetailsValidator()
            {
                // Title
                RuleFor(ev => ev.Title)
                    .NotEmpty()
                    .MaximumLength(255);

                // Description
                RuleFor(ev => ev.Description)
                    .MaximumLength(3000);
            }
        }

        public class DatesValidator : AbstractValidator<LGEventView>
        {
            public DatesValidator()
            {
                // Start date
                RuleFor(ev => ev.StartDate)
                    .SetValidator(new FutureDateValidator<LGEventView>())
                    .SetValidator(new BeforeEndDateValidator<LGEventView>("EndDate", true));

                // End date
                RuleFor(ev => ev.EndDate)
                    .SetValidator(new FutureDateValidator<LGEventView>())
                    .SetValidator(new AfterStartDateValidator<LGEventView>("StartDate", true));
            }
        }
    }
}
