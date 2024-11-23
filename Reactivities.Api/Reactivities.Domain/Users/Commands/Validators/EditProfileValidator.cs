using FluentValidation;
using Reactivities.Domain.Activities.Dtos.Validators;

namespace Reactivities.Domain.Users.Commands.Validators
{
    public class EditProfileValidator : AbstractValidator<EditProfileCommand>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.Profile).SetValidator(new EditProfileDtoValidator());
        }
    }
}
