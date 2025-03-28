using Mongo.Conn;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL.Afiliados.Definicoes
{
    public class TaxasCustosDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AfiliadosDefinicoesTaxasCustos = "Afiliados.Definicoes.TaxasCustos";

        public List<TaxaCustoModel> GetAllItem()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<TaxaCustoModel>(AfiliadosDefinicoesTaxasCustos);

            try
            {
                return collection.AsQueryable().ToList();
            }
            catch
            {
                return null;
            }
        }

        public TaxaCustoModel GetItemById(ObjectId id_item)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<TaxaCustoModel>(AfiliadosDefinicoesTaxasCustos);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.Id == id_item)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public TaxaCustoModel GetItemByNome(string nomeTaxasCustos)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<TaxaCustoModel>(AfiliadosDefinicoesTaxasCustos);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.NomeTaxaCusto == nomeTaxasCustos)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
