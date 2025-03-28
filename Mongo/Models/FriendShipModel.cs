using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class RelationShipModel : BaseModel
    {
        public ObjectId UserId { get; set; }
        public ObjectId FriendId { get; set; }
        public StatusRelationShip StatusRelationShip { get; set; }
        public ObjectId ActionUserId { get; set; }
        public bool Read { get; set; }
    }

    public class UpdateFriend : BaseModel
    {
        public ObjectId UpdatePost { get; set; }
        public ObjectId UserUpdatedId { get; set; }
    }

    public enum StatusRelationShip
    {
        All = 0,
        Pending = 1,
        Accepted = 2,
        Declined = 3,
        Blocked = 4,
    }
}
