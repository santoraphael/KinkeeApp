using Mongo.DAL;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.BSN
{
    public class NotificationBSN
    {
        NotificationDAL _notificationDAL = new NotificationDAL();

        public bool GerarNovaNotificacao(dynamic AddtionalData,
                                            Notificationtype notificationtype, 
                                            ObjectId RecipientUserId, 
                                            ObjectId SenderUserId, 
                                            List<NotificationSendTo> NotificationSendTo)
        {
            NotificationModel newNotification = new NotificationModel();

            newNotification.Notificationtype = notificationtype;
            newNotification.DateCreate = DateTime.Now;
            newNotification.AddtionalData = AddtionalData;
            newNotification.Deleted = false;
            newNotification.Read = false;
            newNotification.RecipientUserId = RecipientUserId;
            newNotification.SenderUserId = SenderUserId;
            newNotification.NotificationSendTo = NotificationSendTo;

            return InsertNotification(newNotification);
        }

        public bool InsertNotification(NotificationModel notificationModel)
        {
            return _notificationDAL.InsertNotification(notificationModel);
        }

        public List<NotificationModel> GetNewsNotificationByUser(string idUsuario, Notificationtype notificationTypeModel)
        {
            List<NotificationModel> notifications = new List<NotificationModel>();
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _notificationDAL.GetNewsNotificationByUser(userId, notificationTypeModel);
            }
            catch
            {

            }

            return notifications;
        }

        public List<NotificationModel> GetLastsNotificationByUser(string idUsuario, Notificationtype notificationTypeModel)
        {
            List<NotificationModel> notifications = new List<NotificationModel>();
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _notificationDAL.GetLastsNotificationByUser(userId, notificationTypeModel);
            }
            catch
            {

            }

            return notifications;
        }

        public int GetCountNotificationByUser(string idUsuario, Notificationtype notificationTypeModel)
        {
            var notifications = 0;
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _notificationDAL.GetCountNotificationByUser(userId, notificationTypeModel);
            }
            catch
            {

            }

            return notifications;
        }

        public void UpdateReadNotifications(string idUsuario, Notificationtype notificationTypeModel)
        {
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                _notificationDAL.UpdateReadNotifications(userId, notificationTypeModel);
            }
            catch
            {

            }
        }

        public bool AddLikeNotification(UserModel From, UserModel To, string publicacaoID)
        {
            //var returno = UsuarioHelper.GetUsuarioByObjetcID(From);

            Dictionary<string, string> valores = new Dictionary<string, string>();
            valores.Add("URL_DESTINO", "/dating/post/" + publicacaoID);
            valores.Add("ALT_IMAGEM", From.Usuario);
            valores.Add("URL_IMAGEM", From.imagemPerfil);
            valores.Add("SUBTITULO", From.Usuario + " Curtiu sua publicação");

            valores.Add("DESCRICAO", "");


            dynamic dados = valores;
            List<NotificationSendTo> notificationSends = new List<NotificationSendTo>();

            notificationSends.Add(NotificationSendTo.Site);
            notificationSends.Add(NotificationSendTo.Push);
            notificationSends.Add(NotificationSendTo.Email);

            return GerarNovaNotificacao(dados, Notificationtype.NotificacaoGeral, To.Id, From.Id, notificationSends);
        }

        public bool ComentarioPublicacaoNotification(UserModel From, UserModel To, string Comentario, string publicacaoID)
        {
            //var returno = UsuarioHelper.GetUsuarioByObjetcID(From);

            Dictionary<string, string> valores = new Dictionary<string, string>();
            valores.Add("URL_DESTINO", "dating/post/" + publicacaoID);
            valores.Add("ALT_IMAGEM", From.Usuario);
            valores.Add("URL_IMAGEM", From.imagemPerfil);
            valores.Add("SUBTITULO", From.Usuario + " Comentou sua publicação");

            valores.Add("DESCRICAO", Comentario);


            dynamic dados = valores;
            List<NotificationSendTo> notificationSends = new List<NotificationSendTo>();

            notificationSends.Add(NotificationSendTo.Site);
            notificationSends.Add(NotificationSendTo.Push);
            notificationSends.Add(NotificationSendTo.Email);


            SendEmailAddress to = new SendEmailAddress();
            to.Email = To.Email;
            to.Nome = To.Name;
            ProcessaEmails.SendMailComentarioPublicacao(to, From.Name, "dating/post/" + publicacaoID);

            return GerarNovaNotificacao(dados, Notificationtype.NotificacaoGeral, To.Id, From.Id, notificationSends);
        }
    }
}