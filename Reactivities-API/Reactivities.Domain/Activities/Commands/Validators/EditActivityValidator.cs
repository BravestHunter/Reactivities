using FluentValidation;
using Reactivities.Domain.Activities.Dtos.Validators;

namespace Reactivities.Domain.Activities.Commands.Validators
{
    internal class EditActivityValidator : AbstractValidator<EditActivityCommand>
    {
        public EditActivityValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new EditActivityDtoValidator());
        }
    }
}
