using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Mongo.BSN
{
    public class LogImageBSN
    {
        private readonly LogImageDAL _logImageDAL = new LogImageDAL();

        public void InsertLogImage(ObjectId senderId, string imageUrl, TypeImageSend typeImageSend)
        {
            var LogImage = new ImageChatUserLogModel
            {
                DateCreate = DateTime.Now,
                DateLastInteraction = DateTime.Now,
                SenderId = senderId,
                ImageUrl = imageUrl,
                TypeImageSend = typeImageSend
            };

            _logImageDAL.InsertLogImage(LogImage);
        }
    }
}
