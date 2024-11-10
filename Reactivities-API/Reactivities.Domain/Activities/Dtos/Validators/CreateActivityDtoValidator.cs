using FluentValidation;

namespace Reactivities.Domain.Activities.Dtos.Validators
{
    public class CreateActivityDtoValidator : AbstractValidator<CreateActivityDto>
    {
        public CreateActivityDtoValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Category).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.Venue).NotEmpty();
        }
    }
}
