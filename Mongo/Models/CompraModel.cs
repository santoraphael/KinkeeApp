using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models.Compra
{

    public class CompraModel
    {
        public CompraModel()
        {
            customer = new Customer();
            billing = new Billing();
            items = new List<Item>();
        }

        public string api_key { get; set; }
        public double amount { get; set; }
        public string card_id { get; set; }
        public Customer customer { get; set; }
        public Billing billing { get; set; }
        public List<Item> items { get; set; }
    }

    public class CompraBoletoModel
    {
        public CompraBoletoModel()
        {
            customer = new Customer();
        }

        public string api_key { get; set; }
        public double amount { get; set; }
        public string payment_method { get; set; }
        public string card_id { get; set; }
        public Customer customer { get; set; }
    }

    public class Item
    {
        public ObjectId id { get; set; }
        public string title { get; set; }
        public double unit_price { get; set; }
        public int quantity { get; set; }
        public bool tangible { get; set; }
    }

    public class Billing
    {
        public Billing()
        {
            Address _address = new Address();
            address = _address;
        }

        public string name { get; set; }
        public Compra.Address address { get; set; }
    }

    public class Address
    {
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string neighborhood { get; set; }
        public string street { get; set; }
        public string street_number { get; set; }
        public string zipcode { get; set; }
    }

    public class Customer
    {
        public Customer()
        {
            documents = new List<Document>();
            phone_numbers = new List<string>();
        }

        public string external_id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public List<Document> documents { get; set; }
        public List<string> phone_numbers { get; set; }
        public string birthday { get; set; }
    }

    public class Document
    {
        public string type { get; set; }
        public string number { get; set; }
    }

    public class TransacaoRetorno
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
        public string boleto_url { get; set; }
        public string boleto_barcode { get; set; }
        public string boleto_expiration_date { get; set; }
        public string referer { get; set; }
        public string ip { get; set; }
        public object subscription_id { get; set; }
        public object phone { get; set; }
        public object address { get; set; }
        public Customer customer { get; set; }
        public Billing billing { get; set; }
        public object shipping { get; set; }
        public List<ItemRetorno> items { get; set; }
        public Card card { get; set; }
        public object split_rules { get; set; }
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

    public class ItemRetorno
    {
        public string id { get; set; }
        public string title { get; set; }
        public double unit_price { get; set; }
        public int quantity { get; set; }
        public bool tangible { get; set; }
    }
}
