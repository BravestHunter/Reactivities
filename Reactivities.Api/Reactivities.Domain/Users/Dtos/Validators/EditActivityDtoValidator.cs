using FluentValidation;
using Reactivities.Domain.Users.Dtos;

namespace Reactivities.Domain.Activities.Dtos.Validators
{
    internal class EditProfileDtoValidator : AbstractValidator<EditProfileDto>
    {
        public EditProfileDtoValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}
