using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Services;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class ValidatorExtensions
    {
        public static void AddValidator(this IServiceCollection services)
        {
            services.AddScoped<IValidatorService, ValidatorService>();
        }
    }
}