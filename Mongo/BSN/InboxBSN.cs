using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;
using Mongo.Infrastruture.Helper;

namespace Mongo.BSN
{
    public class InboxBSN
    {
        InboxDAL InboxDAL = new InboxDAL();

        public bool NewInbox(string _from, string _to, string _message)
        {
            bool retorno = false;
            try
            {
                var de = UsuarioHelper.GetUsuarioByString(_from);
                var para = UsuarioHelper.GetUsuarioByString(_to);

                InboxModel inbox = new InboxModel();
                MessageModel mensagem = new MessageModel();
                List<MessageModel> listMessages = new List<MessageModel>();
                mensagem.Message = _message;
                mensagem.FromId = de.Id;
                mensagem.ToId = para.Id;
                mensagem.DateLastInteraction = DateTime.Now;
                mensagem.isActive = true;


                listMessages.Add(mensagem);
                inbox.Messages = listMessages;
                
                InboxDAL.NewIbox(inbox, de, para);
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public void NewMessage(InboxModel inbox, UserModel de, UserModel para)
        {
            InboxDAL.NewMessage(inbox, de, para);
        }

        public InboxModel GetIboxByID(ObjectId inboxID)
        {
            return InboxDAL.GetIboxByID(inboxID);
        }
    }
}
