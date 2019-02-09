using AlbaVulpes.API.Models.Resource;
using FluentValidation;

namespace AlbaVulpes.API.Validators
{
    public class ChapterValidator : AbstractValidator<Chapter>
    {
        public ChapterValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Author).NotEmpty();

            RuleFor(x => x.ComicId).NotEmpty();
        }
    }
}