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
using AlbaVulpes.API.Services.Content;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlbaVulpes.API.Controllers
{
    [Route("config")]
    [Produces("application/json")]
    public class ConfigController : ApiController
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISecretsManagerService _secretsManager;

        public ConfigController(IUnitOfWork unitOfWork, IValidatorService validator, IHostingEnvironment hostingEnvironment, ISecretsManagerService secretsManager) : base(unitOfWork, validator)
        {
            _hostingEnvironment = hostingEnvironment;
            _secretsManager = secretsManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetConfig()
        {
            var appSecrets = await _secretsManager.Get();

            return Ok(new ConfigResponse
            {
                Environment = _hostingEnvironment.EnvironmentName,
                External = new ConfigResponseExternal
                {
                    GoogleApiKey = appSecrets.Google_ApiKey,
                    GoogleClientId = appSecrets.Google_ClientId
                }
            });
        }
    }
}