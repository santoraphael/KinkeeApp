using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Infrastruture.Helper
{
    public static class ProductHelper
    {
        private static readonly string SubscriptiosCategory = "Subscriptios.Category";
        private static readonly string SubscriptiosProduct = "Subscriptios.Product";
        private static readonly string Subscriptios = "Subscriptios";

        #region Product

        public static bool InsertProduct(ProductSubscriptionModel newProduct)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<ProductSubscriptionModel>(SubscriptiosProduct);
            try
            {
                collection.InsertOne(newProduct);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static ProductSubscriptionModel GetProductById(ObjectId productID)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<ProductSubscriptionModel>(SubscriptiosProduct);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(s => s.Id == productID);
            }
            catch
            {
                return null;
            }
        }

        public static ProductSubscriptionModel GetProductByHashPagSeguroPlan(string hashPagSeguroPlan)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<ProductSubscriptionModel>(SubscriptiosProduct);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(s => s.HashPagSeguroPlan == hashPagSeguroPlan);
            }
            catch
            {
                return null;
            }
        }

        public static List<ProductSubscriptionModel> GetListProductByUserOwnerId(ObjectId userOwnerId)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<ProductSubscriptionModel>(SubscriptiosProduct);

            try
            {
                return collection.AsQueryable()
                                 .Where(s => s.OwnerUserId == userOwnerId)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Subcription

        public static bool InsertSubcription(SubscriptionModel newSubscription)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<SubscriptionModel>(Subscriptios);
            try
            {
                collection.InsertOne(newSubscription);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static SubscriptionModel GetSubscriptionById(ObjectId subscriptionID)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<SubscriptionModel>(Subscriptios);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(s => s.Id == subscriptionID);
            }
            catch
            {
                return null;
            }
        }

        public static List<SubscriptionModel> GetListSubscriptionByUserId(ObjectId userId)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<SubscriptionModel>(Subscriptios);

            try
            {
                return collection.AsQueryable()
                                 .Where(s => s.UserId == userId)
                                 .ToList();
            }
            catch (Exception ex)
            {
                var exc = ex.Message;
                return null;
            }
        }

        public static List<SubscriptionModel> GetListSubscriptionByUserIdStatus(ObjectId userId, SubscriptionStatus subscriptionStatus)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<SubscriptionModel>(Subscriptios);

            try
            {
                return collection.AsQueryable()
                                 .Where(s => s.UserId == userId && s.Status == subscriptionStatus)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        public static List<ProductSubscriptionModel> GetFirstProductSubscriptionActive()
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<ProductSubscriptionModel>(SubscriptiosProduct);

            try
            {
                var resultCount = collection.AsQueryable()
                                            .Count(s => s.Status == ProductStatus.Active);
                var randomSkip = (new Random()).Next(1, resultCount);

                return collection.AsQueryable()
                                 .Where(s => s.Status == ProductStatus.Active)
                                 .Skip(randomSkip * 1)
                                 .Take(1)
                                 .ToList();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return null;
            }
        }

        public static List<ProductSubscriptionModel> GetListProductSubscriptionByUserIdStatus(ObjectId userId, ProductStatus productStatus)
        {
            var dbConn = new Connection();
            var database = dbConn.ConnectServer();
            var collection = database.GetCollection<ProductSubscriptionModel>(SubscriptiosProduct);

            try
            {
                return collection.AsQueryable()
                                 .Where(s => s.OwnerUserId == userId && s.Status == productStatus)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
