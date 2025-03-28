using ImageResizer;
using Mongo.BSN;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using MongoDB.Bson;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    [Authorize]
    public partial class DatingController : Controller
    {

        public Font Font { get; private set; }

        // GET: Perfil

        public async Task<ActionResult> _PulsacaoPublicadaPartial(string UsuarioPublicacaoID, string Id)
        {
            var usuarioLogado = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            var Publicacao = PublicacaoHelper.GetPublicacaoByID(Id);
            var usuarioPublicacao = UsuarioHelper.GetUsuarioByObjetcID(ObjectId.Parse(UsuarioPublicacaoID));
            ViewBag.HabilitaEdicaoExclusao = false;
            ViewBag.IdPublicacao = Id;
            ViewBag.TempodePublicacao = RetornaTempoDaInteracao((DateTime)Publicacao.DateCreate);
            ViewBag.KinkeeGoldValid = usuarioLogado.ContaGold;
            if (usuarioLogado.ContaGold)
            {
                ViewBag.KinkeeGold = "True";
            }
            else
            {
                ViewBag.KinkeeGold = "False";
            }


            if (Publicacao.Comentarios != null)
            {
                //Publicacao.Comentarios.Reverse();
                ViewBag.Comentarios = Publicacao.Comentarios;
            }
            else
            {
                List<UsuarioComentarioPublicacaoModel> comentarios = new List<UsuarioComentarioPublicacaoModel>();
                ViewBag.Comentarios = comentarios;
            }



            if (Publicacao.usuarioCurtiuPublicacao != null)
            {
                Publicacao.usuarioCurtiuPublicacao.Reverse();
                ViewBag.usuarioCurtiuPublicacao = Publicacao.usuarioCurtiuPublicacao;
            }
            else
            {
                List<string> imagensPerfilUsuarioLike = new List<string>();
                ViewBag.imagensPerfilUsuarioLike = imagensPerfilUsuarioLike;
            }


            if (Publicacao.Likes < 1)
            {
                ViewBag.likes = 0;
            }
            else
            {
                ViewBag.likes = Publicacao.Likes;
            }

            if (usuarioLogado.Id.ToString() == UsuarioPublicacaoID)
            {
                ViewBag.HabilitaEdicaoExclusao = true;
            }

            if (!String.IsNullOrEmpty(usuarioPublicacao.Name))
            {
                ViewBag.NomeSobrenome = "(" + usuarioPublicacao.Name + " " + usuarioPublicacao.Lastname + ")";
            }
            else
            {
                ViewBag.NomeSobrenome = "";
            }

            ViewBag.Usuario = usuarioPublicacao.Usuario;

            ViewBag.imagemPerfil = usuarioPublicacao.imagemPerfil;
            ViewBag.Publicacao = Publicacao.Publicacao.Replace("width: 100 %; ", "width:100%;");


            return View();
        }

        //public List<String> imagensUsuarioLikes(List<String> imagensUltimosUsuariosLikePost)
        //{
        //    int contador = 0;
        //    List<string>

        //    foreach(var item in imagensUltimosUsuariosLikePost)
        //    {


        //        contador++;
        //    }

        //    return
        //}

        public String RetornaTempoDaInteracao(DateTime dataDaIntaracao)
        {
            TimeSpan TempodePublicacao = DateTime.Now.Subtract(dataDaIntaracao);
            String label = "";


            if (TempodePublicacao.TotalMinutes == 0)
            {
                label = "(Agora mesmo)";
            }
            else if (TempodePublicacao.TotalMinutes > 0 && TempodePublicacao.TotalMinutes < 60)
            {
                label = "(" + TempodePublicacao.TotalMinutes.ToString("#") + " minutos atrás)";
            }

            else if (TempodePublicacao.TotalHours > 0 && TempodePublicacao.TotalHours < 24)
            {
                label = "(" + TempodePublicacao.TotalHours.ToString("#") + " horas atrás)";
            }
            else if (TempodePublicacao.TotalDays > 0 && TempodePublicacao.TotalDays < 30)
            {
                label = "(" + TempodePublicacao.TotalDays.ToString("#") + " dia(s) atrás)";

            }
            else if (TempodePublicacao.TotalDays > 31)
            {
                label = "(Mais de um mês atrás)";
            }

            return label;
        }

        public ActionResult TodasAsPublicacoesAtivas(int pageIndex, int pageSize, int ordenacao)
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            List<PublicacaoViewModel> listPublicacao = new List<PublicacaoViewModel>();

            var query = new List<PublicacaoModel>();

            if(usuarioLogado.Genero == "Homem")
            {
                query = (from c in TodasAsPublicacoesDeMulheres(usuarioLogado.Id) select c)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList();
            }
            else
            {
                query = (from c in PublicacaoHelper.TodasAsPublicacoesAtivas(ordenacao) select c)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList();
            }


            foreach (var item in query)
            {
                PublicacaoViewModel publicacao = new PublicacaoViewModel();
                publicacao.Id = item.Id.ToString();
                publicacao.UsuarioPublicacaoID = item.UsuarioPublicacaoID.ToString();

                listPublicacao.Add(publicacao);
            }


            return Json(listPublicacao.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult PegarPublicacaoPorID(string Id)
        {

            List<PublicacaoViewModel> listPublicacao = new List<PublicacaoViewModel>();

            var query = PublicacaoHelper.GetPublicacaoByID(Id);

            PublicacaoViewModel publicacao = new PublicacaoViewModel();
            publicacao.Id = query.Id.ToString();
            publicacao.UsuarioPublicacaoID = query.UsuarioPublicacaoID.ToString();

            listPublicacao.Add(publicacao);


            return Json(listPublicacao.ToList(), JsonRequestBehavior.AllowGet);
        }
        public List<PublicacaoModel> TodasAsPublicacoesDeAmigosAtivas(ObjectId UserId)
        {
            var friendShipList = _relationShipBSN.GetListRelationShipByUserID(UserId, StatusRelationShip.Accepted);

            List<ObjectId> objectIds = new List<ObjectId>();

            foreach (var item in friendShipList)
            {
                if (item.UserId == UserId)
                {
                    objectIds.Add(item.FriendId);
                }
                else
                {
                    objectIds.Add(item.UserId);
                }
            }

            //Adiciona o proprio ID
            objectIds.Add(UserId);
            var PostsList = PublicacaoHelper.TodasAsPublicacoesDeAmigosAtivas(objectIds);

            return PostsList;
        }

        public List<PublicacaoModel> TodasAsPublicacoesDeMulheres(ObjectId UserId)
        {
            var friendShipList = UsuarioHelper.GetListUserActive(UserId);

            List<ObjectId> objectIds = new List<ObjectId>();

            foreach (var item in friendShipList)
            {
                objectIds.Add(item.Id);
            }

            //Adiciona o proprio ID
            //objectIds.Add(UserId);
            var PostsList = PublicacaoHelper.TodasAsPublicacoesDeMulheresAtivas(objectIds);

            return PostsList;
        }

        public List<PublicacaoModel> TodasAsPublicacoesDeUsuariosAtivas(ObjectId UserId)
        {
            var friendShipList = _relationShipBSN.GetListRelationShipByUserID(UserId, StatusRelationShip.Accepted);
            List<ObjectId> objectIds = new List<ObjectId>();

            foreach (var item in friendShipList)
            {
                if (item.UserId == UserId)
                {
                    objectIds.Add(item.FriendId);
                }
                else
                {
                    objectIds.Add(item.UserId);
                }
            }

            //Adiciona o proprio ID
            objectIds.Add(UserId);
            var PostsList = PublicacaoHelper.TodasAsPublicacoesDeAmigosAtivas(objectIds);

            return PostsList;
        }

        public ActionResult TodasAsPublicacoesAtivasDoUsuario(int pageIndex, int pageSize, int ordenacao)
        {
            List<PublicacaoViewModel> listPublicacao = new List<PublicacaoViewModel>();
            var usuarioLogado = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);

            var query = (from c in PublicacaoHelper.TodasAsPublicacoesAtivasDoUsuario(usuarioLogado.Id, ordenacao) select c)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList();

            foreach (var item in query)
            {
                PublicacaoViewModel publicacao = new PublicacaoViewModel();
                publicacao.Id = item.Id.ToString();
                publicacao.UsuarioPublicacaoID = item.UsuarioPublicacaoID.ToString();

                listPublicacao.Add(publicacao);
            }


            return Json(listPublicacao.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult TodasAsPublicacoesAtivasSemPaginacao()
        {
            List<PublicacaoModel> query = new List<PublicacaoModel>();

            int ordenacao = 0;
            query = (from c in PublicacaoHelper.TodasAsPublicacoesAtivas(ordenacao) select c).ToList();

            return Json(query, JsonRequestBehavior.AllowGet);
        }

        public void RemovePublicacao(string publicacaoID)
        {
            PublicacaoHelper.RemovePublicacao(publicacaoID);
        }

        public void AddLike(string publicacaoID)
        {
            var usuarioLogado = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            PublicacaoHelper.AddLike(publicacaoID, usuarioLogado.imagemPerfil, usuarioLogado.Usuario);


            var Publicacao = PublicacaoHelper.GetPublicacaoByID(publicacaoID);
            var usuarioPublicacao = UsuarioHelper.GetUsuarioByObjetcID(Publicacao.UsuarioPublicacaoID);

            NotificationBSN _notificationBSN = new NotificationBSN();
            _notificationBSN.AddLikeNotification(usuarioLogado, usuarioPublicacao, publicacaoID);

            Plataforma.Notifications not = new Plataforma.Notifications();
            List<string> PlayerIds = new List<string>();
            foreach (var player in usuarioPublicacao.Players)
            {
                if (!String.IsNullOrEmpty(player))
                {
                    PlayerIds.Add(player);
                }
            }

            not.CreateNotification(PlayerIds, Publicacao.Publicacao, "https://app.kinkeesugar.com/dating/post/" + publicacaoID, usuarioLogado.Usuario + " Curtiu sua publicação");
        }

        public ActionResult AddComentario(string publicacaoID, string comentario)
        {
            var usuarioLogado = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            PublicacaoHelper.AddComentario(publicacaoID, usuarioLogado.Usuario, usuarioLogado.imagemPerfil, comentario);

            UsuarioComentarioPublicacaoModel retorno = new UsuarioComentarioPublicacaoModel();

            retorno.imagemPerfilUsuarioComentario = usuarioLogado.imagemPerfil;
            retorno.NomeUsuarioComentario = usuarioLogado.Usuario;
            retorno.Comentario = comentario;


            var Publicacao = PublicacaoHelper.GetPublicacaoByID(publicacaoID);
            var usuarioPublicacao = UsuarioHelper.GetUsuarioByObjetcID(Publicacao.UsuarioPublicacaoID);

            NotificationBSN _notificationBSN = new NotificationBSN();
            _notificationBSN.ComentarioPublicacaoNotification(usuarioLogado, usuarioPublicacao, comentario, publicacaoID);



            Plataforma.Notifications not = new Plataforma.Notifications();
            List<string> PlayerIds = new List<string>();
            foreach (var player in usuarioPublicacao.Players)
            {
                if (!String.IsNullOrEmpty(player))
                {
                    PlayerIds.Add(player);
                }
            }

            not.CreateNotification(PlayerIds, comentario, "https://app.kinkeesugar.com/dating/post/" + publicacaoID, usuarioLogado.Usuario + " Comentou sua publicação");

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public ActionResult _WriteMessagePartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ViewBag.KinkeeGold = usuarioLogado.ContaGold;

            if (!String.IsNullOrEmpty(usuarioLogado.Name))
            {
                ViewBag.UserName = usuarioLogado.Name + " " + usuarioLogado.Lastname;
            }
            else
            {
                ViewBag.UserName = usuarioLogado.Usuario;
            }

            ViewBag.imagemPerfil = usuarioLogado.imagemPerfil;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EnviarPublicacao(string txtPublicacao, PostType postType = PostType.PublicacaoPadrao)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);


            PublicacaoModel publicacao = new PublicacaoModel();
            publicacao.UsuarioPublicacaoID = usuarioLogado.Id;
            publicacao.Publicacao = txtPublicacao;

            publicacao.DateCreate = DateTime.Now;
            publicacao.DateLastInteraction = DateTime.Now;
            publicacao.isActive = true;

            var publicacaoFeita = PublicacaoHelper.AddPublicacao(publicacao);

            return Json(new { publicacaoFeita = publicacaoFeita }, JsonRequestBehavior.AllowGet);
        }


        public string UploadImagePost(HttpPostedFileBase File)
        {
            if (File != null && File.ContentLength > 0)
            {
                try
                {
                    if (IsImage(File))
                    {
                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = Usuario.GetUserByUsuario(modelUsuario);

                        var instructions = new Instructions
                        {
                            Width = 1024,
                            Height = 768,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(File, "~/ImagesPosts/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        try
                        {
                            _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.PostTimeLine);
                        }
                        catch
                        {
                            return newVirtualPath;
                        }

                        return newVirtualPath;
                    }
                    else
                    {
                        return "";
                    }

                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }
        public ActionResult Post()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ViewBag.ConfiguracoesIniciais = usuarioLogado.ConfiguracoesIniciais;

            return View();
        }

    }
}