using Mongo.Models.Afiliados;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mongo.Models
{
    public class ClikLinkModel
    {
        public ObjectId Id { get; set; }
        public DateTime DataHoraClick { get; set; }
        public ObjectId UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public string CodigoConvite { get; set; }
    }

    public class OperacaoModel
    {
        public ObjectId Id { get; set; }
        public ObjectId IdOperacaoRelacionada { get; set; } //
        public TipoOperacao TipoOperacao { get; set; }
        public DateTime? DataOperacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool OperacaoLiberada { get; set; }
        public ObjectId? UsuarioId { get; set; } //DONO DO BONUS
        public ObjectId? IdConvidado { get; set; } //QUEM CONVIDOU O DONO DO BONUS
        public ObjectId IdTipoGanho { get; set; }
        public NomesPerfis NomePerfil { get; set; }
        public double TaxaCartao { get; set; }
        public double TaxaISS { get; set; }
        public double CustoFixo { get; set; }
        public double CustoBoleto { get; set; }
        public double ValorBruto { get; set; }
        public double ValorLiquido { get; set; }
        public double ValorPremio { get; set; }
        public double ValorComissao { get; set; }
        public double ValorAReceber { get; set; }
        public bool LoteFechado { get; set; }
        public ObjectId idLote { get; set; }
        public DateTime? DataFechamento { get; set; }
    }

    public class LoteOperacoesModel 
    {
        public ObjectId Id { get; set; }
        public ObjectId UsuarioId { get; set; }
        public DateTime? DataFechamento { get; set; }
        public List<ObjectId> OperacoesLote { get; set; }
        public double ValorLote { get; set; }
        public string ContaPagamento { get; set; }
        public string UrlImagemComprovante { get; set; }
        public DateTime? DataPagamento { get; set; }
        public StatusLote StatusLote { get; set; }
    }

    public class RelatorioModel
    {
        public DateTime Data { get; set; }
        public int NumeroClicks { get; set; }
        public int NumeroCadastros{ get; set; }
        public int NumeroPerfisCriados{ get; set; }
        public int NumeroPerfisAprovados{ get; set; }
        public int NumeroAssinaturas { get; set; }
        public double ValorDescontoComissao{ get; set; }
        public double ValorPremioBruto{ get; set; }
        public double ValorPremioLiquido { get; set; }
        public double ValorLiberado { get; set; }
    }

    public class ConfiguracaoPagamentoModel
    {
        public ObjectId Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public ObjectId UsuarioId { get; set; }
        public TipoOpcaoPagamento TipoOpcaoPagamento{ get; set; }
        public TipoChavePix TipoChavePix { get; set; }
        public TipoPessoaConta TipoPessoaConta { get; set; }
        public string ChavePix { get; set; }
        public string NomeTitular { get; set; }
        public string CNPJCPF { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }

    }

    public enum StatusLote
    {
        fechado,
        processamento,
        pago
    }

    public enum TipoOpcaoPagamento
    {
        PIX,
        ContaCorrente,
        ContaPoupanca
    }

    public enum TipoPessoaConta
    {
        Fisica,
        Juridica
    }

    public enum TipoChavePix
    {
        Celular,
        Email,
        CPF,
        CNPJ,
        Aleatoria
    }

    public enum TipoOperacao
    {
        Item,
        Assinatura
    }
}
