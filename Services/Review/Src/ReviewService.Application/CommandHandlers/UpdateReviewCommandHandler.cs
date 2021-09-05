using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abstraction.Handler;
using ReviewService.Abstraction.Command;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;
using Data.UnitOfWork;
using MediatR;
using Simple.OData.Client;
using static System.DateTime;

namespace ReviewService.Application.CommandHandlers
{
    public class UpdateReviewCommandHandler : ICommandHandler<UpdateReviewCommand, Unit>
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;
        private readonly ODataClient _oDataClient;
        

        public UpdateReviewCommandHandler(IUnitOfWork<ReviewDbContext> unitOfWork, ODataClient oDataClient)
        {
            _unitOfWork = unitOfWork;
            _oDataClient = oDataClient;
        }

        public async Task<Unit> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _unitOfWork.GetRepository<Review>()
                .GetFirstAsync(x => x.ReviewId == request.ReviewId, cancellationToken).ConfigureAwait(false);
            
            if (review == null)
            {
                throw new Exception($"Record does not exist. ReviewId: {request.ReviewId} ");
            }
            review.Update(request.ReviewId, request.Reviewer, request.ReviewContent);
            await _unitOfWork.Complete(); //TODO move to postProcessor
            return Unit.Value;
        }
    }
}