using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Constants;
using AlbaVulpes.API.Models.Responses;
using AlbaVulpes.API.Modules.Images;
using AlbaVulpes.API.Services;
using AlbaVulpes.API.Services.AWS;
using AlbaVulpes.API.Services.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbaVulpes.API.Controllers
{
    [Route("images")]
    [Produces("application/json")]
    public class ImageController : ApiController
    {
        private readonly IFilesService _filesService;

        private readonly string[] _supportedFileTypes = { "image/bmp", "image/jpg", "image/jpeg", "image/png", "image/gif" };

        public ImageController(IUnitOfWork unitOfWork, IValidatorService validator, IFilesService filesService) : base(unitOfWork, validator)
        {
            _filesService = filesService;
        }

        [Authorize(Roles = "Creator")]
        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("File not provided or unreadable");
            }

            if (!_supportedFileTypes.Contains(file.ContentType))
            {
                return BadRequest($"Content Type {file.ContentType} not supported");
            }

            var imageResponse = await UploadImage(file);

            return Ok(imageResponse);
        }

        [Authorize(Roles = "Creator")]
        [HttpPost("multiple")]
        public async Task<IActionResult> CreateMultiple(List<IFormFile> files)
        {
            if (files == null || files.Count <= 0)
            {
                return BadRequest("Files not provided or unreadable");
            }

            var imageResponses = new List<ImageResponse>();

            foreach (var file in files)
            {
                if (!_supportedFileTypes.Contains(file.ContentType))
                {
                    return BadRequest($"Content Type {file.ContentType} not supported");
                }

                var imageResponse = await UploadImage(file);

                imageResponses.Add(imageResponse);
            }

            return Ok(imageResponses);
        }

        private async Task<ImageResponse> UploadImage(IFormFile file)
        {
            var contentType = file.ContentType;

            using (var imageFileStream = new MemoryStream())
            {
                await file.CopyToAsync(imageFileStream);

                using (var processedImageStream = ImageProcessor.ProcessImage(imageFileStream, contentType))
                {
                    var fileKey = $"{S3StorageOptions.ImageUploadsKeyPrefix}/image-{Guid.NewGuid()}.jpg";

                    var uploadedImageUrl = await _filesService.UploadFileAsync(fileKey, processedImageStream);

                    return new ImageResponse
                    {
                        ImagePath = uploadedImageUrl
                    };
                }
            }
        }
    }
}