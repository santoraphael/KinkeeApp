using Mongo.BSN;
using Mongo.Models;
using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Plataforma
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new RequireHttpsAttribute());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }



        protected void Application_Error(object sender, EventArgs e)
        {
            var exception = Server.GetLastError();

            LogErrorBSN logErrorBSN = new LogErrorBSN();
            LogErrorModel logErrorModel = new LogErrorModel();
            logErrorModel.NomeUsuario = System.Web.HttpContext.Current.User.Identity.Name;
            logErrorModel.Exception = exception.Message;
            logErrorModel.Comentario = ""
;
            logErrorBSN.InsertLogErro(logErrorModel);

            //UsuarioHelper.SendSMS(message, "5511989234005");
        }
    }
}
