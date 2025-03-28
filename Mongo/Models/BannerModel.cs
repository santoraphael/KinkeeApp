using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class BannerModel : BaseModel
    {
        public string h1Text { get; set; }
        public string pText { get; set; }
        public ICollection<String> vistoPor { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }
        public Int32 heightSize { get; set; }
        public string Genero { get; set; }
        public bool MostraBotao { get; set; }
        public string TextoBotao { get; set; }
        public string UrlBotao { get; set; }
        public string Preco { get; set; }
    }
}
