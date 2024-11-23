using FluentValidation;

namespace Reactivities.Domain.Account.Commands.Validators
{
    public class RegisterValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.User.Email).NotEmpty();
            RuleFor(x => x.User.UserName).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
