using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using AlbaVulpes.API.Services.AWS;
using Amazon.S3;
using Amazon.S3.Model;

namespace AlbaVulpes.API.Services.Content
{
    public interface IFilesService
    {
        Task<Stream> GetFileAsync(string fullSizeImagePath);
        Task<string> UploadFileAsync(string key, Stream fileStream);
        Task<string> DeleteFileAsync(string key);
    }

    public class FilesService : IFilesService
    {
        private readonly ISecretsManagerService _secretsManager;
        private readonly IAmazonClientResolverService _clientResolver;

        public FilesService(ISecretsManagerService secretsManager, IAmazonClientResolverService clientResolver)
        {
            _secretsManager = secretsManager;
            _clientResolver = clientResolver;
        }

        public async Task<Stream> GetFileAsync(string filePath)
        {
            using (var httpClient = new HttpClient())
            {
                var fileStream = await httpClient.GetStreamAsync(filePath);

                return fileStream;
            }
        }

        public async Task<string> UploadFileAsync(string key, Stream fileStream)
        {
            var appSecrets = await _secretsManager.Get();
            var bucketName = appSecrets.AWS_S3BucketName;

            using (var s3Client = _clientResolver.GetClient<AmazonS3Client>())
            {
                await s3Client.PutObjectAsync(new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = key,
                    InputStream = fileStream
                });

                return $"https://{bucketName}.s3.amazonaws.com/{key}";
            }
        }

        public async Task<string> DeleteFileAsync(string key)
        {
            var appSecrets = await _secretsManager.Get();
            var bucketName = appSecrets.AWS_S3BucketName;

            using (var s3Client = _clientResolver.GetClient<AmazonS3Client>())
            {
                await s3Client.DeleteObjectAsync(new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                });

                return key;
            }
        }
    }
}