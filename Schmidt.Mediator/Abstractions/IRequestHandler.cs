using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    internal interface IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        Task HandleAsync(TRequest request);
    }
    internal interface IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
    }
    internal interface IEventHandler<TRequest>
        where TRequest : IEvent
    {
        Task HandleAsync(TRequest request);
    }
}
