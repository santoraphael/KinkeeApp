using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mongo.Models.Afiliados
{
    public class ProdutoModel
    {
        public ObjectId Id { get; set; }
        public string IdPlanPagamento { get; set; } //id do plano no pagarme
        public ObjectId TransacaoItemId { get; set; } //id da tabela Users.Transacao.Item
        public string NomeProduto { get; set; }
        public double ValorBruto { get; set; }
    }

    public class ProdutoLiquidoModel
    {
        public ObjectId Id { get; set; }
        public string NomeProduto { get; set; }
        public double ValorLiquido { get; set; }
    }

    public class TaxaCustoModel
    {
        public ObjectId Id { get; set; }
        public string NomeTaxaCusto { get; set; }
        public string Perfil { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
    }

    public class TipoGanhoModel
    {
        public ObjectId Id { get; set; }
        public string NomeTipoGanho { get; set; }
        public string Perfil { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }
    }

    public enum NomesProdutos
    {
        AssinaturaBabyPremiumMensal,
        AssinaturaBabyPremiumTrimestral,
        AssinaturaBabyPremiumSemestral,
        AssinaturaDaddyPremiumMensal,
        AssinaturaDaddyPremiumTrimestral,
        AssinaturaDaddyPremiumSemestral,

        KinkeePoints500,
        KinkeePoints1050,
        KinkeePoints2175,
        KinkeePoints3850,
        KinkeePoints5550,
        KinkeePoints11500
    }

    public enum NomesTaxasCustos
    {
        CustoFixoTransacao,
        TaxaCartaoCredito,
        ValorBoletoPago,
        ValorSaquePagarMe,
        ValorPixBanco,

        TaxaIIS,
        PercentualComissao
    }

    public enum NomesTiposGanhos
    {
        ValorPorDaddyAprovado,
        InteracaoDaddy,
        PercentualCompras,
        ValorPorBabyAprovada,
        Comissao
    }

    public enum NomesPerfis
    {
        semassinatura,
        influencer,
        comassinatura
    }
}
