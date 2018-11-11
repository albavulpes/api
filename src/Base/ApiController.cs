using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Marten;
using AlbaVulpes.API.Interfaces;

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
