using Mongo.Conn;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL.Afiliados.Definicoes
{
    public class ProdutosDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AfiliadosDefinicoesProdutos = "Afiliados.Definicoes.Produtos";

        public List<ProdutoModel> GetAllItem()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProdutoModel>(AfiliadosDefinicoesProdutos);

            try
            {
                return collection.AsQueryable().ToList();
            }
            catch
            {
                return null;
            }
        }

        public ProdutoModel GetItemById(ObjectId id_item)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProdutoModel>(AfiliadosDefinicoesProdutos);

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

        public ProdutoModel GetProdutoByNome(string nomeProduto)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProdutoModel>(AfiliadosDefinicoesProdutos);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.NomeProduto == nomeProduto)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public ProdutoModel GetProdutoByTransacaoItemId(ObjectId TransacaoItemId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProdutoModel>(AfiliadosDefinicoesProdutos);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.TransacaoItemId == TransacaoItemId)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public ProdutoModel GetProdutoByIdPlanPagamento(string IdPlanPagamento)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ProdutoModel>(AfiliadosDefinicoesProdutos);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.IdPlanPagamento == IdPlanPagamento)
                                 .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }
    }
}
