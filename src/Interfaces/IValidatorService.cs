using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;

namespace AlbaVulpes.API.Interfaces
{
    public interface IValidatorService
    {
        TValidator GetValidator<TValidator>() where TValidator : IValidator;
    }
}