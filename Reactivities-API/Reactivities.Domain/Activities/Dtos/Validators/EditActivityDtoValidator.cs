using FluentValidation;

namespace Reactivities.Domain.Activities.Dtos.Validators
{
    internal class EditActivityDtoValidator : AbstractValidator<EditActivityDto>
    {
        public EditActivityDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty();
        }
    }
}
