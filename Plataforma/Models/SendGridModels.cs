using System.Collections.Generic;

namespace Plataforma.Models
{
    public class CustomFieldsSendGridModel
    {
        public string e1_T { get; set; }
        public string e2_T { get; set; }
    }

    public class ContactFieldsSendGridModel
    {
        public string city { get; set; }
        public string country { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string postal_code { get; set; }
        public string state_province_region { get; set; }
        public CustomFieldsSendGridModel custom_fields { get; set; }
    }

    public class ContactSendGridModel
    {
        public ContactSendGridModel(List<string> _list_ids, List<ContactFieldsSendGridModel> _contacts)
        {
            list_ids = _list_ids;
            contacts = _contacts;
        }

        //Lista para qual vai o contato
        public List<string> list_ids { get; set; }

        //Dados do contato
        public List<ContactFieldsSendGridModel> contacts { get; set; }
    }
}