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
    public class LogErrorModel
    {
        public string NomeUsuario{ get; set; }
        public string Exception { get; set; }
        public string Comentario{ get; set; }
    }
}
