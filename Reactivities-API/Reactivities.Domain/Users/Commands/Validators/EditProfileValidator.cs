using FluentValidation;
using Reactivities.Domain.Activities.Dtos.Validators;

namespace Reactivities.Domain.Users.Commands.Validators
{
    internal class EditProfileValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.Profile).SetValidator(new EditProfileDtoValidator());
        }
    }
}
