using Mongo.Conn;
using Mongo.Models;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mongo.DAL.Afiliados.ConfiguracaoPagamento
{
    public class AfiliadosConfiguracaoPagamentoDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AfiliadosConfiguracaoPagamento = "Afiliados.ConfiguracaoPagamento";

        /// <summary>
        /// Insere o documento (configuração de pagamento).
        /// (Equivalente ao antigo collection.Insert<T>(...))
        /// </summary>
        public bool InsertCofiguracaoPagamento(ConfiguracaoPagamentoModel configuracaoPagamento)
        {
            // Obtenção do IMongoDatabase na nova API
            var database = db.ConnectServer();

            // Agora obtemos uma coleção *tipada* (em vez de BsonDocument)
            var collection = database.GetCollection<ConfiguracaoPagamentoModel>(AfiliadosConfiguracaoPagamento);

            try
            {
                // Substitui collection.Insert<T>(...) pelo InsertOne
                collection.InsertOne(configuracaoPagamento);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Altera (ou insere, se não existir) o documento.
        /// (Equivalente ao antigo collection.Save<T>(...))
        /// </summary>
        public bool AlterarCofiguracaoPagamento(ConfiguracaoPagamentoModel configuracaoPagamento)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ConfiguracaoPagamentoModel>(AfiliadosConfiguracaoPagamento);

            try
            {
                // O antigo "Save" fazia um upsert baseado no _id ou Id
                // Aqui, assumimos que 'configuracaoPagamento.Id' seja a chave do documento
                var filter = Builders<ConfiguracaoPagamentoModel>.Filter.Eq(x => x.Id, configuracaoPagamento.Id);

                // ReplaceOne com IsUpsert = true insere caso não exista, ou substitui caso já exista
                collection.ReplaceOne(filter, configuracaoPagamento, new ReplaceOptions { IsUpsert = true });
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Retorna todas as configurações de pagamento de um usuário específico
        /// (Equivalente ao antigo uso de collection.AsQueryable<T>())
        /// </summary>
        public List<ConfiguracaoPagamentoModel> GetAllCofiguracaoPagamento(ObjectId UsuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ConfiguracaoPagamentoModel>(AfiliadosConfiguracaoPagamento);

            try
            {
                // Substitui a chamada .AsQueryable<TModel>().Where(...)
                return collection.AsQueryable()
                                 .Where(i => i.UsuarioId == UsuarioId)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
