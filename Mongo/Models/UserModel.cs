using Mongo.Models.Afiliados;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Mongo.Models
{
    //SEMPRE ATUALIZAR A MODEL USUARIOS NO PROJETO APP E NA LANDIGN PAGE
    public class UserModel : BaseModel
    {
        public DateTime? DateLastLogin { get; set; }
        public ICollection<ConnectionModel> Connections { get; set; }
        public ICollection<String> Players { get; set; }
        public ICollection<UserInboxModel> Inboxes { get; set; }
        public Interesses Interesses { get; set; }
        public Endereco Endereco { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Descricao { get; set; }
        public string Usuario { get; set; }
        public string Genero { get; set; }
        public DateTime DataAniversario { get; set; }
        public string TipoSugar { get; set; }
        public string TenhoInteresse { get; set; }
        public bool ComplementoCadastrado { get; set; }
        public string Email { get; set; }
        public string SecondEmail { get; set; }
        public string PasswordHash { get; set; }
        public bool AcceptServicesTerms { get; set; }
        public bool LockedOut { get; set; }
        public string imagemPerfil { get; set; }
        public string imagemPerfilPrivado { get; set; }
        public string imagemCapaPrivado { get; set; }
        public ICollection<GaleriaFoto> GaleriaFotos { get; set; }
        public ICollection<String> Favoritos { get; set; }
        public bool ContaSelectBlack { get; set; }
        public bool ContaGold { get; set; }
        public DateTime? DataVencimentoGold { get; set; }
        public string TokenAssinatura { get; set; }
        public bool PerfilVerificado { get; set; }
        public bool PerfilTop { get; set; }
        public bool Adm { get; set; }
        public int? visitasPerfil { get; set; }
        public ICollection<String> visitadoPor { get; set; }
        public ICollection<PedidoUsuarioFotoPrivada> PedidosVisualizarFtPrivadas { get; set; }
        public DateTime? DateLimitSugerido { get; set; }
        public String CodigoConvite { get; set; }
        public ObjectId InvitedBy { get; set; }
        public string NomesPerfil { get; set; }
        public string NumeroTelefone { get; set; }
        public bool PermiteDivulgacaoPerfil { get; set; }

        public bool CategoriaRelacionamentSugar { get; set; }

        public bool CategoriaPacksFotosFas { get; set; }

        public bool CategoriaBDSM { get; set; }

        public bool ConfiguracoesIniciais { get; set; }

        public string DescricaoPerfilPrivado { get; set; }

        public bool ProfileCreated { get; set; }

        public bool ApprovedProfile { get; set; }

        public int QtdClicks { get; set; }

        public DateTime DataLiberacaoClicks { get; set; }

    }

    public class UserInformationModel : BaseModel
    {
        public ObjectId UserInformationId { get; set; }

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
        public string FotoParaAprovacao { get; set; }

        
        public string OrientacaoSexual { get; set; }
        public string EstadoCivil { get; set; }
        public string Signo { get; set; }
        public string Etnia { get; set; }
        public string Cabelos { get; set; }
        public string TipoCabelos { get; set; }
        public string Olhos { get; set; }
        public string Altura { get; set; }
        public string Peso { get; set; }
        public string Corpo { get; set; }
        public string Fuma { get; set; }
        public string Bebe { get; set; }
        public int? sugar_score { get; set; }
    }

    public class UserBasicData
    {
        public ObjectId Id { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public DateTime? Birthday { get; set; }
        public int Gender { get; set; }
        public string CodigoConvite { get; set; }
        public bool? ConviteInvalido { get; set; }
        public ICollection<String> ListaHashTags { get; set; }
        public string HashtagSugestion { get; set; }
    }

    public class UserBlockModel : BaseModel
    {
        public string UserIdBlock { get; set; }
        public string UserBlocked { get; set; }
    }

    public class UserReportModel : BaseModel
    {
        public string UserIdReport { get; set; }
        public string UserIdReported { get; set; }
        public string ReportedType { get; set; }
        public string ReportDetails { get; set; }
    }

    public class UserPrivatePermitionModel : BaseModel
    {
        public string UserIdPermition { get; set; }
        public ICollection<String> UsersPermited { get; set; }
    }

    public class Endereco
    {
        public string CEP { get; set; }
        public string Pais { get; set; }
        public string Estado{ get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereço { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string IPv4 { get; set; }
    }

    public class GaleriaFoto
    {
        public string UrlFoto { get; set; }
        public bool isPrivate { get; set; }
        public bool isApproved { get; set; }
    }

    public class PedidoUsuarioFotoPrivada
    {
        public string idUsuario { get; set; }
        public bool Aprovado { get; set; }
    }

    public class Interesses
    {
        [Display(Name = "Companhia para Viagens")]
        public bool CompanhiaViagens { get; set; }


        [Display(Name = "Companhia para Jantares, Cinemas e Baladas")]
        public bool CompanhiaJantar { get; set; }


        [Display(Name = "Mimos (flores, joias, dinheiro e etc)")]
        public bool Mimos { get; set; }


        [Display(Name = "Mesada (ajuda financeira para faculdade, cursos, beleza e diversão)")]
        public bool Mesada { get; set; }


        [Display(Name = "Amor & Sexo")]
        public bool AmorSexo { get; set; }


        [Display(Name = "Interesse em Novas Experiências")]
        public bool InteresseEmNovasExperiencas { get; set; }

        [Display(Name = "Renda Brunta")]
        public int ValorRenda { get; set; }


        [Display(Name = "Mimos a partir")]
        public int ValorMimo { get; set; }
    }

    public class UserInboxModel : BaseModel
    {
        public ObjectId ParaUsuarioID { get; set; }
        public string nomeUsuario { get; set; }
        public string FotoUsuario { get; set; }
        public ObjectId InboxeID { get; set; }
        public MessageModel LastMessage { get; set; }
        public bool? HasUnreadMessage { get; set; }

    }

    public class SendEmailAddress
    {
        public string Email { get; set; }
        public string Nome { get; set; }
    }

    public class EstadoCivil
    {
        public string Id { set; get; }
        public HttpPostedFileBase File { set; get; }
    }
}
