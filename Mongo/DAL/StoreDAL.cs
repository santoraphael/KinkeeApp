using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.BSN
{
    public class StoreDAL
    {
        private readonly Connection db = new Connection();
        private readonly string StoreCategory = "Store.Category";
        private readonly string StoreProduct = "Store.Product";
        private readonly string StoreBag = "Store.Bag";
        private readonly string StoreOrder = "Store.Order";

        #region CATEGORY

        public bool InsertCategory(CategoryModel newCategory)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CategoryModel>(StoreCategory);
            try
            {
                collection.InsertOne(newCategory);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public CategoryModel GetCategoryById(ObjectId categoryID)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CategoryModel>(StoreCategory);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(c => c.Id == categoryID);
            }
            catch
            {
                return null;
            }
        }

        public CategoryModel GetCategoryByName(string categoryName)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<CategoryModel>(StoreCategory);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(c => c.Name.ToLower() == categoryName.ToLower());
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region PRODUCT

        public bool InsertProduct(ProductModel newProduct)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProductModel>(StoreProduct);
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

        public ProductModel GetProductById(ObjectId productID)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProductModel>(StoreProduct);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(p => p.Id == productID);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region BAG

        public bool InsertBag(BagModel newBag)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<BagModel>(StoreBag);
            try
            {
                collection.InsertOne(newBag);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BagModel GetBagById(ObjectId bagID)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<BagModel>(StoreBag);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(b => b.Id == bagID);
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
