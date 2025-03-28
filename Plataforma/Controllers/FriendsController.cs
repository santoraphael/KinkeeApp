using Mongo.BSN;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class FriendsController : Controller
    {
        NotificationBSN _notificationsBSN = new NotificationBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        UserBSN _userBSN = new UserBSN();


        // GET: Notification
        public ActionResult _Index()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            var listaRequests = _relationShipBSN.GetListRelationShipByUserID(usuarioLogado.Id, StatusRelationShip.Accepted);
            //var listaRequests = _relationShipBSN.GetLastFriendsRequestList(usuarioLogado.Id.ToString(), StatusRelationShip.Pending);

            List<Dictionary<string, object>> listaDicionario = new List<Dictionary<string, object>>();


            foreach (var item in listaRequests.OrderByDescending(u => u.DateCreate).ToList())
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                var retornoUsuario = new UserModel();
                if (usuarioLogado.Id == item.UserId)
                {
                    retornoUsuario = UsuarioHelper.GetUsuarioAtivoByObjetcID(item.FriendId);
                }
                else
                {
                    retornoUsuario = UsuarioHelper.GetUsuarioAtivoByObjetcID(item.UserId);
                }

                if(retornoUsuario != null)
                {
                    //request.Add("READ", item.Read);
                    request.Add("ID_RELATIONSHIP", item.Id.ToString());
                    request.Add("ID_USUARIO", retornoUsuario.Id.ToString());
                    request.Add("NOME_USUARIO", retornoUsuario.Usuario);
                    request.Add("URL_IMAGEM_PERFIL", retornoUsuario.imagemPerfil);

                    if (retornoUsuario.Endereco == null)
                    {
                        request.Add("CIDADE", "Não definido");
                    }
                    else
                    {
                        request.Add("CIDADE", retornoUsuario.Endereco.Cidade);
                    }

                    request.Add("AMIZADE", "Amigos");

                    listaDicionario.Add(request);
                }
            }

            ViewBag.LastsNotifications = listaDicionario;
            ViewBag.FriendsCount = listaRequests.Count;
            return View();
        }

        public ActionResult _Find()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            var list = _relationShipBSN.GetListUserNotFriendShip(usuarioLogado.Id);

            List<Dictionary<string, object>> listaDicionario = new List<Dictionary<string, object>>();
            foreach (var item in list.OrderByDescending(u => u.DateCreate).ToList())
            {
                Dictionary<string, object> request = new Dictionary<string, object>();

                //request.Add("READ", item.Read);
                request.Add("ID_RELATIONSHIP", item.Id.ToString());
                request.Add("ID_USUARIO", item.Id.ToString());
                request.Add("NOME_USUARIO", item.Usuario);
                request.Add("URL_IMAGEM_PERFIL", item.imagemPerfil);

                if (item.Endereco == null)
                {
                    request.Add("CIDADE", "Não definido");
                }
                else
                {
                    request.Add("CIDADE", item.Endereco.Cidade);
                }

                request.Add("AMIZADE", "Amigos");

                listaDicionario.Add(request);
            }

            ViewBag.ListUsers = listaDicionario;

            return View();
        }
    }
}