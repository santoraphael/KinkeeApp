using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Plataforma.Models
{
    public class ConfiguracaoPagamentoViewModel
    {
        public ObjectId Id { get; set; }
        public DateTime DataCadastro { get; set; }
        public ObjectId UsuarioId { get; set; }
        public TipoOpcaoPagamento TipoOpcaoPagamento { get; set; }
        public TipoChavePix TipoChavePix { get; set; }
        public TipoPessoaConta TipoPessoaConta { get; set; }
        public string ChavePix { get; set; }
        public string NomeTitular { get; set; }
        public string CNPJCPF { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
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
        //Celular = 0,
        Email = 1,
        CPF = 2,
        CNPJ = 3,
        Aleatoria = 4
    }
}