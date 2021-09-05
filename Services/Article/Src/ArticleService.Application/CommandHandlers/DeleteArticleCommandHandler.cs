using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Handler;
using ArticleService.Abstraction.Command;
using ArticleService.Domain.Entities;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;
using MediatR;
using Simple.OData.Client;

namespace ArticleService.Application.CommandHandlers
{
    public class DeleteArticleCommandHandler : ICommandHandler<DeleteArticleCommand, Unit>
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;
        private readonly ODataClient _oDataClient;
        

        public DeleteArticleCommandHandler(IUnitOfWork<ArticleDbContext> unitOfWork, ODataClient oDataClient)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
        }

        public async Task<Unit> Handle(DeleteArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _unitOfWork.GetRepository<Article>()
                .GetFirstAsync(x => x.ArticleId == request.ArticleId, cancellationToken).ConfigureAwait(false);
            if (article == null)
            {
                throw new Exception($"Record does not exist. ArticleId: {request.ArticleId} ");
            }

            var reviews = await _oDataClient.For("Review")
                .FindEntriesAsync(new ODataFeedAnnotations(), cancellationToken).ConfigureAwait(false);
            
            if (reviews.Any(x => (int)x["ArticleId"] == article.ArticleId) )
            {
                throw new Exception("It is not possible to delete an article which has reviews");
            }

            _unitOfWork.GetRepository<Article>().Remove(article);
            await _unitOfWork.Complete(); //TODO move to postProcessor
            return Unit.Value;
        }
    }
}