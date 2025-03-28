using Mongo.BSN;
using Mongo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MongoDB.Bson;

namespace Plataforma.Controllers
{
    public class ContaDigitalController : Controller
    {
        UserBSN _UserBsn = new UserBSN();
        NotificationBSN _notificationsBSN = new NotificationBSN();
        // GET: ContaDigital
        public ActionResult Index()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ProcessSwitcher processSwitcher = new ProcessSwitcher();
            ViewBag.Saldo = processSwitcher.GetSaldoWallet(usuarioLogado.Id);
            var saldoReal = ViewBag.Saldo * 0.02;
            var valorDescontado = saldoReal * 30 / 100;
            var SaldoFinal = saldoReal - valorDescontado;
            ViewBag.SaldoFinal = SaldoFinal;
            return View();
        }

        public ActionResult Extrato()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ProcessSwitcher processSwitcher = new ProcessSwitcher();
            ICollection<TransactionModel> lista = processSwitcher.GetWalletByDono(usuarioLogado.Id).Transactions;

            ViewBag.Transactions = lista.Reverse();

            return View();
        }

        public ActionResult Transferencias()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ProcessSwitcher processSwitcher = new ProcessSwitcher();
            ViewBag.Saldo = processSwitcher.GetSaldoWallet(usuarioLogado.Id);

            return View();
        }
    }
}