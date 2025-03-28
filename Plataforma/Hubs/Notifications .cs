using OneSignal.CSharp.SDK;
using OneSignal.CSharp.SDK.Resources;
using OneSignal.CSharp.SDK.Resources.Notifications;
using System;
using System.Collections.Generic;

namespace Plataforma
{
    public class Notifications
    {
        public void CreateNotification(List<string> PlayerIds, string TextoNotificacao, string Url, string Head)
        {

            var client = new OneSignalClient("MTMzNTY1NjgtMzdjNi00ZTVlLTliM2YtYmM4ZGExYTA1MWNj");

            var options = new NotificationCreateOptions();

            IDictionary<string, string> data = new Dictionary<string, string>();
            data.Add("targetUrl", Url);


            options.AppId = new Guid("8ebd0caa-2aaf-4b1b-9a39-2783e3a5ae16");
            //options.IncludedSegments = new List<string> { "Usuários de Teste" };
            options.IncludePlayerIds = PlayerIds;
            options.Data = data;

            options.Contents.Add(LanguageCodes.English, TextoNotificacao);
            //options.Url = Url;
            options.Headings.Add(LanguageCodes.English, Head);

            client.Notifications.Create(options);
        }
    }
}