using AlbaVulpes.API.Services;
using AlbaVulpes.API.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class ValidatorExtensions
    {
        public static void AddValidator(this IServiceCollection services)
        {
            services.AddScoped<IValidatorService, ValidatorService>();

            // validators
            services.AddScoped<ComicValidator>();
            services.AddScoped<ChapterValidator>();
            services.AddScoped<ArcValidator>();
            services.AddScoped<PageValidator>();
            services.AddScoped<ImageValidator>();

            services.AddScoped<LoginRequestValidator>();
            services.AddScoped<RegisterRequestValidator>();
        }
    }
}