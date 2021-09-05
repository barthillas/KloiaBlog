using System;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Handler;
using ArticleService.Abstraction.Command;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;
using MediatR;

namespace ArticleService.Application.CommandHandlers
{
    public class CreateArticleCommandHandler : ICommandHandler<CreateArticleCommand, Unit>
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;

        public CreateArticleCommandHandler(IUnitOfWork<ArticleDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(CreateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = new Article();
            DateTime.TryParse(request.PublishDate, out var publishDate);
            article.Create(request.Title, request.Author, request.ArticleContent, publishDate, request.StarCount);
            await _unitOfWork.GetRepository<Article>().AddAsync(article, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.Complete(); //TODO move to postProcessor
            return Unit.Value;
        }
    }
}