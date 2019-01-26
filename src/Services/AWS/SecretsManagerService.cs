using System.Threading.Tasks;
using AlbaVulpes.API.Models.Config;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AlbaVulpes.API.Services.AWS
{
    public interface ISecretsManagerService
    {
        Task<AppSecrets> Get();
    }

    public class SecretsManagerService : ISecretsManagerService
    {
        private readonly AppSettings _appSettings;
        private readonly IAmazonClientResolverService _clientResolver;

        private AppSecrets _appSecrets;

        public SecretsManagerService(IOptions<AppSettings> appSettings, IAmazonClientResolverService clientResolver)
        {
            _appSettings = appSettings.Value;
            _clientResolver = clientResolver;
        }

        public async Task<AppSecrets> Get()
        {
            if (_appSecrets != null)
                return _appSecrets;

            using (var client = _clientResolver.GetClient<AmazonSecretsManagerClient>())
            {
                var request = new GetSecretValueRequest
                {
                    SecretId = _appSettings.AppSecretsId
                };

                var response = await client.GetSecretValueAsync(request);

                var secretString = response?.SecretString;

                _appSecrets = JsonConvert.DeserializeObject<AppSecrets>(secretString);

                return _appSecrets;
            }
        }
    }
}