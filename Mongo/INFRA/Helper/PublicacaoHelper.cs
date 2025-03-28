using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Infrastruture.Helper
{
    public static class PublicacaoHelper
    {
        public static List<PublicacaoModel> ListaDePosts()
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            var query = collection.AsQueryable()
                                  .Where(u => u.isActive == true)
                                  .ToList();
            return query;
        }

        public static ObjectId AddPublicacao(PublicacaoModel publicacao)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            try
            {
                // 'Save' legado => substituído por ReplaceOne com IsUpsert = true
                var filter = Builders<PublicacaoModel>.Filter.Eq(p => p.Id, publicacao.Id);
                collection.ReplaceOne(filter, publicacao, new ReplaceOptions { IsUpsert = true });
            }
            catch
            {
            }

            return publicacao.Id;
        }

        public static PublicacaoModel GetPublicacaoByID(string _ID)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            var query = collection.AsQueryable()
                                  .FirstOrDefault(b => b.Id == ObjectId.Parse(_ID));
            return query;
        }

        public static List<PublicacaoModel> TodasAsPublicacoesAtivas(int ordenacao)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");
            List<PublicacaoModel> query;

            if (ordenacao == 0)
            {
                query = collection.AsQueryable()
                                  .Where(p => p.isActive == true)
                                  .OrderByDescending(p => p.DateCreate)
                                  .ToList();
            }
            else
            {
                query = collection.AsQueryable()
                                  .Where(p => p.isActive == true)
                                  .OrderByDescending(p => p.DateLastInteraction)
                                  .ToList();
            }

            return query;
        }

        public static List<PublicacaoModel> TodasAsPublicacoesDeAmigosAtivas(List<ObjectId> objectIds)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            // Filtro: UsuarioPublicacaoID in objectIds AND isActive == true
            var filter = Builders<PublicacaoModel>.Filter.In(e => e.UsuarioPublicacaoID, objectIds)
                       & Builders<PublicacaoModel>.Filter.Eq(e => e.isActive, true);

            var result = collection.Find(filter)
                                   .SortByDescending(u => u.DateLastInteraction)
                                   .ToList();
            return result;
        }

        public static List<PublicacaoModel> TodasAsPublicacoesDeMulheresAtivas(List<ObjectId> objectIds)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            // NotIn(e => e.UsuarioPublicacaoID, objectIds) => filtrar com !Contains
            // isActive == true
            var filter = Builders<PublicacaoModel>.Filter.Where(e => !objectIds.Contains(e.UsuarioPublicacaoID) && e.isActive == true);

            var result = collection.Find(filter)
                                   .SortByDescending(u => u.DateLastInteraction)
                                   .ToList();
            return result;
        }

        public static List<PublicacaoModel> TodasAsPublicacoesAtivasExcluindoHomens(List<ObjectId> objectIds)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            // In(e => e.UsuarioPublicacaoID, objectIds) AND isActive == true
            var filter = Builders<PublicacaoModel>.Filter.In(e => e.UsuarioPublicacaoID, objectIds)
                       & Builders<PublicacaoModel>.Filter.Eq(e => e.isActive, true);

            var result = collection.Find(filter)
                                   .SortByDescending(u => u.DateLastInteraction)
                                   .ToList();
            return result;
        }

        public static List<PublicacaoModel> TodasAsPublicacoesAtivasDoUsuario(ObjectId _ID, int ordenacao)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");
            List<PublicacaoModel> query;

            if (ordenacao == 0)
            {
                query = collection.AsQueryable()
                                  .Where(p => p.UsuarioPublicacaoID == _ID && p.isActive == true)
                                  .OrderByDescending(p => p.DateCreate)
                                  .ToList();
            }
            else
            {
                query = collection.AsQueryable()
                                  .Where(p => p.UsuarioPublicacaoID == _ID && p.isActive == true)
                                  .OrderByDescending(p => p.DateLastInteraction)
                                  .ToList();
            }

            return query;
        }

        public static bool RemovePublicacao(string _ID)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            var publicacao = GetPublicacaoByID(_ID);
            if (publicacao == null) return false;

            publicacao.isActive = false;

            bool retorno;
            try
            {
                // Substitui 'Save'
                var filter = Builders<PublicacaoModel>.Filter.Eq(p => p.Id, publicacao.Id);
                collection.ReplaceOne(filter, publicacao, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool AddLike(string _ID, string imagensPerfilUsuarioLike, string UsuarioQueCurtiuAPublicacao)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            var publicacao = GetPublicacaoByID(_ID);
            if (publicacao == null) return false;

            publicacao.Likes += 1;
            publicacao.DateLastInteraction = DateTime.Now;

            var usuarioCurtiu = new UsuarioCurtiuPublicacaoModel
            {
                imagensPerfilUsuarioLike = imagensPerfilUsuarioLike,
                NomeUsuarioLike = UsuarioQueCurtiuAPublicacao
            };

            if (publicacao.usuarioCurtiuPublicacao == null)
            {
                publicacao.usuarioCurtiuPublicacao = new List<UsuarioCurtiuPublicacaoModel>();
            }
            publicacao.usuarioCurtiuPublicacao.Add(usuarioCurtiu);

            bool retorno;
            try
            {
                var filter = Builders<PublicacaoModel>.Filter.Eq(p => p.Id, publicacao.Id);
                collection.ReplaceOne(filter, publicacao, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool AddComentario(string _IDPublicacao, string NomeUsuarioComentario, string imagemPerfilUsuarioComentario, string Comentario)
        {
            var db = new Connection();
            var database = db.ConnectServer();
            var collection = database.GetCollection<PublicacaoModel>("Users.NewsFeed");

            var publicacao = GetPublicacaoByID(_IDPublicacao);
            if (publicacao == null) return false;

            publicacao.DateLastInteraction = DateTime.Now;

            var comentarioModel = new UsuarioComentarioPublicacaoModel
            {
                DateCreate = DateTime.Now,
                DateLastInteraction = DateTime.Now,
                NomeUsuarioComentario = NomeUsuarioComentario,
                imagemPerfilUsuarioComentario = imagemPerfilUsuarioComentario,
                Comentario = Comentario
            };

            if (publicacao.Comentarios == null)
            {
                publicacao.Comentarios = new List<UsuarioComentarioPublicacaoModel>();
            }
            publicacao.Comentarios.Add(comentarioModel);

            bool retorno;
            try
            {
                var filter = Builders<PublicacaoModel>.Filter.Eq(p => p.Id, publicacao.Id);
                collection.ReplaceOne(filter, publicacao, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }
    }
}
