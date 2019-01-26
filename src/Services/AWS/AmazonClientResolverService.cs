using System;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;

namespace AlbaVulpes.API.Services.AWS
{
    public interface IAmazonClientResolverService
    {
        TClientService GetClient<TClientService>() where TClientService : AmazonServiceClient;
    }

    public class AmazonClientResolverService
    {
        private readonly IConfiguration _config;

        public AmazonClientResolverService(IConfiguration config)
        {
            _config = config;
        }

        public TClientService GetClient<TClientService>() where TClientService : AmazonServiceClient
        {
            var awsOptions = _config.GetAWSOptions();

            var isCredentialsResolved = new CredentialProfileStoreChain().TryGetAWSCredentials(awsOptions.Profile, out var awsCredentials);
            if (isCredentialsResolved)
            {
                return (TClientService)Activator.CreateInstance(typeof(TClientService), awsCredentials, awsOptions.Region);
            }
            else
            {
                return (TClientService)Activator.CreateInstance(typeof(TClientService));
            }
        }
    }
}