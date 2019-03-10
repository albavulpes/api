using AlbaVulpes.API.Models.Resource;
using FluentValidation;

namespace AlbaVulpes.API.Validators
{
    public class ArcValidator : AbstractValidator<Arc>
    {
        public ArcValidator()
        {
            RuleFor(x => x.Title).NotEmpty();

            RuleFor(x => x.ComicId).NotEmpty();
        }
    }
}