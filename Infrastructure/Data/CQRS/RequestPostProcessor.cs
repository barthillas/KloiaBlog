using System.Threading;
using System.Threading.Tasks;
using Abstraction.Command;
using Data.Context;
using Data.UnitOfWork;
using MediatR.Pipeline;

namespace Data.CQRS
{
    public class RequestPostProcessor<TCommand, TResponse> : IRequestPostProcessor<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        private readonly IUnitOfWork<DbContextBase> _unitOfWork;

        public RequestPostProcessor(IUnitOfWork<DbContextBase> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Process(TCommand request, TResponse response, CancellationToken cancellationToken)
        {
            await _unitOfWork.Complete();
        }
    }
}
