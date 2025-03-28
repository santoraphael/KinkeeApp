using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Plataforma.Models
{
    public class PaymentViewModel
    {
        public string valorPromocionalSemestralMensal { get; set; }
        public string valorCheioSemDesconto { get; set; }
        
        public PlanModel planoMensal { get; set; }
        public PlanModel planoTrimestral { get; set; }
        public PlanModel planoSemestral { get; set; }
    }

    public class PaymentStoreViewModel
    {
        public List<Item> listaItens { get; set; }
    }

    public class Item
    {
        public ObjectId id { get; set; }
        public string title { get; set; }
        public double unit_price { get; set; }
        public string valor_reais { get; set; }
        public string valor_centavos { get; set; }
        public string valor_original { get; set; }

        public int quantity { get; set; }
        public bool tangible { get; set; }
    }

    public class PlanModel
    {
        public string dataPlanId { get; set; }
        public string descricaoNomeValorPlano { get; set; }
        public string valorPlanoReais { get; set; }
        public string valorPlanoCentavos { get; set; }
        public string valorPoints { get; set; }
        public string descricaoParcelamento{ get; set; }
        public List<MeioPagamento> meioPagamentos { get; set; }
    }

    public enum MeioPagamento
    {
        CartaoCredito = 1,
        TransferenciaBancaria = 2,
        BoletoBancario = 3,
        Points = 4,
    }

    public class PagamentoCartao
    {
        [Required]
        public string plan_id { get; set; }

        [Required]
        public string numeroCartaoCredito { get; set; }

        [Required]
        public string mesValidadeCartao { get; set; }

        [Required]
        public string anoValidadeCartao { get; set; }

        [Required]
        public string nomeTitularCartao { get; set; }

        [Required]
        public string codigoSeguranca { get; set; }

        [Required]
        public string numeroDocumento { get; set; }

        [Required]
        public string cep { get; set; }

        [Required]
        public double valorTotal { get; set; }

        [Required]
        public int numeroParcelas { get; set; }
    }

    public class PagamentoPoints
    {
        [Required]
        public string plan_id { get; set; }

        public int ValorSaldo { get; set; }

        public int ValorItem { get; set; }

        public int ValorFalta { get; set; }

    }

    public class RetornoPagamentoBoleto
    {
        public string valor_reais { get; set; }

        public string valor_centavos { get; set; }

        public string dia_vencimento { get; set; }

        public string mes_vencimento { get; set; }

        public string codigo_barra_boleto { get; set; }

        public string url_boleto { get; set; }
    }
}