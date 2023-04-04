using Conduit.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Conduit.API.Extensions
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection AddFluentValidation (this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssemblyContaining<UserValidator>();
            services.AddValidatorsFromAssemblyContaining<UserLoginDtoValidator>();
            services.AddValidatorsFromAssemblyContaining<UpdateUserValidator>();
            return services;
        }
    }
}
