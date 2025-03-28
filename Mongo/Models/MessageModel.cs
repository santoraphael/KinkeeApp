using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class MessageModel : BaseModel
    {
        public ObjectId FromId { get; set; }
        public ObjectId ToId { get; set; }
        public String Message { get; set; }
    }
}
