using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class ErrorPagesController : Controller
    {
        // GET: Perfil
        public ActionResult Oops()
        {
            return View();
        }
    }
}