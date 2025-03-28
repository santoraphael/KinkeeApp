
using System;
using System.Web;
using System.Web.Security;
using System.Linq;

namespace Mongo.Infrastruture.Helper
{
    public class AuthenticationHelper
    {
        //public static bool AuthenticateUser(string login, string password, bool persistiCookie)
        //{
        //    using (var db = new KinkeeEntities())
        //    {
        //        User usuario = db.Users.SingleOrDefault(q => login.Equals(q.Email) && password.Equals(q.PasswordHash) && q.Active == true);

        //        if (usuario == null)
        //        {
        //            return false;
        //        }

        //        usuario.DateLastLogin = DateTime.Now;
        //        db.SaveChanges();
        //    }

        //    FormsAuthentication.SetAuthCookie(login, persistiCookie);
        //    return true;
        //}

        //public static User GetAuthenticatedUser()
        //{
        //    using (var db = new KinkeeEntities())
        //    {
        //        string email = HttpContext.Current.User.Identity.Name;

        //        if (String.IsNullOrEmpty(email))
        //        {
        //            return null;
        //        }

        //        return db.Users.FirstOrDefault(q => email.Equals(q.Email));
        //    }
        //}

        //public static void LogOff()
        //{
        //    FormsAuthentication.SignOut();
        //}
    }
}