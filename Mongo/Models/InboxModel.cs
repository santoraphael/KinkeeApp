﻿using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class InboxModel : BaseModel
    {
        public ICollection<MessageModel> Messages { get; set; }
    }
}
