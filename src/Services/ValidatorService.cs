using System;
using FluentValidation;

namespace AlbaVulpes.API.Services
{
    public interface IValidatorService
    {
        TValidator GetValidator<TValidator>() where TValidator : IValidator;
    }

    public class ValidatorService : IValidatorService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidatorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public TValidator GetValidator<TValidator>() where TValidator : IValidator
        {
            return (TValidator)_serviceProvider.GetService(typeof(TValidator));
        }
    }
}