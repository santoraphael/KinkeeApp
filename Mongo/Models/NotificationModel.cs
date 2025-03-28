using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class NotificationModel : BaseModel
    {
        public dynamic AddtionalData { get; set; }
        public bool Deleted { get; set; }
        public Notificationtype Notificationtype { get; set; }
        public bool Read { get; set; }
        public ObjectId RecipientUserId { get; set; }
        public ObjectId SenderUserId { get; set; }
        public List<NotificationSendTo> NotificationSendTo { get; set; }
    }

    public class NotificationSettingModel
    {
        public ObjectId UserId { get; set; }
        public bool Allow { get; set; }
        public NotificationSendTo NotificationSendTo { get; set; }
        public bool Urgent { get; set; }
    }

    public enum NotificationSendTo
    {
        Site = 1,
        Push = 2,
        Email = 3,
        Sms = 4,
    }

    public enum Notificationtype
    {
        NotificacaoGeral = 1,
        NovaMesagemRecebida = 2,
        NovoPedidoAmizade = 3,
    }

    public static class NotificationTypes
    {
        //public static NotificationTypeModel Notificacao_Geral()
        //{
            
        //    NotificationTypeModel notificationTypeModel = new NotificationTypeModel();

        //    notificationTypeModel.Notificationtype = Notificationtype.NotificacaoGeral;
        //    notificationTypeModel.NameNotification = "Notificação";
        //    notificationTypeModel.Template = @"<li class='ng-scope'>
        //                                        <a href='{{URL_DESTINO}}' class='clearfix line' target='_self'>
        //                                            <div class='avatar overlayContentCard' hoverintent_s='0'>
        //                                                <img alt='{{ALT_IMAGEM}}' class='img-circle' src='{{URL_IMAGEM}}'>
        //                                            </div>

        //                                            <div class='about'>
        //                                                <div class='about-section'>
        //                                                    <div href='{{URL_DESTINO}}' class='title overlayContentCard' hoverintent_s='0' target='_self'>{{TITULO}}</div>
        //                                                    <span class='subtitle date ng-binding'>{{SUBTITULO}}</span>
        //                                                </div>
        //                                                <div class='about-section'>
        //                                                    <p>
        //                                                        <span class='ng-binding'>{{DESCRICAO}}</span>
        //                                                    </p>
        //                                                </div>
        //                                            </div>
        //                                        </a>
        //                                    </li>";

        //    return notificationTypeModel;
        //}
    }
}
