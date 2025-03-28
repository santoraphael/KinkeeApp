using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class TransactionModel : BaseModel
    {
        public TransactionModel()
        {
            DateCreate = DateTime.Now;
        }

        public TransactionType TransactionType { get; set; }
        public ObjectId WalletAdressFrom { get; set; }
        public String WalletNameFrom { get; set; }
        public ObjectId WalletAdressTo { get; set; }
        public Double Amount { get; set; }
        public string Descricao { get; set; }
    }

    public enum TransactionType
    {
        [Description("Transferência Recebida")]
        Credit = 1,

        [Description("Transferência Realizada")]
        Debit = 2,

        [Description("Estorno Realizado")]
        Chargeback = 3,

        [Description("Crédito de Login Periódico")]
        LoginCredit = 4,

        [Description("Crédito de Nova Conta")]
        NewContaCredit = 5,
    }

    public static class EnumExtensionMethods
    {
        public static string GetDescription(this Enum GenericEnum)
        {
            Type genericEnumType = GenericEnum.GetType();
            MemberInfo[] memberInfo = genericEnumType.GetMember(GenericEnum.ToString());
            if ((memberInfo != null && memberInfo.Length > 0))
            {
                var _Attribs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                if ((_Attribs != null && _Attribs.Count() > 0))
                {
                    return ((System.ComponentModel.DescriptionAttribute)_Attribs.ElementAt(0)).Description;
                }
            }
            return GenericEnum.ToString();
        }

    }
}
