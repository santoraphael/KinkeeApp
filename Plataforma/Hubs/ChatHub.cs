using Microsoft.AspNet.SignalR;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Plataforma.Hubs
{
    public class ChatHub : Hub
    {
        UserBSN Usuario = new UserBSN();
        ChatBSN _chatBSN = new ChatBSN();

        //ConnectionsBSN Connect = new ConnectionsBSN();
        static HashSet<string> CurrentConnections = new HashSet<string>();
        static List<ConnectionModel> SignalRUsers = new List<ConnectionModel>();

        [HttpPost]
        public void Send(string de, string para, string message, string urlImage)
        {
            para = para.Replace(" ", "");
            var mensagemPara = UsuarioHelper.GetUsuarioAtivoENaoAtivo(para);

            if (mensagemPara == null)
            {
                Clients.Caller.showErrorMessage("Could not find that user.");
            }
            else
            {
                if (mensagemPara.Connections == null)
                {
                    Clients.Caller.showErrorMessage("The user is no longer connected.");
                }

                try
                {
                    var mensagemDe = UsuarioHelper.GetUsuario(de);
                    var ChatRoom = _chatBSN.GetUniqueChatRoomByParticipants(mensagemDe.Id, mensagemPara.Id);

                    //CommonHelper.SalvarImagemChat(file, mensagemDe.Usuario);

                    _chatBSN.InsertMessage(ChatRoom.Id, mensagemDe.Id, message, false);

                    var connection = SignalRUsers.Where(c => c.UserName == mensagemPara.Usuario).FirstOrDefault();

                    if (connection != null)
                    {
                        Clients.Client(connection.ConnectionID).broadcastMessage(mensagemPara, message, urlImage, mensagemDe.Id.ToString());
                    }
                    else
                    {
                        EnviarNotificacao(mensagemDe, mensagemPara);
                    }
                }
                catch
                {

                }
                
            }
        }

        public void SendChatMessage(string who, string message)
        {
            var name = Context.User.Identity.Name;

            var user = UsuarioHelper.GetUsuario(who);

            if (user == null)
            {
                Clients.Caller.showErrorMessage("Could not find that user.");
            }
            else
            {
                if (user.Connections == null)
                {
                    Clients.Caller.showErrorMessage("The user is no longer connected.");
                }
                else
                {
                    //var connection = user.Connections.FirstOrDefault();
                    var connection = SignalRUsers.Where(c => c.UserName == who).FirstOrDefault();
                    if (connection != null)
                    {
                        Clients.Client(connection.ConnectionID).broadcastMessage(name, message);
                    }
                }
            }
        }

        public void Connect()
        {

        }

        public override Task OnConnected()
        {
            var name = Context.User.Identity.Name;
            var id = Context.ConnectionId;

            if (name != null)
            {
                if (SignalRUsers.Count(x => x.ConnectionID == id) == 0)
                {
                    SignalRUsers.Add(new ConnectionModel { ConnectionID = id, UserName = name });
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var item = SignalRUsers.FirstOrDefault(x => x.ConnectionID == Context.ConnectionId);
            if (item != null)
            {
                SignalRUsers.Remove(item);
            }

            return base.OnDisconnected(stopCalled);
        }

        public bool EnviarNotificacao(UserModel varUsuarioLogado, UserModel varUsuario)
        {

            bool retorno = false;
            try
            {
                if (true)
                {
                    Plataforma.Notifications not = new Plataforma.Notifications();
                    List<string> PlayerIds = new List<string>();

                    if (varUsuario.Players == null)
                    {
                        varUsuario.Players = PlayerIds;
                    }

                    foreach (var player in varUsuario.Players)
                    {
                        if (!String.IsNullOrEmpty(player))
                        {
                            PlayerIds.Add(player);
                        }
                    }

                    if (PlayerIds.Count > 0)
                    {
                        not.CreateNotification(PlayerIds, varUsuarioLogado.Usuario+" te mandou uma mensagem.", "https://app.kinkeesugar.com/Dating/Inbox", "Nova mensagem");
                    }


                    SendEmailAddress to = new SendEmailAddress();
                    to.Email = varUsuario.Email;
                    to.Nome = varUsuario.Name;

                    ProcessaEmails.SendMailMensagemRecebida(to, varUsuarioLogado.Usuario, varUsuarioLogado.imagemPerfil);
                }
            }
            catch
            {

            }

            return retorno;
        }


        //public override Task OnDisconnected(bool stopCalled)
        //{
        //    try
        //    {
        //        var connection = CurrentConnections.FirstOrDefault(x => x == Context.ConnectionId);

        //        if (connection != null)
        //        {
        //            CurrentConnections.Remove(connection);
        //        }

        //        //var name = Context.User.Identity.Name;
        //        //var user = UsuarioHelper.GetUsuario(name);


        //        //List<ConnectionModel> connection = user.Connections.ToList();
        //        //var conn = connection.Find(c => c.ConnectionID == Context.ConnectionId);
        //        //connection.Remove(conn);
        //        //conn.Connected = false;

        //        //connection.Add(conn);

        //        //user.Connections = connection;

        //        //Usuario.EditarUsuario(user);
        //    }
        //    catch
        //    {

        //    }

        //    return base.OnDisconnected(stopCalled);
        //    //return base.OnDisconnected(stopCalled);
        //}

        public List<string> GetAllActiveConnections()
        {
            return CurrentConnections.ToList();
        }

        public async Task<string> GetActiveConnectionByUserId(string UserName)
        {
            var connection = SignalRUsers.Select(c => c.UserName == UserName).FirstOrDefault();

            if (connection != false)
            {
                return "true";
            }
            else
            {
                return "false";
            }
        }

        public async Task<bool> GetActiveConnectionByUsername(string UserName)
        {
            var connection = SignalRUsers.Select(c => c.UserName == UserName).FirstOrDefault();

            return connection;
        }
    }
}