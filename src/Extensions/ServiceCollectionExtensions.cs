using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories;
using Marten;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddMarten(this IServiceCollection services)
        {
            services.AddScoped<IDocumentStore>(provider =>
                DocumentStore.For(options =>
                {
                    options.Connection("host=localhost;port=5432;database=AlbaVulpes;username=albavulpes;password=asdfghjkl");
                })
            );
        }

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}