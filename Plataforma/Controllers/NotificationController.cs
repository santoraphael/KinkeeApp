using Mongo.BSN;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class NotificationController : Controller
    {
        NotificationBSN _notificationsBSN = new NotificationBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        UserBSN _userBSN = new UserBSN();

        // GET: Notification
        public ActionResult Index()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            //ViewBag.NewsNotifications = _notificationsBSN.GetNewsNotificationByUser(usuarioLogado.Id.ToString(), Notificationtype.NotificacaoGeral);
            ViewBag.LastsNotifications = _notificationsBSN.GetLastsNotificationByUser(usuarioLogado.Id.ToString(), Notificationtype.NotificacaoGeral);

            return View();
        }

        public ActionResult _CardFriendNotificationListPartial()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            var listaRequests = _relationShipBSN.GetLastFriendsRequestListNotification(usuarioLogado.Id.ToString(), StatusRelationShip.Pending);
            List<Dictionary<string, object>> listaDicionario = new List<Dictionary<string, object>>();


            foreach (var item in listaRequests)
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                var retornoUsuario = UsuarioHelper.GetUsuarioAtivoByObjetcID(item.ActionUserId);

                request.Add("READ", item.Read);
                request.Add("DATE_CREATE", item.DateCreate);
                request.Add("ID_RELATIONSHIP", item.Id.ToString());
                request.Add("ID_USUARIO", retornoUsuario.Id.ToString());
                request.Add("PRIMEIRO_NOME", retornoUsuario.Name);
                request.Add("ULTIMONOME_NOME", retornoUsuario.Lastname);
                request.Add("NOME_USUARIO", retornoUsuario.Usuario);
                request.Add("URL_IMAGEM_PERFIL", retornoUsuario.imagemPerfil);

                listaDicionario.Add(request);
            }

            ViewBag.LastsNotifications = listaDicionario;

            return View();
        }

        public ActionResult _CardMessageNotificationListPartial()
        {

            return View();
        }

        public ActionResult _CardNotificationListPartial()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            ViewBag.NewsNotifications = _notificationsBSN.GetNewsNotificationByUser(usuarioLogado.Id.ToString(), Notificationtype.NotificacaoGeral);
            ViewBag.LastsNotifications = _notificationsBSN.GetLastsNotificationByUser(usuarioLogado.Id.ToString(), Notificationtype.NotificacaoGeral);

            return View();
        }

        public void SalvarUserID(string userId)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _userBSN.GetUserByUsuario(u);


            if (usuarioLogado.Players == null)
            {
                List<string> play = new List<string>();
                usuarioLogado.Players = play;
            }

            var contains = usuarioLogado.Players.Contains(userId);

            if (!contains)
            {
                usuarioLogado.Players.Add(userId);
                var retorno = UsuarioHelper.SalvarUserID(usuarioLogado);
            }
        }
    }
}