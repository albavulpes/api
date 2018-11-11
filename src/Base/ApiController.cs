using AlbaVulpes.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AlbaVulpes.API.Base
{
    public abstract class ApiController : Controller
    { 
        protected readonly IUnitOfWork UnitOfWork;
        protected readonly IValidatorService ValidatorService;

        protected ApiController(IUnitOfWork unitOfWork, IValidatorService validator)
        {
            UnitOfWork = unitOfWork;
            ValidatorService = validator;
        }
    }
}
