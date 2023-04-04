using Conduit.Domain.DTOs;
using FluentValidation;

namespace Conduit.Application.Validators
{
    public class UserValidator : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email).NotEmpty().NotNull()
                .Matches("^[^@\\s]+@[^@\\s]+\\.(com|net|org|gov|co)$");
            RuleFor(user => user.Password).NotEmpty().NotNull().Length(6);
            RuleFor(user => user.UserName).NotEmpty().NotNull();
        }
    }
}
