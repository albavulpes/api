using AlbaVulpes.API.Services.AWS;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class AmazonServicesExtensions
    {
        public static void AddAmazonServices(this IServiceCollection services)
        {
            services.AddScoped<IAmazonClientResolverService, AmazonClientResolverService>();

            services.AddScoped<ISecretsManagerService, SecretsManagerService>();
            services.AddScoped<IS3UploadService, S3UploadService>();
        }
    }
}