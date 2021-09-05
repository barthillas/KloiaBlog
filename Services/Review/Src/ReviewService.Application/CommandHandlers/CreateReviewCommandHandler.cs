using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Handler;
using Data.UnitOfWork;
using MediatR;
using ReviewService.Abstraction.Command;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;
using Simple.OData.Client;

namespace ReviewService.Application.CommandHandlers
{
    public class CreateReviewCommandHandler : ICommandHandler<CreateReviewCommand, Unit>
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;
        private readonly ODataClient _oDataClient;
        

        public CreateReviewCommandHandler(IUnitOfWork<ReviewDbContext> unitOfWork, ODataClient oDataClient)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
        }

        public async Task<Unit> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = new Review();
            var article = await _oDataClient.For("Article").FindEntryAsync(cancellationToken);
                
            if (!article.Any())
            {
                throw new Exception($"Article does not exist. ArticleId: {request.ArticleId}");
            }
            review.Create(request.ArticleId, request.Reviewer,request.ReviewContent);
            await _unitOfWork.GetRepository<Review>().AddAsync(review, cancellationToken).ConfigureAwait(false);
            await _unitOfWork.Complete(); //TODO move to postProcessor
            return Unit.Value;
        }

    }
}