using Conduit.Domain.DTOs;
using FluentValidation;

namespace Conduit.Application.Validators
{
    public class UpdateUserValidator : AbstractValidator<UserUpdateDto>
    {
        public UpdateUserValidator()
        {
            RuleFor(update => update.Email).NotNull().NotEmpty()
                .Matches("^[^@\\s]+@[^@\\s]+\\.(com|net|org|gov|co)$");
            RuleFor(update => update.Bio).NotNull();
        }
    }
}
