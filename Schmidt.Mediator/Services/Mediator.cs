using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Schmidt.Mediator
{
    internal class Mediator : IMediator
    {
        static internal ConcurrentDictionary<Type, Type> RequestsHandlers = new ConcurrentDictionary<Type, Type>();
        static internal ConcurrentDictionary<Type, IEnumerable<Type>> EventsHandlers = new ConcurrentDictionary<Type, IEnumerable<Type>>();
        private readonly IServiceProvider _serviceProvider;

        public Mediator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task SendAsync<TRequest>(TRequest request, CancellationToken cancelationToken = default)
            where TRequest : IRequest
        {
            var typeHandler = RequestsHandlers[request.GetType()];
            var handler = _serviceProvider.GetService(typeHandler);
            var method = handler.GetType().GetMethod(nameof(IRequestHandler<IRequest>.HandleAsync));
            var response = method.Invoke(handler, new object[] { request, cancelationToken }) as Task;
            return response;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancelationToken = default)
            where TEvent : IEvent
        {
            var typeHandler = EventsHandlers[@event.GetType()];
            var events = new List<Task>();
            foreach (var handlerType in typeHandler)
            {
                var handler = _serviceProvider.GetService(handlerType);
                var method = handler.GetType().GetMethod(nameof(IEventHandler<IEvent>.HandleAsync));
                var response = method.Invoke(handler, new object[] { @event, cancelationToken }) as Task;
                events.Add(response);
            }
            await Task.WhenAll(events);
        }

        public Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancelationToken = default)
        {
            var typeHandler = RequestsHandlers[request.GetType()];
            var handler = _serviceProvider.GetRequiredService(typeHandler);
            var method = handler.GetType().GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.HandleAsync));
            var response = method.Invoke(handler, new object[] { request, cancelationToken }) as Task<TResponse>;
            return response;
        }
    }
}
