using FluentValidation;
using Reactivities.Domain.Activities.Dtos.Validators;

namespace Reactivities.Domain.Activities.Commands.Validators
{
    internal class CreateActivityValidator : AbstractValidator<CreateActivityCommand>
    {
        public CreateActivityValidator()
        {
            RuleFor(x => x.Activity).SetValidator(new CreateActivityDtoValidator());
        }
    }
}
