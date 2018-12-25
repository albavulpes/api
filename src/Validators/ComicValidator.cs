using AlbaVulpes.API.Models.Resource;
using FluentValidation;

namespace AlbaVulpes.API.Validators
{
    public class ComicValidator : AbstractValidator<Comic>
    {
        public ComicValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Author).NotEmpty();

            RuleFor(x => x.CoverImage).NotNull().SetValidator(new ImageValidator());
        }
    }
}