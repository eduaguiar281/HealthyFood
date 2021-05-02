using HowToDevelop.Core.Comunicacao.Interfaces;
using HowToDevelop.Core.StoredEvents;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace HowToDevelop.EventSourcing
{
    public class EventStoreService : IEventStoreService
    {
        public static void InitializeClasses()
        {
            BsonClassMap.RegisterClassMap<StoredEventBase<int>>(cm =>
            {
                cm.MapProperty(p => p.AggregateRootId);
                cm.MapProperty(p => p.Data);
                cm.MapProperty(p => p.EventName);
                cm.MapProperty(p => p.EventTimeStamp);
                cm.MapProperty(p => p.User);
                cm.SetDiscriminator("StoredEventBase`int");
                cm.MapIdProperty(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));
            });
            BsonClassMap.RegisterClassMap<StoredEvent>(cm =>
            {
                cm.MapCreator(s => new StoredEvent(s.AggregateRootId, s.Data, s.EventName, s.EventTimeStamp, s.User));
            });
        }


        private readonly IEventStoreRepository _eventStoreRepository;
        public EventStoreService(IEventStoreRepository repository)
        {
            _eventStoreRepository = repository;
        }
        public async Task<IEnumerable<StoredEvent>> ObterEventos(int aggregateId)
        {
            return await _eventStoreRepository.GetByIdAsync(aggregateId);
        }

        public async Task Salvar(IEventoDominio evento)
        {
            var storedEvent = new StoredEvent(evento.RaizAgregacaoId, evento.Data, evento.MensagemTipo, evento.Timestamp, null);
            await _eventStoreRepository.SaveAsync(storedEvent);
        }
    }
}
