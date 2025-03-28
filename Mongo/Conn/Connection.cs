using MongoDB.Driver;
using System;

namespace Mongo.Conn
{
    public class Connection
    {
        // Você pode definir a connection string como constante ou pegar de um arquivo de configuração.
        private readonly string connectionString =
            "mongodb://kinkee01:R330p908@193.203.165.26:27017/Kinkee?loadBalanced=false&connectTimeoutMS=10000&authMechanism=SCRAM-SHA-1&authSource=Kinkee";

        // O MongoClient (driver 3.x) que iremos reutilizar
        private readonly MongoClient client;

        public Connection()
        {
            // Instanciamos o client apenas uma vez, em geral (Singleton ou static, dependendo do seu cenário).
            client = new MongoClient(connectionString);
        }

        /// <summary>
        /// Retorna um IMongoDatabase referente ao banco "Kinkee".
        /// </summary>
        public IMongoDatabase ConnectServer()
        {
            // No driver novo, basta chamar GetDatabase no cliente
            return client.GetDatabase("Kinkee");
        }
    }
}
