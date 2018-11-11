using System;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
using AlbaVulpes.API.Models.Requests;
using FluentValidation;
using FluentValidation.Results;
using Marten;

namespace AlbaVulpes.API.Services
{
    public class ValidatorService : IValidatorService
    {
        public TValidator GetValidator<TValidator>() where TValidator : IValidator
        {
            return (TValidator)Activator.CreateInstance(typeof(TValidator));
        }
    }
}