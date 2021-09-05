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
using static System.DateTime;

namespace ArticleService.Application.CommandHandlers
{
    public class UpdateArticleCommandHandler : ICommandHandler<UpdateArticleCommand, Unit>
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;
        private readonly ODataClient _oDataClient;
        

        public UpdateArticleCommandHandler(IUnitOfWork<ArticleDbContext> unitOfWork, ODataClient oDataClient)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
        }

        public async Task<Unit> Handle(UpdateArticleCommand request, CancellationToken cancellationToken)
        {
            var article = await _unitOfWork.GetRepository<Article>()
                .GetFirstAsync(x => x.ArticleId == request.ArticleId, cancellationToken).ConfigureAwait(false);
            
            if (article == null)
            {
                throw new Exception($"Record does not exist. ArticleId: {request.ArticleId} ");
            }

            var tryParse = TryParse(request.PublishDate, out var publishDate);
            article.Update(request.Title, request.Author, request.ArticleContent, tryParse ? publishDate : new DateTime(), request.StarCount);
            await _unitOfWork.Complete(); //TODO move to postProcessor
            return Unit.Value;
        }
    }
}