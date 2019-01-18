using System.Threading.Tasks;
using AlbaVulpes.API.Models.Config;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _config;
        private readonly AppSettings _appSettings;

        private AppSecrets _appSecrets;

        public SecretsManagerService(IConfiguration config, IOptions<AppSettings> appSettings)
        {
            _config = config;
            _appSettings = appSettings.Value;
        }

        public AppSecrets Get()
        {
            if (_appSecrets != null)
                return _appSecrets;

            var awsOptions = _config.GetAWSOptions();

            var isCredentialsResolved = new CredentialProfileStoreChain().TryGetAWSCredentials(awsOptions.Profile, out var awsCredentials);

            if (!isCredentialsResolved)
            {
                throw new AmazonSecretsManagerException("Failed to resolve credentials.");
            }

            var client = new AmazonSecretsManagerClient(awsCredentials, awsOptions.Region);

            var request = new GetSecretValueRequest
            {
                SecretId = _appSettings.AppSecretsId
            };

            var response = Task.Run(async () => await client.GetSecretValueAsync(request)).Result;

            var secretString = response?.SecretString;

            _appSecrets = JsonConvert.DeserializeObject<AppSecrets>(secretString);

            return _appSecrets;
        }
    }
}