using HowToDevelop.Core.Comunicacao;
using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.Core.Comunicacao.Mediator;
using HowToDevelop.Core.Entidade;
using HowToDevelop.Core.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Infraestrutura
{
    [ExcludeFromCodeCoverage]
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, HealthFoodDbContext context)
        {
            IEnumerable<EntityEntry<RaizAgregacao>> domainEntities = context.ChangeTracker
                .Entries<RaizAgregacao>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());


            List<IEventoDominio> domainEvents = domainEntities
                .SelectMany(x => x.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) => 
                {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }

        public static async Task PublicarEventos(this IMediatorHandler mediator, IEnumerable<RaizAgregacao> entitiesWithNotifications)
        {
            var domainEvents = new List<IEventoDominio>();
            List<RaizAgregacao> withNotifications = entitiesWithNotifications.ToList();
            foreach (var entity in withNotifications)
            {
                var entityEvents = entity.Notificacoes.ToList();
                entityEvents.ForEach(x => 
                { 
                    x.RaizAgregacaoId = entity.Id;
                    domainEvents.Add(x); 
                });
            }

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
            
            withNotifications
                .ForEach(x => x.LimparEventos());
        }


    }
}
