using System.Threading.Tasks;
using AlbaVulpes.API.Models.Config;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AlbaVulpes.API.Services
{
    public interface ISecretsManagerService
    {
        AppSecrets Get();
    }

    public class SecretsManagerService : ISecretsManagerService
    {
        private readonly AppSettings _appSettings;

        private AppSecrets _appSecrets;

        public SecretsManagerService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AppSecrets Get()
        {
            if (_appSecrets != null)
                return _appSecrets;

            var client = new AmazonSecretsManagerClient(new AmazonSecretsManagerConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1
            });

            var request = new GetSecretValueRequest
            {
                SecretId = _appSettings.AWS.AppSecretsId
            };

            var response = Task.Run(async () => await client.GetSecretValueAsync(request)).Result;

            var secretString = response?.SecretString;

            _appSecrets = JsonConvert.DeserializeObject<AppSecrets>(secretString);

            return _appSecrets;
        }
    }
}