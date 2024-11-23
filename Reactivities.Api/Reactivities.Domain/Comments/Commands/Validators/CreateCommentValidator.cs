using FluentValidation;
using Reactivities.Domain.Comments.Dtos.Validators;

namespace Reactivities.Domain.Comments.Commands.Validators
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Comment).SetValidator(new CreateCommentDtoValidator());
        }
    }
}
