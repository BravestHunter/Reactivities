using FluentValidation;

namespace Reactivities.Domain.Users.Commands.Validators
{
    public class TogleFollowValidator : AbstractValidator<TogleFollowCommand>
    {
        public TogleFollowValidator()
        {
            RuleFor(x => x.TargetUsername).NotEmpty();
        }
    }
}
