using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Abstraction.Command;

namespace Data.CQRS
{
    public interface ICommandSender
    {
        Task<TResult> SendAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

        Task SendAsync(ICommand command, CancellationToken cancellationToken = default);
    }
}