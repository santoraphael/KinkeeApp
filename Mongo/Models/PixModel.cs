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
    public class PixModel
    {
        public ObjectId Id { get; set; }
        public DateTime DataEnvio { get; set; }
        public ObjectId UsuarioIdPagador { get; set; }
        public ObjectId UsuarioIdRecebedor { get; set; }
        public QrCodeEstaticoModel DadosQRCode { get; set; }
        public string Codigo { get; set; }
        public string Imagem { get; set; }
        public bool ConfirmacaoPagamento { get; set; }
    }

    public class QrCodeEstaticoModel
    {
        public string nome { get; set; }
        public string cidade { get; set; }
        public string chave { get; set; }
        public double valor { get; set; }
        public string txid { get; set; }
        public string mcc { get; set; }
        public SaidaPix saida { get; set; }
        public string tamanho { get; set; }
    }

    public class CodigoBrModel
    {
        public string brcode { get; set; }
    }

    public enum SaidaPix
    {
        qr,
        br
    }
}
