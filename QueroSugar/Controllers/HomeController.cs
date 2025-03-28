using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QueroSugar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _ModalLoginPartial()
        {
           

            return View();
        }

        public ActionResult _ModalRecuperaContaPartial()
        {


            return View();
        }

        public ActionResult _ModalRecuperaPorEmailPartial()
        {
            return View();
        }


        public ActionResult _ModalCadastrarCelularPartial()
        {
            return View();
        }

        public ActionResult _ModalCadastrarEmailPartial()
        {
            return View();
        }

        public ActionResult _ModalCadastrarInformacoesPartial()
        {
            return View();
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}