using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Linq;
using System.Reflection;

namespace Schmidt.Mediator
{
    public static class MediatorExtensions
    {
        public static IServiceCollection AddMediator(this IServiceCollection services, params Type[] types)
        {
            var assembliesToScan = types.Select(t => t.Assembly).Distinct().ToArray();

            services.AddSingleton<IMediator, Mediator>();

            AddRequestHandlers(services, typeof(IRequestHandler<>), assembliesToScan);
            AddRequestHandlers(services, typeof(IRequestHandler<,>), assembliesToScan);
            AddEventHandlers(services, typeof(IEventHandler<>), assembliesToScan);

            return services;
        }

        private static void AddRequestHandlers(IServiceCollection services, Type typeToAdd, Assembly[] assemblies)
        {
            var types = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.ContainsGenericParameters)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeToAdd))
                .Select(t => new { Command = t.ImplementedInterfaces.First().GenericTypeArguments.First(), Handler = t.UnderlyingSystemType })
                .ToList();

            foreach (var type in types)
            {
                services.TryAddTransient(type.Handler);
                Mediator.RequestsHandlers[type.Command] = type.Handler;
            }
        }
        private static void AddEventHandlers(IServiceCollection services, Type typeToAdd, Assembly[] assemblies)
        {
            var types = assemblies
                .SelectMany(a => a.DefinedTypes)
                .Where(t => !t.ContainsGenericParameters)
                .Where(t => t.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeToAdd))
                .Select(t => new { Command = t.ImplementedInterfaces.First().GenericTypeArguments.First(), Handler = t.UnderlyingSystemType })
                .GroupBy(t => t.Command)
                .ToList();

            foreach (var type in types)
            {
                foreach (var handler in type)
                {
                    services.TryAddTransient(handler.Handler);
                }
                Mediator.EventsHandlers[type.Key] = type.Select(t => t.Handler).ToList();
            }
        }
    }
}
