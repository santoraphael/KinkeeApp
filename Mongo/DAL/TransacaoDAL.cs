using Mongo.Conn;
using Mongo.Models;
using Mongo.Models.Compra;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL
{
    public class TransacaoDAL
    {
        private readonly Connection db = new Connection();
        private readonly string UsersTransactions = "Users.Transacao";
        private readonly string UsersTransactionsItem = "Users.Transacao.Item";

        public bool InsertTransacao(TransacaoRetorno newTransacao)
        {
            var _database = db.ConnectServer();
            var collection = _database.GetCollection<TransacaoRetorno>(UsersTransactions);
            try
            {
                collection.InsertOne(newTransacao);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertItem(Item newItem)
        {
            var _database = db.ConnectServer();
            var collection = _database.GetCollection<Item>(UsersTransactionsItem);
            try
            {
                collection.InsertOne(newItem);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Item> GetAllItem()
        {
            var _database = db.ConnectServer();
            var collection = _database.GetCollection<Item>(UsersTransactionsItem);

            try
            {
                return collection.AsQueryable().ToList();
            }
            catch
            {
                return null;
            }
        }

        public Item GetItemById(ObjectId id_item)
        {
            var _database = db.ConnectServer();
            var collection = _database.GetCollection<Item>(UsersTransactionsItem);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(i => i.id == id_item);
            }
            catch
            {
                return null;
            }
        }
    }
}
