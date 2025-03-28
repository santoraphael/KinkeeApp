using System.Web.Mvc;

namespace Plataforma.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        // GET: Perfil
        public ActionResult Index()
        {
            return View();
        }
    }
}