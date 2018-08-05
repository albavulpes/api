using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace AlbaVulpes.API.Interfaces
{
    public interface IValidatorService
    {
        TValidator GetValidator<TModel, TValidator>() where TValidator : AbstractValidator<TModel>;

        Task<ValidationResult> Validate<TValidator, TModel>(TModel model) where TValidator : AbstractValidator<TModel>;
    }
}