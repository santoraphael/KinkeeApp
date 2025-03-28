using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mongo.DAL;
using Mongo.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.Configuration;

namespace Mongo.Services
{
    public class Assinatura
    {
        Requests requests = new Requests();
        SubscriptionDAL subscriptionDal = new SubscriptionDAL();
        


        public CartaoModel CriarCartao(CartaoDTO cartaoDTO, ObjectId userId)
        {
            var cartao = requests.RequestCreateCadrd(cartaoDTO);
            var cartaoCriado = JsonConvert.DeserializeObject<Card>(cartao);
            CartaoModel cartaoModel = new CartaoModel();

            if (cartaoCriado.valid)
            {
                cartaoModel.userId = userId;
                cartaoModel.id_cartao = cartaoCriado.id;
                cartaoModel.DateCreate = cartaoCriado.date_created;
                cartaoModel.DateLastInteraction = cartaoCriado.date_updated;
                cartaoModel.brand = cartaoCriado.brand;
                cartaoModel.holder_name = cartaoCriado.holder_name;
                cartaoModel.first_digits = cartaoCriado.first_digits;
                cartaoModel.last_digits = cartaoCriado.last_digits;
                cartaoModel.country = cartaoCriado.country;
                cartaoModel.fingerprint = cartaoCriado.fingerprint;
                cartaoModel.customer = cartaoCriado.customer;
                cartaoModel.valid = cartaoCriado.valid;
                cartaoModel.expiration_date = cartaoCriado.expiration_date;


                subscriptionDal.InsertCard(cartaoModel);
            }
            else
            {
                cartaoModel.valid = false;
            }
            


            return cartaoModel;
        }


        public AssinaturaModel FazerAssinatura(AssinaturaDTO assinaturaDTO, ObjectId userId)
        {
            var assinatura = requests.RequestCreateSubscription(assinaturaDTO);
            var subscription = JsonConvert.DeserializeObject<Subscription>(assinatura);
            AssinaturaModel assinaturaModel = new AssinaturaModel();

            if (!string.IsNullOrEmpty(subscription.id))
            {
                assinaturaModel.userId = userId;

                assinaturaModel.plan = subscription.plan;
                assinaturaModel.subscriptionId = subscription.id;
                assinaturaModel.current_transaction = subscription.current_transaction;
                assinaturaModel.postback_url = subscription.postback_url;
                assinaturaModel.payment_method = subscription.payment_method;
                assinaturaModel.card_brand = subscription.card_brand;
                assinaturaModel.card_last_digits = subscription.card_last_digits;
                assinaturaModel.current_period_start = subscription.current_period_start;
                assinaturaModel.current_period_end = subscription.current_period_end;
                assinaturaModel.charges = subscription.charges;
                assinaturaModel.soft_descriptor = subscription.soft_descriptor;
                assinaturaModel.status = subscription.status;
                assinaturaModel.date_created = subscription.date_created;
                assinaturaModel.date_updated = subscription.date_updated;
                assinaturaModel.phone = subscription.phone;
                assinaturaModel.address = subscription.address;
                assinaturaModel.customer = subscription.customer;
                assinaturaModel.card = subscription.card;
                assinaturaModel.metadata = subscription.metadata;
                assinaturaModel.fine = subscription.fine;
                assinaturaModel.interest = subscription.interest;
                assinaturaModel.settled_charges = subscription.settled_charges;
                assinaturaModel.manage_token = subscription.manage_token;
                assinaturaModel.manage_url = subscription.manage_url;

                
                return subscriptionDal.InsertAssinatura(assinaturaModel);
            }
            else
            {
                return assinaturaModel;
            }
        }


        public AssinaturaModel ConsultarAssinatura(string plan_id)
        {
            //MUDAR A CHAVE PARA DE PRODUÇÃO
            var assinatura = requests.RequestGetSubscription(plan_id);
            var subscription = JsonConvert.DeserializeObject<Subscription>(assinatura);
            AssinaturaModel assinaturaModel = new AssinaturaModel();


            if (!string.IsNullOrEmpty(subscription.id))
            {
                assinaturaModel.plan = subscription.plan;
                assinaturaModel.current_transaction = subscription.current_transaction;
                assinaturaModel.postback_url = subscription.postback_url;
                assinaturaModel.payment_method = subscription.payment_method;
                assinaturaModel.card_brand = subscription.card_brand;
                assinaturaModel.card_last_digits = subscription.card_last_digits;
                assinaturaModel.current_period_start = subscription.current_period_start;
                assinaturaModel.current_period_end = subscription.current_period_end;
                assinaturaModel.charges = subscription.charges;
                assinaturaModel.soft_descriptor = subscription.soft_descriptor;
                assinaturaModel.status = subscription.status;
                assinaturaModel.date_created = subscription.date_created;
                assinaturaModel.date_updated = subscription.date_updated;
                assinaturaModel.phone = subscription.phone;
                assinaturaModel.address = subscription.address;
                assinaturaModel.customer = subscription.customer;
                assinaturaModel.card = subscription.card;
                assinaturaModel.metadata = subscription.metadata;
                assinaturaModel.fine = subscription.fine;
                assinaturaModel.interest = subscription.interest;
                assinaturaModel.settled_charges = subscription.settled_charges;
                assinaturaModel.manage_token = subscription.manage_token;
                assinaturaModel.manage_url = subscription.manage_url;
            }

            return assinaturaModel;
        }

        public Address RequestConsultaCEP(string cep)
        {
            var address = requests.RequestConsultaCEP(cep);
            var enderecoEncontrado = JsonConvert.DeserializeObject<Address>(address);
            enderecoEncontrado.street_number = "100";

            return enderecoEncontrado;
        }
    }
}