using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;

namespace Mongo.DAL
{
    public class LogErrorDAL
    {
        private readonly Connection db = new Connection();
        private readonly string LogError = "LogError";

        public bool InsertLogErro(LogErrorModel logError)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<LogErrorModel>(LogError);
            try
            {
                collection.InsertOne(logError);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
