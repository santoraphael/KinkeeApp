using System.Web.Mvc;
using System.Web.Routing;

namespace Plataforma
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "CodigoCadastro",
            //    url: "Cadastro/{codigoConvite}",
            //    defaults: new { controller = "Home", action = "Cadastro", codigoConvite = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Cadastro",
                url: "{controller}/{action}/{codigoConvite}",
                defaults: new { controller = "Dating", action = "Perfil", codigoConvite = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Profile",
                url: "{controller}/{action}/{username}",
                defaults: new { controller = "Dating", action = "Perfil", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "MagicLInk",
                url: "{controller}/{action}/{userEmail}/{profileId}",
                defaults: new { controller = "Mobile", action = "MagicLInk", userEmail = UrlParameter.Optional, profileId = UrlParameter.Optional }
            );
        }
    }
}
