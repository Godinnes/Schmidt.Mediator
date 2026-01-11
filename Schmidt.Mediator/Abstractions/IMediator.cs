using System.Threading;
using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    public interface IMediator
    {
        Task SendAsync<TRequest>(TRequest request, CancellationToken cancelationToken)
            where TRequest : IRequest;
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancelationToken);
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancelationToken)
            where TEvent : IEvent;
    }
}
