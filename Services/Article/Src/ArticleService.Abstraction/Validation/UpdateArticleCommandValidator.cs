using Abstraction.Validation;
using ArticleService.Abstraction.Command;
using FluentValidation;

namespace ArticleService.Abstraction.Validation
{
    public class UpdateArticleCommandValidator : BaseValidator<UpdateArticleCommand>
    {
        public UpdateArticleCommandValidator()
        {
            RuleFor(x => x.ArticleId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.ArticleContent).NotEmpty();
            RuleFor(x => x.Author).NotEmpty();
            RuleFor(x => x.PublishDate).NotNull().NotEmpty();
            RuleFor(x => x.StarCount).NotEmpty();
        }

    }
}