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
    public class PixDAL
    {
        private readonly Connection db = new Connection();
        private readonly string CollectionUsersPix = "Users.Pix";

        public bool InsertPix(PixModel pix)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<PixModel>(CollectionUsersPix);
            try
            {
                collection.InsertOne(pix);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public PixModel PegarPixPorId(ObjectId pixId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<PixModel>(CollectionUsersPix);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(p => p.Id == pixId);
            }
            catch
            {
                return null;
            }
        }

        public bool SalvarPixPorId(PixModel pix)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<PixModel>(CollectionUsersPix);

            try
            {
                var filter = Builders<PixModel>.Filter.Eq("_id", pix.Id);
                collection.ReplaceOne(filter, pix, new ReplaceOptions { IsUpsert = true });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoverPixPorId(ObjectId pixId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<PixModel>(CollectionUsersPix);

            try
            {
                var filter = Builders<PixModel>.Filter.Eq("_id", pixId);
                collection.DeleteOne(filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<PixModel> PegarPixPorUsuarioPagador(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<PixModel>(CollectionUsersPix);

            try
            {
                return collection.AsQueryable()
                                 .Where(p => p.UsuarioIdPagador == usuarioId && p.ConfirmacaoPagamento == true)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<PixModel> PegarPixPorUsuarioRecebedor(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<PixModel>(CollectionUsersPix);

            try
            {
                return collection.AsQueryable()
                                 .Where(p => p.UsuarioIdRecebedor == usuarioId && p.ConfirmacaoPagamento == false)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }
    }
}
