using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Mongo.BSN
{
    public class LogImageDAL
    {
        private readonly Connection db = new Connection();
        private readonly string CollectionLogImageDAL = "Chat.LogImageDAL";

        public void InsertLogImage(ImageChatUserLogModel image)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<ImageChatUserLogModel>(CollectionLogImageDAL);
            try
            {
                collection.InsertOne(image);
            }
            catch
            {
            }
        }
    }
}
