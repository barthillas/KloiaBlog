using Abstraction.Validation;
using ReviewService.Abstraction.Command;
using FluentValidation;

namespace ReviewService.Abstraction.Validation
{
    public class CreateReviewCommandValidator : BaseValidator<CreateReviewCommand>
    {
        public CreateReviewCommandValidator()
        {
            RuleFor(x => x.ArticleId).NotEmpty();
            RuleFor(x => x.Reviewer).NotEmpty();
            RuleFor(x => x.ReviewContent).NotEmpty();
        }
    }
}