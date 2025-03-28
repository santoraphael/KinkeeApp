using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyMongoWrapper
{
    // Simula o comportamento da classe legada MongoServer
    public class LegacyMongoServer
    {
        private readonly MongoClient _client;

        public LegacyMongoServer(MongoClient client)
        {
            _client = client;
        }

        public LegacyMongoDatabase GetLegacyDatabase(string databaseName)
        {
            var db = _client.GetDatabase(databaseName);
            return new LegacyMongoDatabase(db);
        }
    }

    // Simula o comportamento da classe legada MongoDatabase
    public class LegacyMongoDatabase
    {
        private readonly IMongoDatabase _database;

        public LegacyMongoDatabase(IMongoDatabase database)
        {
            _database = database;
        }

        public LegacyMongoCollection<T> GetCollection<T>(string name)
        {
            var collection = _database.GetCollection<T>(name);
            return new LegacyMongoCollection<T>(collection);
        }
    }

    // Simula a coleção legada (para operações como AsQueryable)
    public class LegacyMongoCollection<T>
    {
        private readonly IMongoCollection<T> _collection;

        public LegacyMongoCollection(IMongoCollection<T> collection)
        {
            _collection = collection;
        }

        public IQueryable<T> AsQueryable()
        {
            return _collection.AsQueryable();
        }
    }

    // Extensão para que o MongoClient "ganhe" o método GetLegacyServer()
    public static class MongoClientExtensions
    {
        public static LegacyMongoServer GetLegacyServer(this MongoClient client)
        {
            return new LegacyMongoServer(client);
        }
    }
}
