using Mongo.BSN;
using Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class BDSMAndFetichesController : Controller
    {
        NotificationBSN _notificationsBSN = new NotificationBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        UserBSN _userBSN = new UserBSN();

        public ActionResult Index()
        {
            ViewBag.Title = "Relacionamento Sugar";
            ViewBag.IsBusca = "0";
            return View();
        }

        public ActionResult LoadProfiles(int pageIndex, int pageSize, string url)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            usuarioLogado.DateLastInteraction = DateTime.Now;
            _userBSN.EditarUsuario(usuarioLogado);


            List<UserModel> listaUsuarios = new List<UserModel>();
            List<UserModel> listaTopUsuarios = new List<UserModel>();

            ViewBag.TopUsers = false;

            List<UserModel> query = new List<UserModel>();

            string Genero = "";

            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
            }
            else
            {
                Genero = "Homem";
            }

            query = (from c in _userBSN.GetListUserByGenero(Genero) select c)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize).ToList();


            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult _DestaquesPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            List<UserModel> listaUsuarios = new List<UserModel>();
            List<UserModel> Aux = new List<UserModel>();


            //ViewBag.PromotionalCode = usuarioLogado.PromotionalCode;

            string Genero = "";

            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
                ViewBag.Subtitulo = "Sugar Babies";
            }
            else
            {
                Genero = "Homem";
                ViewBag.Subtitulo = "Sugar Daddies";
            }


            listaUsuarios = _userBSN.GetListDestaquesSugar(Genero).Take(10).ToList();

            foreach (var item in listaUsuarios)
            {
                UserModel userAlter = new UserModel();
                userAlter = item;

                if (!string.IsNullOrEmpty(item.imagemPerfil))
                {
                    userAlter.imagemPerfil = item.imagemPerfil.Replace("\\", "/");
                }

                Aux.Add(userAlter);
            }

            ViewBag.Destaques = Aux;


            return View();
        }

        public ActionResult _SugestaoPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _userBSN.GetUserByUsuario(u);
            List<UserModel> listaUsuarios = new List<UserModel>();


            ViewBag.PromotionalCode = usuarioLogado.CodigoConvite;

            string Genero = "";

            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
                ViewBag.Subtitulo = "Novas";
            }
            else
            {
                Genero = "Homem";
                ViewBag.Subtitulo = "Novos";
            }


            listaUsuarios = _userBSN.GetListStoriesSugar(Genero).Take(10).ToList();

            ViewBag.Destaques = listaUsuarios;

            return View();
        }
    }
}