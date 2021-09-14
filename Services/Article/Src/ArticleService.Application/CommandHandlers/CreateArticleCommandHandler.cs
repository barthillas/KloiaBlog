using System;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using Abstraction.Handler;
using ArticleService.Abstraction.Command;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;
using static System.DateTime;

namespace ArticleService.Application.CommandHandlers
{
    public class CreateArticleCommandHandler : ICommandHandler<CreateArticleCommand, ArticleDto>
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;

        public CreateArticleCommandHandler(IUnitOfWork<ArticleDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ArticleDto> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = new Article();
            var tryParse = TryParse(request.PublishDate, out var publishDate);
            article.Create(request.Title, request.Author, request.ArticleContent, tryParse ? publishDate : new DateTime(), request.StarCount);
            await _unitOfWork.GetRepository<Article>().AddAsync(article, cancellationToken).ConfigureAwait(false);
            return new ArticleDto{Title = article.Title, Author = article.Author, ArticleContent =  article.ArticleContent, ArticleId = article.ArticleId, PublishDate = article.PublishDate};
        }
    }
}