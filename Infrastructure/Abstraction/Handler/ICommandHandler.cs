using Abstraction.Command;
using MediatR;

namespace Abstraction.Handler
{
    public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
    }
}
