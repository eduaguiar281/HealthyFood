using HowToDevelop.Core.StoredEvents;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HowToDevelop.EventSourcing
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly IMongoCollection<StoredEvent> _mongoCollection;

        public EventStoreRepository(string connectionString, string databaseName)
        {
            MongoClient mongoClient = new MongoClient(connectionString);
            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(databaseName);
            _mongoCollection = mongoDatabase.GetCollection<StoredEvent>(nameof(StoredEvent));
        }

        public EventStoreRepository(IConfiguration configuration)
        {
            var settings = configuration.GetSection(nameof(EventStoreConnectionSettings)).Get<EventStoreConnectionSettings>();
            MongoClient mongoClient = new MongoClient(settings.ConnectionString);

            IMongoDatabase mongoDatabase = mongoClient.GetDatabase(settings.DataBaseName);
            _mongoCollection = mongoDatabase.GetCollection<StoredEvent>(nameof(StoredEvent));
        }


        public async Task<IEnumerable<StoredEvent>> GetByIdAsync(int aggregateRootId)
        {
            var sort = Builders<StoredEvent>.Sort.Descending(nameof(StoredEvent.EventTimeStamp));
            return await _mongoCollection
                .Find(s => s.AggregateRootId == aggregateRootId).Sort(sort).ToListAsync();
        }

        public async Task SaveAsync(StoredEvent @event)
        {
            await _mongoCollection.InsertOneAsync(@event);
        }
    }
}
