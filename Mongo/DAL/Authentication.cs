using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;

namespace Mongo.DAL
{
    public class Authentication
    {
        Connection db = new Connection();

        //public bool AuthenticateUser(string login, string password, bool persistiCookie)
        //{
        //    MongoDatabase _database = db.ConnectServer();
        //    MongoCollection<BsonDocument> collection = _database.GetCollection<BsonDocument>("usuarios");
        //    UserModel returnUsuario = new UserModel();


        //    UserModel usuario = collection.FirstOrDefault .SingleOrDefault(q => login.Equals(q.Email) && password.Equals(q.PasswordHash) && q.Active == true);

        //    if (usuario == null)
        //    {
        //        return false;
        //    }

        //    usuario.DateLastLogin = DateTime.Now;
        //    db.SaveChanges();
            

        //    //FormsAuthentication.SetAuthCookie(login, persistiCookie);
        //    return true;
        //}
    }
}
