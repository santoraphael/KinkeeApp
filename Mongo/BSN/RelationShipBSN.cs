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
    public class RelationShipBSN
    {
        UserBSN _userBSN = new UserBSN();
        NotificationBSN _notificationBSN = new NotificationBSN();
        RelationShipDAL _relationShipDAL = new RelationShipDAL();

        public bool InsertRelationShip(ObjectId UserId, ObjectId FriendId, StatusRelationShip StatusRelationShip, ObjectId ActionUserId)
        {
            bool retorno = false;

            if(!VerifyRelationShip(UserId, FriendId, StatusRelationShip.All))
            {
                RelationShipModel relationShipModel = new RelationShipModel()
                {
                    DateCreate = DateTime.Now,
                    UserId = UserId,
                    FriendId = FriendId,
                    Read = false,
                    StatusRelationShip = StatusRelationShip.Pending,
                    ActionUserId = UserId,
                };

                retorno = _relationShipDAL.InsertRelationShip(relationShipModel);
            }
            else
            {
                var relationShip = GetRelationShip(UserId, FriendId, StatusRelationShip.All);

                relationShip.DateLastInteraction = DateTime.Now;
                relationShip.StatusRelationShip = StatusRelationShip.Pending;
                relationShip.ActionUserId = UserId;

                retorno = AlterRelationShip(relationShip);
            }

            return retorno;
        }

        public RelationShipModel GetRelationShip(ObjectId UserId, ObjectId FriendId, StatusRelationShip StatusRelationShip)
        {
            return _relationShipDAL.GetRelationShip(UserId, FriendId, StatusRelationShip);
        }

        public List<RelationShipModel> GetListRelationShipByUserID(ObjectId UserId, StatusRelationShip StatusRelationShip)
        {
            return _relationShipDAL.GetListRelationShipByUserID(UserId, StatusRelationShip);
        }

        public bool AlterRelationShip(RelationShipModel relationShipModel)
        {
            relationShipModel.DateLastInteraction = DateTime.Now;
            return _relationShipDAL.AlterRelationShip(relationShipModel);
        }

        public bool VerifyRelationShip(ObjectId UserId, ObjectId FriendId, StatusRelationShip StatusRelationShip)
        {
            var relationShip = GetRelationShip(UserId, FriendId, StatusRelationShip);

            if(relationShip != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<RelationShipModel> GetLastFriendsRequestList(string idUsuario, StatusRelationShip StatusRelationShip)
        {
            List<RelationShipModel> notifications = new List<RelationShipModel>();
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _relationShipDAL.GetLastFriendsRequestList(userId, StatusRelationShip);
            }
            catch
            {

            }

            return notifications;
        }

        public List<RelationShipModel> GetLastFriendsRequestListNotification(string idUsuario, StatusRelationShip StatusRelationShip)
        {
            List<RelationShipModel> notifications = new List<RelationShipModel>();
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _relationShipDAL.GetLastFriendsRequestListNotification(userId, StatusRelationShip);
            }
            catch
            {

            }

            return notifications;
        }

        public List<RelationShipModel> GetFriendsRequestList(string idUsuario, StatusRelationShip StatusRelationShip)
        {
            List<RelationShipModel> notifications = new List<RelationShipModel>();
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _relationShipDAL.GetFriendsRequestList(userId, StatusRelationShip);
            }
            catch
            {

            }

            return notifications;
        }

        public void UpdateReadFriedRequestNotifications(string idUsuario)
        {
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                _relationShipDAL.UpdateReadFriedRequestNotifications(userId);
            }
            catch
            {

            }
        }

        public bool UpdateResponseFriedRequest(String Request, StatusRelationShip StatusRelationShip, String ActionUserId)
        {
            try
            {
                ObjectId FriendRequestId = ObjectId.Parse(Request);
                ObjectId UserId = ObjectId.Parse(ActionUserId);

                if(StatusRelationShip == StatusRelationShip.Accepted)
                {
                    GerarNotificacaoAmizadeAceita(UserId, FriendRequestId);
                }
                

                return _relationShipDAL.UpdateResponseFriedRequest(FriendRequestId, StatusRelationShip, UserId);
            }
            catch
            {
                return false;
            }
        }

        public int GetCountRequestNotificationsByUser(string idUsuario)
        {
            var notifications = 0;
            try
            {
                ObjectId userId = ObjectId.Parse(idUsuario);
                notifications = _relationShipDAL.GetCountRequestNotificationsByUser(userId);
            }
            catch
            {

            }

            return notifications;
        }

        public bool GerarNotificacaoAmizadeAceita(ObjectId From, ObjectId To)
        {
            var returno = UsuarioHelper.GetUsuarioByObjetcID(From);

            Dictionary<string, string> valores = new Dictionary<string, string>();
            valores.Add("URL_DESTINO", "/Dating/Perfil/"+returno.Usuario);
            valores.Add("ALT_IMAGEM", returno.Usuario);
            valores.Add("URL_IMAGEM", returno.imagemPerfil);
            valores.Add("SUBTITULO", returno.Usuario+" Aceitou sua solicitação de amizade");

            valores.Add("DESCRICAO", "Agora vocês poderão ter uma interação maior na Kinkee");


            dynamic dados = valores;
            List<NotificationSendTo> notificationSends = new List<NotificationSendTo>();

            notificationSends.Add(NotificationSendTo.Site);
            notificationSends.Add(NotificationSendTo.Push);
            notificationSends.Add(NotificationSendTo.Email);

            return _notificationBSN.GerarNovaNotificacao(dados, Notificationtype.NotificacaoGeral, To, From, notificationSends);
        }

        public List<UserModel> GetListUserNotFriendShip(ObjectId UserId)
        {
            var friendShipList = GetListRelationShipByUserID(UserId, StatusRelationShip.Accepted);
            List<ObjectId> objectIds = new List<ObjectId>();

            foreach (var item in friendShipList)
            {
                if(item.UserId == UserId)
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
            var usersList = _userBSN.GetListAllActiveUsersNotFriendShip(objectIds);

            return usersList;
        }

        public List<UserModel> GetListUserNotFriendShip(ObjectId UserId, string genero)
        {
            var friendShipList = GetListRelationShipByUserID(UserId, StatusRelationShip.Accepted);
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
            var usersList = _userBSN.GetListAllActiveUsersNotFriendShip(objectIds, genero);

            return usersList;
        }
    }
}