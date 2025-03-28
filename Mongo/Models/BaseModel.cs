using System;
using MongoDB.Bson;

namespace Mongo.Models
{
    public class BaseModel
    {
        public ObjectId Id { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateLastInteraction { get; set; }
        public bool? isActive { get; set; }
    }
}
