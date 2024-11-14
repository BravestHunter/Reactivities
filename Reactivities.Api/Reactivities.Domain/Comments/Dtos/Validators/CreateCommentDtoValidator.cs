using FluentValidation;

namespace Reactivities.Domain.Comments.Dtos.Validators
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Body).NotEmpty();
        }
    }
}
