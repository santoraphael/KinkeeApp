using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public class CountriesModel : BaseModel
    {
        public string Country_str_code { get; set; }
        public string Country_str_name { get; set; }
    }

    public class StatesModel : BaseModel
    {
        public string Admin1_str_code { get; set; }
        public string Country_str_code { get; set; }
        public string Admin1_str_name { get; set; }
    }

    public class CitiesModel : BaseModel
    {
        public string Feature_int_id { get; set; }
        public string Admin1_str_code { get; set; }
        public string Country_str_code { get; set; }
        public string Feature_str_name { get; set; }
        public string Feature_dec_lat { get; set; }
        public string Feature_dec_lon { get; set; }
        public string capital { get; set; }
    }
}
