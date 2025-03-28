using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;
using Mongo.Infrastruture.Helper;

namespace Mongo.BSN
{
    public class StoreBSN
    {
        StoreDAL storeDAL = new StoreDAL();


        #region CATEGORY

        public bool InsertCategory(CategoryModel newCategory)
        {
            return storeDAL.InsertCategory(newCategory);
        }

        public CategoryModel GetCategoryById(ObjectId categoryID)
        {
            return storeDAL.GetCategoryById(categoryID);
        }

        public CategoryModel GetCategoryByName(string categoryName)
        {
            return storeDAL.GetCategoryByName(categoryName);
        }

        #endregion

        #region PRODUCT

        public bool InsertProduct(ProductModel newProduct)
        {
            //Produto sem categoria
            if(newProduct.CategoryId == null)
            {
                var categoria = GetCategoryByName("Sem Categoria");

                if(categoria == null)
                {
                    categoria.Name = "Sem Categoria";

                    InsertCategory(categoria);
                }
                
                newProduct.CategoryId = categoria.Id;
            }

            return storeDAL.InsertProduct(newProduct);
        }

        public ProductModel GetProductById(ObjectId productID)
        {
            return storeDAL.GetProductById(productID);
        }

        #endregion

        #region BAG

        public bool InsertBag(BagModel newBag)
        {
            return storeDAL.InsertBag(newBag);
        }

        public BagModel GetBagById(ObjectId bagID)
        {
            return storeDAL.GetBagById(bagID);
        }

        #endregion
    }
}
