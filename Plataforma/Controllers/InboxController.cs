using ImageResizer;
using Mongo.BSN;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    [Authorize]
    public partial class DatingController : Controller
    {
        InboxBSN _InboxBSN = new InboxBSN();
        UserBSN _UsusarioBSN = new UserBSN();
        LogImageBSN _logImage = new LogImageBSN();


        public ActionResult Inbox()
        {
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var user = Usuario.GetUserByUsuario(modelUsuario);

            var ChatRoomsCurrentUser = _chatBSN.GetListChatRoomByUser(user.Id);

            List<dynamic> ChatsRooms = new List<dynamic>();

            if (ChatRoomsCurrentUser != null)
            {
                foreach (var item in ChatRoomsCurrentUser)
                {
                    ObjectId consultarUsuario;
                    if (item.ParticipantOne == user.Id)
                    {
                        consultarUsuario = item.ParticipantTwo;
                    }
                    else
                    {
                        consultarUsuario = item.ParticipantOne;
                    }

                    var usuario = new UserModel();

                    if (!user.Adm)
                    {
                        usuario = UsuarioHelper.GetUsuarioAtivoByObjetcID(consultarUsuario);
                    }
                    else
                    {
                        usuario = UsuarioHelper.GetUsuarioByObjetcID(consultarUsuario);
                    }

                    if (usuario != null)
                    {
                        dynamic chatRoom = new ExpandoObject();
                        chatRoom.DateLastInteraction = item.DateLastInteraction;
                        chatRoom.Id = item.Id;
                        chatRoom.UserId = usuario.Id;
                        chatRoom.imagemPerfil = usuario.imagemPerfil;
                        chatRoom.Usuario = usuario.Usuario;
                        chatRoom.LastMessage = item.LastMessage;

                        ChatsRooms.Add(chatRoom);
                    }
                    
                    
                }
            }

            ViewBag.ChatsRooms = ChatsRooms.OrderByDescending(c => c.DateLastInteraction.Ticks).ToList();

            return View();
        }

        public ActionResult MensagensPartial(string inboxID, string urlImage, string NomeTo)
        {
            var name = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UsuarioHelper.GetUsuarioByString(name);
            var RoomId = ObjectId.Parse(inboxID);

            var chatMessages = new List<ChatMessagesModel>();

            // Quanod o usuárioé antigo
            try
            {
                chatMessages = _chatBSN.GetListChatRoomMassagesByUser(RoomId);
                _chatBSN.ReadChatRoom(RoomId);
                _chatBSN.ReadChatMessage(RoomId, user.Id);
            }
            catch (Exception ex)
            {

            }

            UsuarioHelper.AddLastInteraction(user);

            ViewBag.urlImage = urlImage;
            ViewBag.NomeTo = NomeTo;

            ViewBag.Me = user.Id;
            ViewBag.Messages = chatMessages;

            return PartialView();
        }

        public ActionResult ArquivarMensagem(string idUsuarioSelecionado)
        {
            var name = System.Web.HttpContext.Current.User.Identity.Name;
            var user = UsuarioHelper.GetUsuarioByString(name);

            // Quanod o usuárioé antigo
            try
            {
                user.Inboxes.Where(i => i.ParaUsuarioID.ToString() == idUsuarioSelecionado.Trim()).FirstOrDefault().isActive = false;
            }
            catch (Exception ex)
            {

            }

            UsuarioHelper.AddLastInteraction(user);

            return View();
        }

        [HttpPost]
        public string UploadImage(HttpPostedFileBase File)
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

                        var i = new ImageJob(File, "~/ImagesChat/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        try
                        {
                            _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.ChatMessage);
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

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}