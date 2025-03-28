using Mongo.Conn;
using Mongo.Models;
using Mongo.Models.Afiliados;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL.Afiliados.Operacoes
{
    public class OperacoesDAL
    {
        private readonly Connection db = new Connection();
        private readonly string AfiliadosOperacoes = "Afiliados.Operacoes";
        private readonly string AfiliadosOperacoesHistorico = "Afiliados.Operacoes.Historico";
        private readonly string AfiliadosLote = "Afiliados.Lote";

        public bool InsertOperacao(OperacaoModel operacao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);
            try
            {
                collection.InsertOne(operacao);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool InsertOperacaoHistorico(OperacaoModel operacao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoesHistorico);
            try
            {
                collection.InsertOne(operacao);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeteleOperacaoFechamento(OperacaoModel operacao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);
            try
            {
                var filter = Builders<OperacaoModel>.Filter.Eq("_id", operacao.Id);
                collection.DeleteOne(filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool AlterarOperacao(OperacaoModel operacao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);

            bool retorno;
            try
            {
                var filter = Builders<OperacaoModel>.Filter.Eq(x => x.Id, operacao.Id);
                collection.ReplaceOne(filter, operacao, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public OperacaoModel GetItemById(ObjectId id_item)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(i => i.Id == id_item);
            }
            catch
            {
                return null;
            }
        }

        public List<OperacaoModel> GetIAllById(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.UsuarioId == usuarioId)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        public List<OperacaoModel> ListarOperacoesLiberadas(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);

            try
            {
                return collection.AsQueryable()
                                 .Where(i => i.UsuarioId == usuarioId && i.OperacaoLiberada == true)
                                 .ToList();
            }
            catch
            {
                return null;
            }
        }

        public OperacaoModel FecharOperacaoLote(ObjectId id_item, ObjectId idLote)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<OperacaoModel>(AfiliadosOperacoes);

            // Em seu código, há a intenção de atualizar "LoteFechado", "idLote" e "DataFechamento",
            // mas a função final apenas retorna o item, sem efetivar a atualização. Segue o mesmo comportamento:
            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(i => i.Id == id_item);
            }
            catch
            {
                return null;
            }
        }

        public LoteOperacoesModel InsertLote(LoteOperacoesModel LoteOperacao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<LoteOperacoesModel>(AfiliadosLote);
            try
            {
                collection.InsertOne(LoteOperacao);
                return LoteOperacao;
            }
            catch
            {
                return null;
            }
        }

        public LoteOperacoesModel PegarLoteNaoPago(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<LoteOperacoesModel>(AfiliadosLote);

            try
            {
                return collection.AsQueryable()
                                 .FirstOrDefault(i => i.UsuarioId == usuarioId && i.StatusLote != StatusLote.pago);
            }
            catch
            {
                return null;
            }
        }

        public bool AlterarLote(LoteOperacoesModel LoteOperacao)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<LoteOperacoesModel>(AfiliadosLote);

            bool retorno;
            try
            {
                var filter = Builders<LoteOperacoesModel>.Filter.Eq(x => x.Id, LoteOperacao.Id);
                collection.ReplaceOne(filter, LoteOperacao, new ReplaceOptions { IsUpsert = true });
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
