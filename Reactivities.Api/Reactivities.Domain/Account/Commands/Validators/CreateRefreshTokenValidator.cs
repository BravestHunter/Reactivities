using FluentValidation;

namespace Reactivities.Domain.Account.Commands.Validators
{
    public class CreateRefreshTokenValidator : AbstractValidator<CreateRefreshTokenCommand>
    {
        public CreateRefreshTokenValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.RefreshTokenLifetime).GreaterThan(TimeSpan.Zero);
        }
    }
}
