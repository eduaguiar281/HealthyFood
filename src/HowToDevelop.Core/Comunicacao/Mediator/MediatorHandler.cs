using CSharpFunctionalExtensions;
using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.Core.StoredEvents;
using MediatR;
using System.Threading.Tasks;

namespace HowToDevelop.Core.Comunicacao.Mediator
{
    public class MediatorHandler: IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly IEventStoreService _eventStoreService;

        public MediatorHandler(IMediator mediator, IEventStoreService eventStoreService)
        {
            _mediator = mediator;
            _eventStoreService = eventStoreService;
        }

        public async Task<Result<T>> EnviarComando<T>(ICommand<T> comando)
        {
            return await _mediator.Send(comando);
        }

        public async Task<Result<T>> EnviarQuery<T>(IQuery<T> query)
        {
            return await _mediator.Send(query);
        }

        public async Task PublicarEvento<T>(T evento) where T : IEventoDominio
        {
            await _mediator.Publish(evento);
            await _eventStoreService.Salvar(evento);
        }
    }
}
