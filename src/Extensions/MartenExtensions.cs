using AlbaVulpes.API.Services;
using Marten;
using Marten.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AlbaVulpes.API.Extensions
{
    public static class MartenExtensions
    {
        public static void AddMarten(this IServiceCollection services)
        {
            services.AddScoped<IDocumentStore>(provider =>
            {
                var secretsManager = provider.GetService<ISecretsManagerService>();
                var appSecrets = secretsManager.Get();

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