using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace Mongo.DAL
{
    public class SwitcherDAL
    {
        private readonly Connection db = new Connection();
        private const string CollectionName = "Users.Wallet";

        /// <summary>
        /// Insere uma nova Wallet para o usuário (donoCarteira),
        /// caso ele ainda não possua uma.
        /// </summary>
        public bool InsertWallet(ObjectId donoCarteira)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<WalletModel>(CollectionName);

            bool retorno;
            try
            {
                // Se ainda não existir carteira para este dono, cria uma nova
                if (GetWalletByDono(donoCarteira) == null)
                {
                    var newWallet = new WalletModel(donoCarteira);
                    collection.InsertOne(newWallet);
                    retorno = true;
                }
                else
                {
                    // Já existe carteira para este dono
                    retorno = false;
                }
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        /// <summary>
        /// Altera uma Wallet existente (ou insere caso não exista),
        /// equivalendo ao antigo 'collection.Save<WalletModel>(...)'.
        /// </summary>
        public bool AlterarWallet(WalletModel walletUser)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<WalletModel>(CollectionName);

            bool retorno;
            try
            {
                // Se 'walletUser.Id' for a propriedade identificadora, usamos como filtro
                // Ajuste conforme a sua classe (se a chave for outro campo).
                var filter = Builders<WalletModel>.Filter.Eq(x => x.Id, walletUser.Id);

                // Upsert garante que se não existir, será criado, simulando o 'Save'
                collection.ReplaceOne(filter, walletUser, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        /// <summary>
        /// Busca a Wallet de um dono específico pelo campo DonoId.
        /// </summary>
        public WalletModel GetWalletByDono(ObjectId donoCarteira)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<WalletModel>(CollectionName);

            var wallet = collection.AsQueryable()
                                   .FirstOrDefault(w => w.DonoId == donoCarteira);
            return wallet;
        }
    }
}
