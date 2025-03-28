using Mongo.Models;
using System.Web;

namespace Plataforma.Models
{
    public class CreateProfileModel
    {

        public string NomeUsuario { get; set; }

        public string GeneroUsuario { get; set; }

        public string PrimeiroNome { get; set; }

        public bool ProfileCreated { get; set; }

        public bool ApprovedProfile { get; set; }

        public string LinkFacebook { get; set; }

        public string LinkInstagram { get; set; }


        public int Country_str_code { get; set; }

        public int Admin1_str_code { get; set; }

        public string Admin1_str_name { get; set; }

        public int Feature_int_id { get; set; }

        public string Feature_str_name { get; set; }

        public int StatusRelacionamento { get; set; }

        public string NomeStatusRelacionamento { get; set; }

        public string DescricaoRelacionamento { get; set; }

        public int IdGrupoProfissao { get; set; }

        public string NomeGrupoProfissao { get; set; }

        public int IdProfissao { get; set; }

        public string NomeProfissao { get; set; }

        public int IdRenaMensal { get; set; }

        public string DescricaoRenaMensal { get; set; }

        public int IdPatrimonio { get; set; }

        public string DescricaoPatrimonio { get; set; }

        public bool DisponibilidadeViagens { get; set; }

        public int IdGenerosidade { get; set; }

        public string DescricaoGenerosidade { get; set; }

        public string DetalheGenerosidade { get; set; }

        public int IdMotivoBaby { get; set; }

        public string DescricaoMotivoBaby { get; set; }

        public string DetalheMotivoBaby { get; set; }

        public bool DesejoReceberComunicacao { get; set; }

        public bool AceitoOsTermos { get; set; }

        public bool Finalizado { get; set; }

        public HttpPostedFileBase Foto { get; set; }
    }
}