//using Hangfire;
//using Hangfire.Mongo;
//using Hangfire.Mongo.Migration.Strategies;
//using Hangfire.Mongo.Migration.Strategies.Backup;
//using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo
{
    public class Program
    {
        public static void Main()
        {

            //var options = new MongoStorageOptions
            //{
            //    MigrationOptions = new MongoMigrationOptions
            //    {
            //        MigrationStrategy = new DropMongoMigrationStrategy(),
            //        BackupStrategy = new NoneMongoBackupStrategy()
            //    }
            //};
            //var mongoStorage = new MongoStorage(
            //                MongoClientSettings.FromConnectionString("mongodb://kinkee01:R330p908@mongodb.kinkeesugar.com/?authSource=kinkee01"),
            //                "kinkee01", // database name
            //                options);

            //using (new BackgroundJobServer(mongoStorage))
            //{
                
            //}

            // Run mongoDb
            // CMD at Administration C:\mongodb\bin\mongod.exe --dbpath C:\data\db

            //var connectionString = "mongodb://localhost";
            //var client = new MongoClient(connectionString);
            //var server = client.GetServer();
            //var database = server.GetDatabase("dodoit_db");
            //var collection = database.GetCollection<Entity>("entities");

            //var entity = new Entity { Name = "Tom" };

            ////string json = JsonConvert.SerializeObject(entity);

            //collection.Insert(entity);
            //var id = entity.Id;

            //var query = Query<Entity>.EQ(e => e.Id, id);
            //entity = collection.FindOne(query);

            //entity.Name = "Dick";
            //collection.Save(entity);

            //var update = Update<Entity>.Set(e => e.Name, "Harry");
            //collection.Update(query, update);

            //collection.Remove(query);
        }
    }
}
