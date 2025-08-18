using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Applications.Events
{
    internal sealed partial class DomainEventsDispatcher(IServiceProvider serviceProvider) : IDomainEventsDispatcher
    {
        private static readonly ConcurrentDictionary<Type, Type> HandlerTypeDictionary = new();
        private static readonly ConcurrentDictionary<Type, Type> WrapperTypeDictionary = new();

        public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents, CancellationToken cancellationToken = default)
        {
            foreach (IDomainEvent domainEvent in domainEvents)
            {
                using IServiceScope scope = serviceProvider.CreateScope();

                Type domainEventType = domainEvent.GetType();

                Type handlerType = HandlerTypeDictionary.GetOrAdd(
                    domainEventType,
                    et => typeof(IDomainEventHandler<>).MakeGenericType(et));

                IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);

                foreach (object? handler in handlers)
                {
                    if (handler is null) continue;

                    var handlerWrapper = HandlerWrapper.Create(handler, domainEventType);

                    await handlerWrapper.Handle(domainEvent, cancellationToken);
                }
            }
        }

       
    }



}
