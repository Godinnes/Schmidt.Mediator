using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    public interface IMediator
    {
        Task SendAsync<TRequest>(TRequest request)
            where TRequest : IRequest;
        Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request);
        Task PublishAsync<TEvent>(TEvent @event)
            where TEvent : IEvent;
    }
}
