using System.Threading;
using System.Threading.Tasks;
using Abstraction.Command;
using Data.UnitOfWork;
using MediatR.Pipeline;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Context;

namespace ReviewService.Application.Processors
{
    public class RequestPostProcessor<TCommand, TResponse> : IRequestPostProcessor<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        private readonly IUnitOfWork<ReviewDbContext> _unitOfWork;

        public RequestPostProcessor(IUnitOfWork<ReviewDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task Process(TCommand request, TResponse response, CancellationToken cancellationToken)
        {
            await _unitOfWork.Complete();
        }
    }
}