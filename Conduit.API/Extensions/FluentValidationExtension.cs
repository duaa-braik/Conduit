using FluentValidation.AspNetCore;

namespace Conduit.API.Extensions
{
    public static class FluentValidationExtension
    {
        public static IServiceCollection AddFluentValidation (this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            return services;
        }
    }
}
