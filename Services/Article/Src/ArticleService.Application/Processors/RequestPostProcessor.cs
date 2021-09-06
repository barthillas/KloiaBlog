using System.Threading;
using System.Threading.Tasks;
using Abstraction.Command;
using ArticleService.Infrastructure.Context;
using Data.UnitOfWork;
using MediatR.Pipeline;

namespace ArticleService.Application.Processors
{
    public class RequestPostProcessor<TCommand, TResponse> : IRequestPostProcessor<TCommand, TResponse> where TCommand : ICommand<TResponse>
    {
        private readonly IUnitOfWork<ArticleDbContext> _unitOfWork;

        public RequestPostProcessor(IUnitOfWork<ArticleDbContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Process(TCommand request, TResponse response, CancellationToken cancellationToken)
        {
            await _unitOfWork.Complete();
        }
    }
}