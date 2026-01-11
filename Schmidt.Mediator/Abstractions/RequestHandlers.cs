using System.Threading;
using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    public abstract class AsyncRequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract Task HandleAsync(TRequest request, CancellationToken cancelationToken);
    }

    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract void Handle(TRequest request, CancellationToken cancelationToken);
        public Task HandleAsync(TRequest request, CancellationToken cancelationToken)
        {
            Handle(request, cancelationToken);
            return Task.CompletedTask;
        }
    }

    public abstract class AsyncRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> HandleAsync(TRequest request, CancellationToken cancelationToken);
    }

    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract TResponse Handle(TRequest request, CancellationToken cancellationToken);
        public Task<TResponse> HandleAsync(TRequest request, CancellationToken cancelationToken)
        {
            TResponse response = Handle(request, cancelationToken);
            return Task.FromResult(response);
        }
    }

    public abstract class AsyncEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract Task HandleAsync(TEvent @event, CancellationToken cancelationToken);
    }

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract void Handle(TEvent @event, CancellationToken cancellationToken);
        public Task HandleAsync(TEvent @event, CancellationToken cancelationToken)
        {
            Handle(@event, cancelationToken);
            return Task.CompletedTask;
        }
    }
}
