using System;
using System.Web;
using System.Web.Security;
using System.Linq;
using Mongo.Conn;
using MongoDB.Driver;
using Mongo.Models;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Text;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Clients;
using Twilio.Types;
using System.Net;


namespace Mongo.Infrastruture.Helper
{
    public static class SMSHelper
    {
        public static void SendSMS(string Body, string PhoneNumberTo)
        {
            //ACCOUNT SID PRODUCAO
            //AC82b8dd548b1df1cbd29215062f3f91b0

            //AUTH TOKEN PRODUCAO
            //f8a5fa18971e6bf744fda6120b0aee24
            const string accountSid = "AC82b8dd548b1df1cbd29215062f3f91b0";
            const string authToken = "f8a5fa18971e6bf744fda6120b0aee24";


            TwilioClient.Init(accountSid, authToken);

            try
            {
                var messageOptions = new CreateMessageOptions(new PhoneNumber(PhoneNumberTo)); // EXEMPLO: +5511989234005

                messageOptions.From = new PhoneNumber("+19384448442");
                messageOptions.Body = Body;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                                | SecurityProtocolType.Tls11
                                                | SecurityProtocolType.Tls12
                                                | SecurityProtocolType.Ssl3;

                var message = MessageResource.Create(messageOptions);

                var retorno = "Message SID: " + message.Sid;
            }
            catch (Exception ex)
            {

            }

        }
    }
}