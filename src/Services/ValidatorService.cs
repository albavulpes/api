using System;
using System.Threading.Tasks;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Interfaces;
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

        public async Task<ValidationResult> Validate<TValidator>(object model) where TValidator : IValidator
        {
            var validator = GetValidator<TValidator>();

            var results = await validator.ValidateAsync(model);

            return results;
        }
    }
}