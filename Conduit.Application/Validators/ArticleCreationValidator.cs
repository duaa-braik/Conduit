using Conduit.Domain.DTOs;
using FluentValidation;

namespace Conduit.Application.Validators
{
    public class ArticleCreationValidator : AbstractValidator<ArticleCreationDto>
    {
        public ArticleCreationValidator()
        {
            RuleFor(a => a.Title).NotNull().NotEmpty().WithMessage("Title is required");
            RuleFor(a => a.Text).NotNull().NotEmpty().WithMessage("Article Body is required");
            RuleFor(a => a.Description).NotNull().NotEmpty().WithMessage("Description is required");
        }
    }
}
