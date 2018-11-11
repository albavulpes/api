using System;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Requests;
using AlbaVulpes.API.Repositories.Identity;
using AlbaVulpes.API.Validators;
using Microsoft.AspNetCore.Mvc;

namespace AlbaVulpes.API.Controllers
{
    [Route("auth")]
    [Produces("application/json")]
    public class AuthController : ApiController
    {
        public AuthController(IUnitOfWork unitOfWork, IValidatorService validator) : base(unitOfWork, validator)
        {
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var validation = ValidatorService.GetValidator<LoginRequestValidator>().Validate(request);

            if (!validation.IsValid)
            {
                var error = validation.Errors.FirstOrDefault();
                return BadRequest(error?.ErrorMessage);
            }

            var authenticatedUser = await UnitOfWork.GetRepository<AuthRepository>().AuthenticateUser(request);

            if (authenticatedUser == null)
            {
                return Unauthorized();
            }

            return Ok();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var validation = ValidatorService.GetValidator<RegisterRequestValidator>().Validate(request);

            if (!validation.IsValid)
            {
                var error = validation.Errors.FirstOrDefault();
                return BadRequest(error?.ErrorMessage);
            }

            var createdUser = await UnitOfWork.GetRepository<AuthRepository>().RegisterNewUser(request);

            if (createdUser == null)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}