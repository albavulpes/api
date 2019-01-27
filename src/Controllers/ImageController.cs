using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Constants;
using AlbaVulpes.API.Models.Responses;
using AlbaVulpes.API.Modules.Images;
using AlbaVulpes.API.Services;
using AlbaVulpes.API.Services.AWS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbaVulpes.API.Controllers
{
    [Route("images")]
    [Produces("application/json")]
    public class ImageController : ApiController
    {
        private readonly IS3UploadService _s3UploadService;

        public ImageController(IUnitOfWork unitOfWork, IValidatorService validator, IS3UploadService s3UploadService) : base(unitOfWork, validator)
        {
            _s3UploadService = s3UploadService;
        }

        [Authorize(Roles = "Creator")]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, bool createThumbnail = false)
        {
            if (file == null)
            {
                return BadRequest("File not provided or unreadable");
            }

            var contentType = file.ContentType;
            var supportedFileTypes = new[] { "image/bmp", "image/jpg", "image/jpeg", "image/png", "image/gif" };

            if (!supportedFileTypes.Contains(contentType))
            {
                return BadRequest($"Content Type {contentType} not supported");
            }

            string originalImageUrl = null;
            string thumbnailImageUrl = null;

            using (var imageFileStream = new MemoryStream())
            {
                await file.CopyToAsync(imageFileStream);

                using (var processedImageStream = ImageProcessor.ProcessImage(imageFileStream, contentType))
                {
                    var fileKey = $"{S3StorageOptions.ImageUploadsKeyPrefix}/image-{Guid.NewGuid()}.jpg";

                    originalImageUrl = await _s3UploadService.UploadFileAsync(fileKey, processedImageStream);

                    if (createThumbnail)
                    {
                        using (var thumbnailImageStream = ImageProcessor.CreateThumbnail(processedImageStream, contentType))
                        {
                            var keyWithoutExt = Path.GetFileNameWithoutExtension(fileKey);
                            var thumbnailFileKey = fileKey.Replace(keyWithoutExt, keyWithoutExt + "-thumbnail");

                            thumbnailImageUrl = await _s3UploadService.UploadFileAsync(thumbnailFileKey, thumbnailImageStream);
                        }
                    }
                }
            }

            return Ok(new ImageResponse
            {
                ImagePath = originalImageUrl,
                ThumbnailPath = thumbnailImageUrl
            });
        }
    }
}