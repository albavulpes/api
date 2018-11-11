﻿using System;
using FluentValidation;

namespace AlbaVulpes.API.Services
{
    public interface IValidatorService
    {
        TValidator GetValidator<TValidator>() where TValidator : IValidator;
    }

    public class ValidatorService : IValidatorService
    {
        public TValidator GetValidator<TValidator>() where TValidator : IValidator
        {
            return (TValidator)Activator.CreateInstance(typeof(TValidator));
        }
    }
}