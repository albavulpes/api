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
        public TValidator GetValidator<TModel, TValidator>() where TValidator : AbstractValidator<TModel>
        {
            return (TValidator)Activator.CreateInstance(typeof(TValidator));
        }

        public async Task<ValidationResult> Validate<TValidator, TModel>(TModel model) where TValidator : AbstractValidator<TModel>
        {
            var validator = GetValidator<TModel, TValidator>();

            var results = await validator.ValidateAsync(model);

            return results;
        }
    }
}