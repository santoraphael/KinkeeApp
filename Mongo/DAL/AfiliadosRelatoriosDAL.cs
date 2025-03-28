using Mongo.Conn;
using Mongo.Models;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL.Afiliados.Relatorios
{
    public class AfiliadosRelatoriosDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AfiliadosRelatorios = "Afiliados.Relatorios";

        public bool InsertClick(ClikLinkModel clicklink)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ClikLinkModel>(AfiliadosRelatorios);

            try
            {
                collection.InsertOne(clicklink);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<ClikLinkModel> GetIAllByDate(DateTime data)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ClikLinkModel>(AfiliadosRelatorios);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.DataHoraClick == data)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<ClikLinkModel> PegarClicksPorUsuario(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ClikLinkModel>(AfiliadosRelatorios);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.UsuarioId == usuarioId)
                                 .OrderByDescending(c => c.DataHoraClick)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
