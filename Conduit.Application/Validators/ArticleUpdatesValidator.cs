using Conduit.Domain.DTOs;
using FluentValidation;

namespace Conduit.Application.Validators
{
    public class ArticleUpdatesValidator : AbstractValidator<ArticleUpdateDto>
    {
        public ArticleUpdatesValidator()
        {
            RuleFor(a => a.Title).NotNull().NotEmpty().MaximumLength(200)
                .WithMessage("Article title must have less than 200 characters");
        }
    }
}
