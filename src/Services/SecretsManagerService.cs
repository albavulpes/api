using System;
using System.Threading.Tasks;
using AlbaVulpes.API.Models.Config;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using AutoMapper.Configuration;
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

        public SecretsManagerService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AppSecrets Get()
        {
            var credentialProfileStoreChain = new CredentialProfileStoreChain();
            credentialProfileStoreChain.TryGetAWSCredentials(_appSettings.AWS.CredentialsProfileName, out var credentials);

            var client = new AmazonSecretsManagerClient(credentials, new AmazonSecretsManagerConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1
            });

            var request = new GetSecretValueRequest
            {
                SecretId = _appSettings.AWS.AppSecretsId
            };

            var response = Task.Run(async () => await client.GetSecretValueAsync(request)).Result;

            var secretString = response?.SecretString;

            return JsonConvert.DeserializeObject<AppSecrets>(secretString);
        }
    }
}