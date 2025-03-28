using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL
{
    public class NotificationDAL
    {
        private readonly Connection db = new Connection();
        private readonly string CollectionNotification = "Users.Notification";
        private readonly string CollectionNotificationSetting = "Users.Notification.Setting";

        #region NOTIFICATION

        public bool InsertNotification(NotificationModel notificationModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationModel>(CollectionNotification);

            bool retorno;
            try
            {
                collection.InsertOne(notificationModel);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public List<NotificationModel> GetNotificationByUser(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationModel>(CollectionNotification);

            var query = collection.AsQueryable()
                                  .Where(n => n.RecipientUserId == usuarioId)
                                  .ToList();
            return query;
        }

        public List<NotificationModel> GetNewsNotificationByUser(ObjectId usuarioId, Notificationtype notificationTypeModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationModel>(CollectionNotification);

            var query = collection.AsQueryable()
                                  .Where(n => n.RecipientUserId == usuarioId
                                           && n.Notificationtype == notificationTypeModel
                                           && n.Read == false)
                                  .OrderByDescending(u => u.DateCreate)
                                  .ToList();

            return query;
        }

        public List<NotificationModel> GetLastsNotificationByUser(ObjectId usuarioId, Notificationtype notificationTypeModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationModel>(CollectionNotification);

            var query = collection.AsQueryable()
                                  .Where(n => n.RecipientUserId == usuarioId
                                           && n.Notificationtype == notificationTypeModel
                                           && n.Read == true)
                                  .OrderByDescending(u => u.DateCreate)
                                  .Take(5)
                                  .ToList();

            return query;
        }

        public int GetCountNotificationByUser(ObjectId usuarioId, Notificationtype notificationTypeModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationModel>(CollectionNotification);

            var count = collection.AsQueryable()
                                  .Count(n => n.RecipientUserId == usuarioId
                                           && n.Notificationtype == notificationTypeModel
                                           && n.Read == false);

            return count;
        }

        public void UpdateReadNotifications(ObjectId usuarioId, Notificationtype notificationTypeModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationModel>(CollectionNotification);

            var filter = Builders<NotificationModel>.Filter.And(
                Builders<NotificationModel>.Filter.Eq(e => e.RecipientUserId, usuarioId),
                Builders<NotificationModel>.Filter.Eq(e => e.Notificationtype, notificationTypeModel)
            );
            var update = Builders<NotificationModel>.Update.Set(e => e.Read, true);

            collection.UpdateMany(filter, update);
        }

        #endregion

        #region NOTIFICATION SETTINGS

        public bool InsertNotificationSetings(NotificationSettingModel notificationSettingModel)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationSettingModel>(CollectionNotificationSetting);

            bool retorno;
            try
            {
                collection.InsertOne(notificationSettingModel);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public List<NotificationSettingModel> GetNotificationSetting(ObjectId usuarioId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<NotificationSettingModel>(CollectionNotificationSetting);

            var query = collection.AsQueryable()
                                  .Where(n => n.UserId == usuarioId)
                                  .ToList();
            return query;
        }

        #endregion
    }
}
