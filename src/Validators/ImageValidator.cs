using AlbaVulpes.API.Models.Shared;
using FluentValidation;

namespace AlbaVulpes.API.Validators
{
    public class ImageValidator : AbstractValidator<Image>
    {
        public ImageValidator()
        {
            RuleFor(x => x.Thumbnail).NotEmpty();
            RuleFor(x => x.FullSize).NotEmpty();
        }
    }
}