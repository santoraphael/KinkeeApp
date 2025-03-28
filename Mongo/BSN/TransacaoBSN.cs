using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;
using Mongo.Infrastruture.Helper;
using Mongo.Models.Compra;

namespace Mongo.BSN
{
    public class TransacaoBSN
    {
        TransacaoDAL transacaoDAL = new TransacaoDAL();
        public List<Item> GetAllItems()
        {

            return transacaoDAL.GetAllItem();

        }

        public Item GetItemById(ObjectId id_item)
        {
            return transacaoDAL.GetItemById(id_item);
        }
    }
}
