using AlbaVulpes.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class SecretsManagerExtensions
    {
        public static void AddSecretsManager(this IServiceCollection services)
        {
            services.AddScoped<ISecretsManagerService, SecretsManagerService>();
        }
    }
}