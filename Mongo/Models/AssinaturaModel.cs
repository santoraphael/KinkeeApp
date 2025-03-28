using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{

    #region Cartao
    public class CartaoModel : BaseModel
    {
        public ObjectId userId { get; set; }
        public string tipo { get; set; }
        public string id_cartao { get; set; }
        public string brand { get; set; }
        public string holder_name { get; set; }
        public string first_digits { get; set; }
        public string last_digits { get; set; }
        public string country { get; set; }
        public string fingerprint { get; set; }
        public string customer { get; set; }
        public bool valid { get; set; }
        public string expiration_date { get; set; }

    }

    public class Card
    {
        public string @object { get; set; }
        public string id { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public string brand { get; set; }
        public string holder_name { get; set; }
        public string first_digits { get; set; }
        public string last_digits { get; set; }
        public string country { get; set; }
        public string fingerprint { get; set; }
        public bool valid { get; set; }
        public string expiration_date { get; set; }
        public string customer { get; set; }
    }

    public class CartaoDTO
    {
        public string api_key { get; set; }
        public string card_expiration_date { get; set; }
        public string card_number { get; set; }
        public string card_cvv { get; set; }
        public string card_holder_name { get; set; }
    }
    #endregion


    #region assinatura
    public class AssinaturaDTO
    {
        public AssinaturaDTO()
        {
            Customer _customer = new Customer();
            customer = _customer;
        }

        public string api_key { get; set; }
        public string card_id { get; set; }
        public Customer customer { get; set; }
        public string payment_method { get; set; }
        public string plan_id { get; set; }
        public string postback_url { get; set; }
    }

    public class AssinaturaModel
    {
        public ObjectId Id { get; set; }
        public ObjectId userId { get; set; }
        public Plan plan { get; set; }
        public string subscriptionId { get; set; }
        public CurrentTransaction current_transaction { get; set; }
        public string postback_url { get; set; }
        public string payment_method { get; set; }
        public string card_brand { get; set; }
        public string card_last_digits { get; set; }
        public DateTime current_period_start { get; set; }
        public DateTime current_period_end { get; set; }
        public int charges { get; set; }
        public object soft_descriptor { get; set; }
        public string status { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public Phone phone { get; set; }
        public Address address { get; set; }
        public Customer customer { get; set; }
        public Card card { get; set; }
        public object metadata { get; set; }
        public Fine fine { get; set; }
        public Interest interest { get; set; }
        public object settled_charges { get; set; }
        public string manage_token { get; set; }
        public string manage_url { get; set; }
    }

    public class Subscription
    {
        public string @object { get; set; }
        public Plan plan { get; set; }
        public string id { get; set; }
        public CurrentTransaction current_transaction { get; set; }
        public string postback_url { get; set; }
        public string payment_method { get; set; }
        public string card_brand { get; set; }
        public string card_last_digits { get; set; }
        public DateTime current_period_start { get; set; }
        public DateTime current_period_end { get; set; }
        public int charges { get; set; }
        public object soft_descriptor { get; set; }
        public string status { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public Phone phone { get; set; }
        public Address address { get; set; }
        public Customer customer { get; set; }
        public Card card { get; set; }
        public object metadata { get; set; }
        public Fine fine { get; set; }
        public Interest interest { get; set; }
        public object settled_charges { get; set; }
        public string manage_token { get; set; }
        public string manage_url { get; set; }
    }

    #endregion


    public class Customer
    {
        public Customer()
        {
            Address _address = new Address();
            address = _address;

            Phone _phone = new Phone();
            phone = _phone;
        }

        public Address address { get; set; }
        public string document_number { get; set; }
        public string email { get; set; }
        public string name { get; set; }
        public Phone phone { get; set; }
        public int id { get; set; }
        public object external_id { get; set; }
        public object type { get; set; }
        public object country { get; set; }
        public string document_type { get; set; }
        public object phone_numbers { get; set; }
        public object born_at { get; set; }
        public object birthday { get; set; }
        public object gender { get; set; }
        public DateTime date_created { get; set; }
        //public List<object> documents { get; set; }
    }

    public class Address
    {
        //Bairro
        public string neighborhood { get; set; }

        //Rua
        public string street { get; set; }

        //Numero da Casa
        public string street_number { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        //CEP 04571020
        public string zipcode { get; set; }

        //public object complementary { get; set; }
        //public string state { get; set; }
        //public string country { get; set; }
    }

    public class Phone
    {
        public string ddd { get; set; }

        public string number { get; set; }
    }


    public class Plan
    {
        public string @object { get; set; }
        public int id { get; set; }
        public int amount { get; set; }
        public int days { get; set; }
        public string name { get; set; }
        public int trial_days { get; set; }
        public DateTime date_created { get; set; }
        public List<string> payment_methods { get; set; }
        public string color { get; set; }
        public object charges { get; set; }
        public int installments { get; set; }
        public int invoice_reminder { get; set; }
        public int payment_deadline_charges_interval { get; set; }
    }

    public class Metadata
    {
    }

    public class AntifraudMetadata
    {
    }

    public class CurrentTransaction
    {
        public string @object { get; set; }
        public string status { get; set; }
        public object refuse_reason { get; set; }
        public string status_reason { get; set; }
        public string acquirer_response_code { get; set; }
        public string acquirer_name { get; set; }
        public string acquirer_id { get; set; }
        public string authorization_code { get; set; }
        public object soft_descriptor { get; set; }
        public int tid { get; set; }
        public int nsu { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }
        public int amount { get; set; }
        public int authorized_amount { get; set; }
        public int paid_amount { get; set; }
        public int refunded_amount { get; set; }
        public int installments { get; set; }
        public int id { get; set; }
        public int cost { get; set; }
        public string card_holder_name { get; set; }
        public string card_last_digits { get; set; }
        public string card_first_digits { get; set; }
        public string card_brand { get; set; }
        public object card_pin_mode { get; set; }
        public bool card_magstripe_fallback { get; set; }
        public bool cvm_pin { get; set; }
        public object postback_url { get; set; }
        public string payment_method { get; set; }
        public string capture_method { get; set; }
        public object antifraud_score { get; set; }
        public object boleto_url { get; set; }
        public object boleto_barcode { get; set; }
        public object boleto_expiration_date { get; set; }
        public string referer { get; set; }
        public string ip { get; set; }
        public int subscription_id { get; set; }
        public Metadata metadata { get; set; }
        public AntifraudMetadata antifraud_metadata { get; set; }
        public object reference_key { get; set; }
        public object device { get; set; }
        public object local_transaction_id { get; set; }
        public object local_time { get; set; }
        public bool fraud_covered { get; set; }
        public object fraud_reimbursed { get; set; }
        public object order_id { get; set; }
        public string risk_level { get; set; }
        public object receipt_url { get; set; }
        public object payment { get; set; }
        public object addition { get; set; }
        public object discount { get; set; }
        public object private_label { get; set; }
        public object pix_qr_code { get; set; }
        public object pix_expiration_date { get; set; }
    }


    public class Fine
    {
    }

    public class Interest
    {
    }
}
