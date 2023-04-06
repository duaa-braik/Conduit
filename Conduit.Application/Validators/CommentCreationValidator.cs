using Conduit.Domain.DTOs;
using FluentValidation;

namespace Conduit.Application.Validators
{
    public class CommentCreationValidator : AbstractValidator<CommentCreationDto>
    {
        public CommentCreationValidator()
        {
            RuleFor(c => c.body).NotEmpty().NotNull().WithMessage("Comment is required");
        }
    }
}
