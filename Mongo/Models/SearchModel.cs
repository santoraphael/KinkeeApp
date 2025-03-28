using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class BuscarModel
    {
        public BuscarModel(){
            ListaUsuarios = new List<UserModel>();
        }


        public List<UserModel> ListaUsuarios { get; set; }
    }
}
