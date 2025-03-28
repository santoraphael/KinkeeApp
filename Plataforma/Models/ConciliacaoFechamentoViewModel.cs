using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace Plataforma.Models
{
    public class ConciliacaoFechamentoViewModel
    {
        public ObjectId UsuarioId { get; set; }
        public string NomeUsuario { get; set; }
        public int NumeroClicks { get; set; }
        public double ValorBruto { get; set; }
        public double ValorLiquido { get; set; }
        public double ValorPremio { get; set; }
        public double ValorComissao { get; set; }
        public double ValorAReceber { get; set; }
    }
}