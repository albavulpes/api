using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Repositories.Resource;
using AlbaVulpes.API.Services;
using FluentValidation;

namespace AlbaVulpes.API.Validators
{
    public class ChapterValidator : AbstractValidator<Chapter>
    {
        public ChapterValidator(IUnitOfWork unitOfWork)
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Author).NotEmpty();

            RuleFor(x => x.ComicId).NotEmpty();

            RuleFor(x => x.ArcId)
                .Custom((arcId, context) =>
                {
                    if (arcId != null)
                    {
                        var arc = unitOfWork.GetRepository<ArcRepository>().Get(arcId.Value).Result;

                        if (arc == null)
                        {
                            context.AddFailure($"Arc with specified ID does not exist.");
                        }
                    }
                });
        }
    }
}