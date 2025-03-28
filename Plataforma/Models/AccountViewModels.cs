using Mongo.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Plataforma.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Manter logado?")]
        public bool RememberMe { get; set; }
    }

    public class MessageViewModel
    {
        [Required]
        [Display(Name = "Mensagem")]
        public string Mensagem { get; set; }

        [Required]
        [Display(Name = "Para")]
        public string Para { get; set; }
    }

    public class BannerViewModel
    {
        public string Id { get; set; }
        public string h1Text { get; set; }
        public string pText { get; set; }
        public ICollection<ObjectId> vistoPor { get; set; }
        public DateTime dataInicio { get; set; }
        public DateTime dataFim { get; set; }
        public Int32 heightSize { get; set; }
        public string Genero { get; set; }
        public bool MostraBotao { get; set; }
        public string TextoBotao { get; set; }
        public string UrlBotao { get; set; }
        public string Preco { get; set; }

    }

    public class Endereco
    {
        public string CEP { get; set; }
        public string Estado { get; set; }
        public string Cidade { get; set; }
        public string Bairro { get; set; }
        public string Endereço { get; set; }
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

    public class PerfilViewModel
    {
        public ObjectId Id { get; set; }
        public Interesses Interesses { get; set; }
        public Endereco Endereco { get; set; }

        public string Localizacao_Pais { get; set; }
        public string Localizacao_Estado { get; set; }
        public string Localizacao_Cidade { get; set; }

        public string Name { get; set; }
        public string Lastname { get; set; }
        public string Descricao_Curta { get; set; }

        public string Descricao_Longa { get; set; }

        public string Usuario { get; set; }
        public string Genero { get; set; }
        public string TipoSugar { get; set; }
        public string TenhoInteresse { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public bool AcceptServicesTerms { get; set; }
        public bool LockedOut { get; set; }
        public string imagemPerfil { get; set; }
        public int? visitasPerfil { get; set; }
        public bool PerfilVerificado { get; set; }
        public bool PerfilTop { get; set; }
        public bool ContaGold { get; set; }
        public bool ContaSelectBlack { get; set; }
        public bool Adm { get; set; }
        public bool HabilitaEdicao { get; set; }
        public bool PodeVerFotoPrivada { get; set; }
        public bool UsuarioLogadoFotoPrivada { get; set; }
        public bool MostraBotaoEnviaPix { get; set; }
        public String PromotionalCode { get; set; }
        public string CodigoConvite { get; set; }
        public ObjectId InvitedBy { get; set; }
        public ICollection<GaleriaFotoViewModel> GaleriaFotos { get; set; }
        public ICollection<PedidoUsuarioFotoPrivadaViewModel> PedidosVisualizarFtPrivadas { get; set; }
        public IList<PixViewModel> ListaPix { get; set; }

        public string NomeCompleto { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Idade { get; set; }
        public string OrientacaoSexual { get; set; }
        public string Profissao { get; set; }
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
        public string url_image_score { get; set; }
        public string name_sugar_score { get; set; }
        public int? sugar_score { get; set; }

        public CreateProfileModel createProfileModel { get; set; }
    }

    public class PixViewModel
    {
        public double valor { get; set; }
    }

    public class QrCodeEstaticoViewModel
    {
        public string nome { get; set; }
        public string cidade { get; set; }
        public string chave { get; set; }
        public double valor { get; set; }
        public string txid { get; set; }
        public string mcc { get; set; }
        public SaidaPix saida { get; set; }
        public string tamanho { get; set; }
    }

    public class GaleriaFotoViewModel
    {
        public string UrlFoto { get; set; }
        public bool isPrivate { get; set; }
        public bool isApproved { get; set; }
    }

    public class PedidoUsuarioFotoPrivadaViewModel
    {
        public string idUsuario { get; set; }
        public bool Aprovado { get; set; }
    }

    public class CadastroViewModel
    {
        [StringLength(100, ErrorMessage = "Copie e Cole o Codigo de Convite que você recebeu em seu e-mail ou por SMS. Verifique sua Caixa de Spam.")]
        public string CodigoConvite { get; set; }

        [Required]
        [RegularExpression(@"^[a-z0-9._]+@[a-z0-9]+\.[a-z]+(\.[a-z]+)?$", ErrorMessage = "E-mail não é válido")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9](_(?!(\\.|_))|\\.(?!(_|\\.))|[a-zA-Z0-9]){1,18}[a-zA-Z0-9]$", ErrorMessage = "Nome de usuário deve contem numeros e/ou letras apenas e não deve conter espaços. Também devem ter mais que 8 caracteres.")]
        public string NomeUsuario { get; set; }

        [Required]
        public Genero Genero { get; set; }

        [Required]
        public TipoSugar TipoSugar { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Sua senha é muito curta, ela precisa ter ao menos 6 caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

    }

    public enum Genero
    {
        Daddy = 1,
        Baby = 2,
    }

    //public class Genero
    //{
    //    public int GeneroId { get; set; }
    //    public string Nome { get; set; }

    //    public List<Genero> ListaGeneros()
    //    {
    //        return new List<Genero>
    //        {
    //            new Genero { GeneroId = 0, Nome = "---- Eu sou ---"},
    //            new Genero { GeneroId = 1, Nome = "Homem"},
    //            new Genero { GeneroId = 2, Nome = "Mulher"},
    //        };
    //    }
    //}

    public class PublicacaoViewModel
    {
        public string Id { get; set; }
        public string UsuarioPublicacaoID { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetViewModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }

    public class UploadFileResult
    {
        public int IDArquivo { get; set; }
        public string Nome { get; set; }
        public int Tamanho { get; set; }
        public string Tipo { get; set; }
        public string Caminho { get; set; }
    }

}
