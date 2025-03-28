using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mongo.DAL;
using Mongo.Models;
using Mongo.Models.Compra;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Configuration;
using Mongo.BSN;

namespace Mongo.Services
{
    public class Compra
    {
        Requests requests = new Requests();
        TransacaoDAL TransacaoDal = new TransacaoDAL();
        


        public bool CriarNovaTransacao(CompraModel compraDTO)
        {
            var cartao = requests.RequestCriarTransacao(compraDTO);
            var cartaoCriado = JsonConvert.DeserializeObject<TransacaoRetorno>(cartao);

            if(String.IsNullOrEmpty(cartaoCriado.status))
            {
                return false;
            }

            if (cartaoCriado.status == "paid")
            {
                TransacaoDal.InsertTransacao(cartaoCriado);

                return true;
            }
            else
            {
                return false;
            }
            
            //return cartaoModel;
        }

        public TransacaoRetorno CriarNovaTransacaoBoleto(CompraBoletoModel compraDTO)
        {
            var cartao = requests.RequestCriarTransacaoBoleto(compraDTO);
            var cartaoCriado = JsonConvert.DeserializeObject<TransacaoRetorno>(cartao);

            if (String.IsNullOrEmpty(cartaoCriado.status))
            {
                return null;
            }

            if (cartaoCriado.status == "waiting_payment")
            {
                TransacaoDal.InsertTransacao(cartaoCriado);
            }

            return cartaoCriado;
        }
    }
}