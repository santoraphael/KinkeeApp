using Mongo.Models;
using Mongo.Models.Compra;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Mongo.Services
{
    public class Requests
    {
        private string apiKey = ConfigurationManager.AppSettings["apiKeyToken"].ToString();


        public string RequestCreateCadrd(CartaoDTO cartaoDTO) 
        {
            cartaoDTO.api_key = apiKey;

            var json = JsonConvert.SerializeObject(cartaoDTO);

            var client = new RestClient("https://api.pagar.me/1/cards");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            
            return response.Content;
        }

        public string RequestCreateSubscription(AssinaturaDTO assinaturaDTO)
        {
            assinaturaDTO.api_key = apiKey;

            var json = JsonConvert.SerializeObject(assinaturaDTO);

            var client = new RestClient("https://api.pagar.me/1/subscriptions");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        //MUDAR A CHAVE PARA DE PRODUÇÃO
        public string RequestGetSubscription(string plan_id)
        {
            dynamic ob = new System.Dynamic.ExpandoObject();
            ob.api_key = apiKey;

            var json = JsonConvert.SerializeObject(ob);


            var client = new RestClient("https://api.pagar.me/1/subscriptions/" + plan_id);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");


            //MUDAR A CHAVE PARA DE PRODUÇÃO
            request.AddParameter("application/json", json, ParameterType.RequestBody);


            IRestResponse response = client.Execute(request);

            return response.Content;




        }

        public string RequestConsultaCEP(string CEP)
        {
            var client = new RestClient("https://api.pagar.me/1/zipcodes/" + CEP);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");
            request.AddParameter("application/json", "{}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            return response.Content;
        }


        public string RequestCriarTransacao(CompraModel compraDTO)
        {
            compraDTO.api_key = apiKey;

            var json = JsonConvert.SerializeObject(compraDTO);

            var client = new RestClient("https://api.pagar.me/1/transactions");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

        public string RequestCriarTransacaoBoleto(CompraBoletoModel compraDTO)
        {
            compraDTO.api_key = apiKey;

            var json = JsonConvert.SerializeObject(compraDTO);

            var client = new RestClient("https://api.pagar.me/1/transactions");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }


        private object Request()
        {
            var client = new RestClient("https://api.pagar.me/1/cards");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("Cookie", "__cfduid=d3804576e8ff38dc1ca4b5c76b985af991618114069");
            request.AddParameter("application/json", "{\n    \"api_key\": \"ak_test_uorQgpRZ92wycYV8wTdObz7qcxoCzp\", \n    \"card_expiration_date\": \"0427\", \n    \"card_number\": \"5502091548637496\",\n    \"card_cvv\": \"326\", \n    \"card_holder_name\": \"Raphael e Santo\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }

    }
}
