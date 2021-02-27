using HowToDevelop.Core.Comunicacao.Mediator;
using HowToDevelop.Core.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace HowToDevelop.HealthFood.Dominio.Infraestrutura
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, HealthFoodDbContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<IRaizAgregacao>()
                .Where(x => x.Entity.Notificacoes != null && x.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
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

    }
}
