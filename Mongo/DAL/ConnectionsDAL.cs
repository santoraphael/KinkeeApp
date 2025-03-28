using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Mongo.DAL
{
    public class ConnectionsDAL
    {
        private readonly Connection db = new Connection();
        private const string CollectionName = "connections";

        /// <summary>
        /// Insere um novo ConnectionModel na coleção "connections".
        /// </summary>
        public bool InserConnection(ConnectionModel connection)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ConnectionModel>(CollectionName);

            bool retorno;
            try
            {
                collection.InsertOne(connection);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        /// <summary>
        /// Altera (ou insere caso não exista) um ConnectionModel na coleção "connections".
        /// Equivale ao antigo 'collection.Save<T>(...)'.
        /// </summary>
        public bool AlterarConnection(ConnectionModel connection)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ConnectionModel>(CollectionName);

            bool retorno;
            try
            {
                // Supondo que 'connection.Id' seja a chave, para que o ReplaceOne funcione como upsert.
                var filter = Builders<ConnectionModel>.Filter.Eq(x => x.ConnectionID, connection.ConnectionID);
                collection.ReplaceOne(filter, connection, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        /// <summary>
        /// Exemplo de método que obtém um ConnectionModel pelo ConnectionId (string),
        /// caso seja usado como identificador primário. Ajuste conforme sua lógica real.
        /// </summary>
        public ConnectionModel GetConnection(string connectionId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ConnectionModel>(CollectionName);

            // Se 'ConnectionId' for uma string única, busque por esse campo.
            // Se for na verdade 'Id' do tipo ObjectId, converta e compare.
            // Supondo que ConnectionModel tenha um campo 'ConnectionId' (string).
            var filter = Builders<ConnectionModel>.Filter.Eq(x => x.ConnectionID, connectionId);
            var result = collection.Find(filter).FirstOrDefault();
            return result;
        }
    }
}
