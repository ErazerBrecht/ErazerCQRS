using MediatR;

namespace Erazer.Messages.Commands
{
    public interface ICommand : IRequest
    {
    }

    public interface ICommand<out T> : IRequest<T>
    {
    }
}