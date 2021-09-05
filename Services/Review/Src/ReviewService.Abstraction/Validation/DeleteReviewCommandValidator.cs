using Abstraction.Validation;
using FluentValidation;
using ReviewService.Abstraction.Command;

namespace ReviewService.Abstraction.Validation
{
    public class DeleteReviewCommandValidator : BaseValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId).NotEmpty();
        }

    }
}