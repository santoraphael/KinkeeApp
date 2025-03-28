
using Mongo.INFRA;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class WalletModel : BaseModel
    {
        public WalletModel(ObjectId _DonoId)
        {
            DateCreate = DateTime.Now;
            DonoId = _DonoId;
            Saldo = 0;
            List<TransactionModel> _transactions = new List<TransactionModel>();
            Transactions = _transactions;
        }

        public ObjectId DonoId { get; set; }
        public Double Saldo { get; set; }
        public ICollection<TransactionModel> Transactions { get; set; }
    }

    public class DadosBancariosModel : BaseModel
    {
        public ObjectId UserId { get; set; }
        public string NomeCompleto { get; set; }
        public string CPF { get; set; }
        public string NomeBanco { get; set; }
        public string TipoConta { get; set; }
        public string NumeroAgencia { get; set; }
        public string NumeroConta { get; set; }
    }
}
