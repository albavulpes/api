using AlbaVulpes.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}