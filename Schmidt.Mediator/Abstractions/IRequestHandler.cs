using System.Threading;
using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    internal interface IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        Task HandleAsync(TRequest request, CancellationToken cancelationToken);
    }
    internal interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request, CancellationToken cancelationToken);
    }
    internal interface IEventHandler<TRequest>
        where TRequest : IEvent
    {
        Task HandleAsync(TRequest request, CancellationToken cancelationToken);
    }
}
