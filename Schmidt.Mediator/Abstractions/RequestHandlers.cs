using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    public abstract class AsyncRequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract Task HandleAsync(TRequest request);
    }

    public abstract class RequestHandler<TRequest> : IRequestHandler<TRequest>
        where TRequest : IRequest
    {
        public abstract void Handle(TRequest request);
        public Task HandleAsync(TRequest request)
        {
            Handle(request);
            return Task.CompletedTask;
        }
    }

    public abstract class AsyncRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract Task<TResponse> HandleAsync(TRequest request);
    }

    public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        public abstract TResponse Handle(TRequest request);
        public Task<TResponse> HandleAsync(TRequest request)
        {
            TResponse response = Handle(request);
            return Task.FromResult(response);
        }
    }

    public abstract class AsyncEventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract Task HandleAsync(TEvent @event);
    }

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract void Handle(TEvent @event);
        public Task HandleAsync(TEvent @event)
        {
            Handle(@event);
            return Task.CompletedTask;
        }
    }
}
