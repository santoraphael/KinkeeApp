using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class InstitucionalController : Controller
    {
        // GET: Perfil
        public ActionResult Sobre()
        {
            return View();
        }

        public ActionResult _SubMenuInstitucionalPartial()
        {
            return View();
        }

        public ActionResult _FooterInstitucionalPartial()
        {
            return View();
        }
    }
}