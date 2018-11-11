﻿using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Requests;
using AlbaVulpes.API.Repositories.Identity;
using AlbaVulpes.API.Services;
using AlbaVulpes.API.Validators;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

            var claimsPrincipal = await UnitOfWork.GetRepository<AuthRepository>().AuthenticateUser(request);

            if (claimsPrincipal == null)
            {
                return Unauthorized();
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

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

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();

            return Ok();
        }
    }
}