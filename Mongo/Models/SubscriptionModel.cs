using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class CategorySubscriptionModel : BaseModel
    {
        public string Name{ get; set; }
    }


    public class ProductSubscriptionModel : BaseModel
    {
        public string codeplanpagseguro { get; set; }
        //public CategorySubscriptionModel Category { get; set; }
        public string NameProduct { get; set; }
        public ObjectId OwnerUserId { get; set; }
        public SubscriptionType Type { get; set; }
        public double Amount { get; set; }
        public ProductStatus Status { get; set; }
        public string StatusComentary { get; set; }

        public string HashPagSeguroPlan { get; set; }
    }


    public class SubscriptionModel : BaseModel
    {
        public ObjectId UserId { get; set; }
        public ProductSubscriptionModel Product { get; set; }
        public DateTime Expire { get; set; }
        public SubscriptionStatus Status { get; set; }
    }

    public enum SubscriptionType
    {
        [Description("Semanal")]
        Semanal = 0,

        [Description("Mensal")]
        Mensal = 1,

        [Description("Bimestral")]
        Bimestral = 2,

        [Description("Trimestral")]
        Trimestral = 3,

        [Description("Semestral")]
        Semestral = 4,

        [Description("Anual")]
        Anual = 5,
    }

    public enum SubscriptionStatus
    {
        InProcess = 0,
        Active = 1,
        Expired = 2,
        Canceled = 3,
    }

    public enum ProductStatus
    {
        InProvation = 0,
        Active = 1,
        Canceled = 3,
    }

    public class FriendlyEnumMethods
    {
        // }
        public static string GetFriendlyEnums()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (SubscriptionType colorEnum in Enum.GetValues(typeof(SubscriptionType)))
            {
                stringBuilder.Append(colorEnum.GetDescription() + "|");
            }
            return stringBuilder.ToString();
        }
        //{ autofold
    }
}
