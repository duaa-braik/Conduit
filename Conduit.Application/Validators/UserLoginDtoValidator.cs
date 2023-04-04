using Conduit.Domain.DTOs;
using FluentValidation;

namespace Conduit.Application.Validators
{
    public class UserLoginDtoValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginDtoValidator()
        {
            RuleFor(user => user.Email).NotEmpty().NotNull();
        }
    }
}
