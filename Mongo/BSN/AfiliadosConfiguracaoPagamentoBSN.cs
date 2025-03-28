using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.Models;
using System.Collections.Generic;
using Mongo.DAL.Afiliados.ConfiguracaoPagamento;

namespace Mongo.BSN
{
    public class AfiliadosConfiguracaoPagamentoBSN
    {
        AfiliadosConfiguracaoPagamentoDAL AfiliadosRelatoriosDAL = new AfiliadosConfiguracaoPagamentoDAL();

        public bool InsertCofiguracaoPagamento(ConfiguracaoPagamentoModel configuracaoPagamento)
        {
            return AfiliadosRelatoriosDAL.InsertCofiguracaoPagamento(configuracaoPagamento);
        }

        public bool AlterarCofiguracaoPagamento(ConfiguracaoPagamentoModel configuracaoPagamento)
        {
            return AfiliadosRelatoriosDAL.AlterarCofiguracaoPagamento(configuracaoPagamento);
        }

        public IList<ConfiguracaoPagamentoModel> GetAllCofiguracaoPagamento(ObjectId usuarioId)
        {
            return AfiliadosRelatoriosDAL.GetAllCofiguracaoPagamento(usuarioId);
        }
    }
}
