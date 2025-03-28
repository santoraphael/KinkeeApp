using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class CategoryModel : BaseModel
    {
        public string Name { get; set; }
    }

    public class ProductModel : BaseModel
    {
        public ObjectId CategoryId { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Description { get; set; }
        public double Prince { get; set; }
        public int Amount { get; set; }
        public ObjectId ActionUserId { get; set; }
    }

    public class BagModel : BaseModel
    {
        public ObjectId UserId { get; set; }
        public List<ProductModel> ProductsList { get; set; }
        public StatusBag StatusBag { get; set; }
    }

    public class OrderModel : BaseModel
    {
        public ObjectId UserPayerId { get; set; }
        public BagModel Bag { get; set; }
        public StatusOrder StatusOrder { get; set; }
    }

    public enum StatusBag
    {
        Open = 0,
        Close = 1,
        Abondon = 2,
    }

    public enum StatusOrder
    {
        EmProcessamento = 0,
        PagamentoAceito = 1,
        PagamentoRecusado = 2,
        Cancelada = 3,
        Finalizada = 3,
    }
}
