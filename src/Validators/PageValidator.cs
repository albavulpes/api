using AlbaVulpes.API.Models.Resource;
using FluentValidation;

namespace AlbaVulpes.API.Validators
{
    public class PageValidator : AbstractValidator<Page>
    {
        public PageValidator()
        {
            RuleFor(x => x.ChapterId).NotEmpty();

            RuleFor(x => x.CoverImage)
                .NotNull()
                .SetValidator(new ImageValidator());
        }
    }
}