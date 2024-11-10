using FluentValidation;
using Reactivities.Domain.Users.Dtos;

namespace Reactivities.Domain.Activities.Dtos.Validators
{
    public class EditProfileDtoValidator : AbstractValidator<EditProfileDto>
    {
        public EditProfileDtoValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}
