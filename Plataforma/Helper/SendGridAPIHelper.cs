using Newtonsoft.Json;
using Plataforma.Models;
using RestSharp;

namespace Plataforma.Helper
{
    public static class SendGridAPIHelper
    {
        public static string baseUrlAPI = "https://api.sendgrid.com/v3/";
        public static string token = "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";


        public static bool AddContactToList(ContactSendGridModel contacts)
        {
            var client = new RestClient(baseUrlAPI + "marketing/contacts");
            var json = JsonConvert.SerializeObject(contacts);

            IRestResponse response = Request(json, client, Method.PUT);

            return true;
        }


        public static IRestResponse Request(string json, RestClient client, Method method)
        {
            var request = new RestRequest(method);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + token);

            request.AddParameter("application/json", json, ParameterType.RequestBody);

            IRestResponse response = client.Execute(request);

            return response;
        }
    }
}