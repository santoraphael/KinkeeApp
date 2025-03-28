using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL
{
    public class SubscriptionDAL
    {
        private readonly Connection db = new Connection();
        private readonly string UsersCards = "Users.Cards";
        private readonly string UsersSubscriptios = "Users.Subscriptios";

        public bool InsertCard(CartaoModel newCard)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CartaoModel>(UsersCards);

            try
            {
                var cartao = GetCardByIdCartao(newCard.id_cartao);
                if (cartao == null)
                {
                    collection.InsertOne(newCard);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CartaoModel GetCardByUserId(ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CartaoModel>(UsersCards);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(s => s.Id == userId);
            }
            catch
            {
                return null;
            }
        }

        public CartaoModel GetCardByIdCartao(string id_cartao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CartaoModel>(UsersCards);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(s => s.id_cartao == id_cartao);
            }
            catch
            {
                return null;
            }
        }

        public AssinaturaModel InsertAssinatura(AssinaturaModel newAssinatura)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<AssinaturaModel>(UsersSubscriptios);

            try
            {
                collection.InsertOne(newAssinatura);
                return newAssinatura;
            }
            catch
            {
                return newAssinatura;
            }
        }

        public AssinaturaModel GetAssinaturaByUserId(ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<AssinaturaModel>(UsersSubscriptios);

            try
            {
                return collection.AsQueryable()
                                 .Where(s => s.userId == userId)
                                 .OrderByDescending(s => s.current_period_end)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public AssinaturaModel GetAssinaturaByID(ObjectId id_subscription)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<AssinaturaModel>(UsersSubscriptios);

            try
            {
                return collection.AsQueryable()
                                 .Where(s => s.Id == id_subscription)
                                 .OrderByDescending(s => s.current_period_end)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
