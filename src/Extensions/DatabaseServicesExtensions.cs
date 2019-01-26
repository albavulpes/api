using System.Threading.Tasks;
using AlbaVulpes.API.Repositories.Identity;
using AlbaVulpes.API.Repositories.Resource;
using AlbaVulpes.API.Services.AWS;
using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;
using IUnitOfWork = AlbaVulpes.API.Services.IUnitOfWork;
using UnitOfWork = AlbaVulpes.API.Services.UnitOfWork;

namespace AlbaVulpes.API.Extensions
{
    public static class DatabaseServicesExtensions
    {
        public static void AddDatabaseServices(this IServiceCollection services)
        {
            services.AddMarten();
            services.AddUnitOfWork();

        }
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

        public static void AddMarten(this IServiceCollection services)
        {
            services.AddScoped<IDocumentStore>(provider =>
            {
                var secretsManager = provider.GetService<ISecretsManagerService>();
                var appSecrets = Task.Run(async () => await secretsManager.Get()).Result;

                return DocumentStore.For(options =>
                {
                    options.Connection($"host={appSecrets.Database_Host};port={appSecrets.Database_Port};database={appSecrets.Database_Name};username={appSecrets.Database_Username};password={appSecrets.Database_Password}");

                    options.Serializer(new JsonNetSerializer
                    {
                        EnumStorage = EnumStorage.AsString
                    });
                });
            });
        }
    }
}