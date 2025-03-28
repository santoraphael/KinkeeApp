using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mongo.DAL;
using Mongo.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Configuration;
using RestSharp;
using ImageResizer;
using System.IO;
using System.Web.Hosting;
using System.Net;
using System.Drawing;
using System.Text;

namespace Mongo.Services
{
    public class QrCode
    {

        public string RequestGeraQrCodeEstatico(QrCodeEstaticoModel qrCodeParam)
        {
            string baseUrlQRCODE = "https://gerarqrcodepix.com.br/api/v1?nome="+ qrCodeParam.nome + "&cidade="+ qrCodeParam.cidade + "&chave="+ qrCodeParam.chave + "&valor="+ qrCodeParam.valor.ToString().Replace(",",".") + "&txid="+ qrCodeParam.txid + "&saida="+ qrCodeParam.saida;

            //string urlFormat = string.Format(baseUrlQRCODE, qrCodeParam.nome, qrCodeParam.cidade, qrCodeParam.chave, qrCodeParam.valor, qrCodeParam.txid, qrCodeParam.saida);

            var client = new RestClient(baseUrlQRCODE);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if(response.ContentType == "image/png")
            {
                Stream s = new MemoryStream();
                //byte[] buffer = new byte[response.Content.Length];
                //s.Read(buffer, 0, buffer.Length);

                response.StatusCode = HttpStatusCode.OK;
                byte[] buffer = response.RawBytes;

                return Convert.ToBase64String(buffer);
            }

            return response.Content;
        }
    }
}