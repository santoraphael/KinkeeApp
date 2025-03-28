using Mongo.Conn;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL.Afiliados.Definicoes
{
    public class TiposGanhosDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AfiliadosDefinicoesTiposGanhos = "Afiliados.Definicoes.TiposGanhos";

        public List<TipoGanhoModel> GetAllItem()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<TipoGanhoModel>(AfiliadosDefinicoesTiposGanhos);

            try
            {
                return collection.AsQueryable().ToList();
            }
            catch
            {
                return null;
            }
        }

        public TipoGanhoModel GetItemById(ObjectId id_item)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<TipoGanhoModel>(AfiliadosDefinicoesTiposGanhos);

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

        public TipoGanhoModel GetTipoGanhoByNome(string nomeToposGanhos, string perfil)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<TipoGanhoModel>(AfiliadosDefinicoesTiposGanhos);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.NomeTipoGanho == nomeToposGanhos && i.Perfil == perfil)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
