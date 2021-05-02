using HowToDevelop.Core.StoredEvents;
using Microsoft.Extensions.DependencyInjection;

namespace HowToDevelop.EventSourcing
{
    public static class EventSourceInjection
    {
        public static IServiceCollection AddEventSource(this IServiceCollection services)
        {
            EventStoreService.InitializeClasses();
            services
                .AddScoped<IEventStoreRepository, EventStoreRepository>()
                .AddScoped<IEventStoreService, EventStoreService>();

            return services;
        }

    }


}
