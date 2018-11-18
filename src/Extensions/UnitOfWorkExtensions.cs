using AlbaVulpes.API.Repositories.Identity;
using AlbaVulpes.API.Repositories.Resource;
using AlbaVulpes.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class UnitOfWorkExtensions
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            // base unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // resources
            services.AddScoped<ComicRepository>();
            services.AddScoped<ArcRepository>();
            services.AddScoped<ChapterRepository>();
            services.AddScoped<PageRepository>();
            services.AddScoped<AuthRepository>();
        }
    }
}