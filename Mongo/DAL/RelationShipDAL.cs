using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.BSN
{
    public class RelationShipDAL
    {
        private readonly Connection db = new Connection();
        private readonly string CollectionRelationShip = "Users.RelationShip";
        private const string CollectionUsersBooking = "Users.Booking";

        public bool InsertRelationShip(RelationShipModel relationShipModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            bool retorno;
            try
            {
                collection.InsertOne(relationShipModel);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public RelationShipModel GetRelationShip(ObjectId UserId, ObjectId FriendId, StatusRelationShip statusRelationShip)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            RelationShipModel query;
            if (statusRelationShip == StatusRelationShip.All)
            {
                query = collection.AsQueryable()
                                  .FirstOrDefault(n =>
                                    (n.UserId == UserId && n.FriendId == FriendId) ||
                                    (n.UserId == FriendId && n.FriendId == UserId));
            }
            else
            {
                query = collection.AsQueryable()
                                  .FirstOrDefault(n =>
                                    ((n.UserId == UserId && n.FriendId == FriendId) ||
                                     (n.UserId == FriendId && n.FriendId == UserId)) &&
                                     n.StatusRelationShip == statusRelationShip);
            }

            return query;
        }

        public List<RelationShipModel> GetListRelationShipByUserID(ObjectId UserId, StatusRelationShip statusRelationShip)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var query = collection.AsQueryable()
                                  .Where(n => (n.UserId == UserId || n.FriendId == UserId) &&
                                              n.StatusRelationShip == statusRelationShip)
                                  .OrderByDescending(n => n.DateCreate)
                                  .ToList();
            return query;
        }

        public bool AlterRelationShip(RelationShipModel relationShipModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            bool retorno;
            try
            {
                var filter = Builders<RelationShipModel>.Filter.Eq(x => x.Id, relationShipModel.Id);
                collection.ReplaceOne(filter, relationShipModel, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public List<RelationShipModel> GetLastFriendsRequestList(ObjectId UserId, StatusRelationShip statusRelationShip)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var query = collection.AsQueryable()
                                  .Where(e => (e.UserId == UserId || e.FriendId == UserId) &&
                                              e.StatusRelationShip == statusRelationShip)
                                  .OrderByDescending(n => n.DateCreate)
                                  .Take(5)
                                  .ToList();
            return query;
        }

        public List<RelationShipModel> GetLastFriendsRequestListNotification(ObjectId UserId, StatusRelationShip statusRelationShip)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var query = collection.AsQueryable()
                                  .Where(e => (e.UserId == UserId || e.FriendId == UserId) &&
                                              e.ActionUserId != UserId &&
                                              e.StatusRelationShip == statusRelationShip)
                                  .OrderByDescending(n => n.DateCreate)
                                  .Take(5)
                                  .ToList();
            return query;
        }

        public List<RelationShipModel> GetFriendsRequestList(ObjectId UserId, StatusRelationShip statusRelationShip)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var query = collection.AsQueryable()
                                  .Where(e => (e.UserId == UserId || e.FriendId == UserId) &&
                                              e.ActionUserId != UserId &&
                                              e.StatusRelationShip == statusRelationShip)
                                  .OrderByDescending(n => n.DateCreate)
                                  .ToList();
            return query;
        }

        public void UpdateReadFriedRequestNotifications(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var filter = Builders<RelationShipModel>.Filter.Where(e =>
                (e.FriendId == usuarioId || e.UserId == usuarioId) &&
                e.ActionUserId != usuarioId &&
                e.Read == false);

            var update = Builders<RelationShipModel>.Update.Set(e => e.Read, true);
            collection.UpdateMany(filter, update);
        }

        public bool UpdateResponseFriedRequest(ObjectId request, StatusRelationShip statusRelationShip, ObjectId actionUserId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var filter = Builders<RelationShipModel>.Filter.Where(e =>
                (e.UserId == actionUserId && e.FriendId == request) ||
                (e.UserId == request && e.FriendId == actionUserId));

            var readVal = statusRelationShip != StatusRelationShip.Pending;

            var update = Builders<RelationShipModel>.Update
                .Set(e => e.StatusRelationShip, statusRelationShip)
                .Set(e => e.ActionUserId, actionUserId)
                .Set(e => e.Read, readVal);

            bool retorno;
            try
            {
                collection.UpdateOne(filter, update);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public int GetCountRequestNotificationsByUser(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<RelationShipModel>(CollectionRelationShip);

            var count = collection.AsQueryable()
                                  .Count(n => (n.FriendId == usuarioId || n.UserId == usuarioId) &&
                                              n.ActionUserId != usuarioId &&
                                              n.Read == false);

            return count;
        }
    }
}
