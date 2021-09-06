using Abstraction.Validation;
using ArticleService.Abstraction.Command;
using FluentValidation;

namespace ArticleService.Abstraction.Validation
{
    public class DeleteArticleCommandValidator : BaseValidator<DeleteArticleCommand>
    {
        public DeleteArticleCommandValidator()
        {
            RuleFor(x => x.ArticleId).NotEmpty().GreaterThan(0);
        }

    }
}