using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Dto;
using Abstraction.Exceptions;
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
                throw new BusinessException($"Record does not exist. ArticleId: {request.ArticleId} ");
            }

            try
            {
                var reviewsResponse = await _oDataClient
                    .For<ReviewDto>("Review")
                    .Filter(x => x.ArticleId == request.ArticleId).FindEntriesAsync(cancellationToken);

                if (reviewsResponse.Any(x => x.ArticleId == article.ArticleId))
                {
                    throw new BusinessException("It is not possible to delete an article which has reviews");
                }
            }
            catch (BusinessException e)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new BusinessException("Could not find Review Microservice", e);
            }

            _unitOfWork.GetRepository<Article>().Remove(article);
            return Unit.Value;
        }
    }
}