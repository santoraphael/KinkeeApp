using System.Web.Mvc;

namespace Plataforma.Controllers
{
    [AllowAnonymous]
    public class PagamentoController : Controller
    {
        public ActionResult Pagamento()
        {
            return View();
        }

        public ActionResult _CheckoutPartial()
        {

            return View();
        }
    }
}