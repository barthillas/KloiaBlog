using MediatR;

namespace Abstraction.Command
{
    public interface ICommand<out TResult> : IRequest<TResult>
    {
    }
}
