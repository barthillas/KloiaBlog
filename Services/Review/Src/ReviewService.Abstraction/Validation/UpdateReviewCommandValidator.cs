using Abstraction.Validation;
using FluentValidation;
using ReviewService.Abstraction.Command;

namespace ReviewService.Abstraction.Validation
{
    public class UpdateReviewCommandValidator : BaseValidator<UpdateReviewCommand>
    {
        public UpdateReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
            RuleFor(x => x.Reviewer).NotNull().NotEmpty();
            RuleFor(x => x.ReviewContent).NotEmpty();
        }

    }
}