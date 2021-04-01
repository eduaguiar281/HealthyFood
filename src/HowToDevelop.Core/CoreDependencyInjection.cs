using HowToDevelop.Core.Comunicacao.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace HowToDevelop.Core
{
    [ExcludeFromCodeCoverage]
    public static class CoreDependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        {
            // database
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }

    }
}
