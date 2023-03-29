using Conduit.Application.Interfaces;
using Conduit.Application.Services;
using Conduit.Infrastructure.Repositories;
using Conduit.Domain.Interfaces;
using Conduit.Infrastructure;
using Microsoft.EntityFrameworkCore;
using EntityFramework.Exceptions.SqlServer;

namespace Conduit.API.Extensions
{
    public static class ApplicationServices
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            return services;
        }

        public static IServiceCollection AddDbContext
            (this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddDbContext<ConduitDbContext>(
                options => options.UseSqlServer(
                    builder.Configuration["ConnectionString"])
                .UseExceptionProcessor());
            return services;

        }
    } 
}
