using Mongo.BSN;
using MongoDB.Bson;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using Plataforma.Helper;
using Plataforma.Hubs;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Miscellaneous.SugarScore;
using System.Globalization;
using Mongo.INFRA;

namespace Plataforma.Controllers
{
    [Authorize]
    public partial class DatingController : Controller
    {
        InboxBSN _Inbox = new InboxBSN();
        ChatHub hub = new ChatHub();
        ChatBSN _chatBSN = new ChatBSN();

        UserBSN _UserBsn = new UserBSN();
        NotificationBSN _notificationsBSN = new NotificationBSN();


        public ActionResult Index()
        {
            ViewBag.Title = "Relacionamento Sugar";
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            ViewBag.IsBusca = "0";
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            var userInformation = _UserBsn.GetInformationByUserId(usuarioLogado.Id);

            if(userInformation == null)
            {
                userInformation = InserirInformationErro(usuarioLogado);
            }

            ViewBag.ConfiguracoesIniciais = usuarioLogado.ConfiguracoesIniciais;


            //VALIDAR USUARIO APROVADO =======================
            if (usuarioLogado.ProfileCreated && usuarioLogado.ApprovedProfile)
            {
                TempData["tempUsuarioAprovado"] = true;
                
                ViewBag.usuarioAprovado = true;
                ViewBag.MostrarAvisoBloqueio = false;
            }
            else
            {
                if(usuarioLogado.Genero == "Mulher")
                {
                    return Redirect("https://raphaelsanto.com.br/kinkee-sugarbaby");
                }
                

                TempData["tempUsuarioAprovado"] = false;
                ViewBag.usuarioAprovado = false;

                ViewBag.MostrarAvisoBloqueio = true;
            }
            //VALIDAR USUARIO APROVADO =======================


            //BannerModel bannerModel = new BannerModel();
            //bannerModel.dataFim = DateTime.Now;
            //bannerModel.dataInicio = DateTime.Now;
            //bannerModel.DateCreate = DateTime.Now;
            //bannerModel.DateLastInteraction = DateTime.Now;
            //bannerModel.Genero = "0";
            //bannerModel.h1Text = "Seja PREMIUM! Oferta de lançamento.";
            //bannerModel.heightSize = 3;
            //bannerModel.isActive = true;
            //bannerModel.MostraBotao = true;
            //bannerModel.pText = "Aproveite a oferta de lançamento para ter toda a exclusividade Premium por apenas";
            //bannerModel.TextoBotao = "Texto Botão";
            //bannerModel.UrlBotao = "#UrlBotao";
            //bannerModel.Preco = "R$ 9,90";

            //UsuarioHelper.AddBanner(bannerModel);

            if(userInformation != null)
            {
                ViewBag.EloGenero = Scores.PegarEloGenero(userInformation.sugar_score, usuarioLogado.Genero);
            }
            else
            {
                ViewBag.EloGenero = null;
            }
            

            if(!usuarioLogado.ContaGold)
            {
                
                if(usuarioLogado.DataLiberacaoClicks < DateTime.Now)
                {
                    usuarioLogado.QtdClicks = 20;
                    usuarioLogado.DataLiberacaoClicks = DateTime.Now.AddHours(10);

                    _UserBsn.EditarUsuario(usuarioLogado);
                }


                if (usuarioLogado.QtdClicks > 0 && ViewBag.usuarioAprovado)
                {
                    ViewBag.PermissaoBlend = true;
                    TempData["tempUsuarioAprovado"] = true;
                }
                else
                {
                    ViewBag.PermissaoBlend = false;
                    TempData["tempUsuarioAprovado"] = false;

                }
            }
            else
            {
                ViewBag.PermissaoBlend = true;
                TempData["tempUsuarioAprovado"] = true;
            }

            ViewBag.ContaGold = usuarioLogado.ContaGold;
            ViewBag.QtdClicks = usuarioLogado.QtdClicks;
            ViewBag.DataLiberacaoClicks = usuarioLogado.DataLiberacaoClicks.ToString("s", CultureInfo.CreateSpecificCulture("en-US"));

            return View();
        }

        public UserInformationModel InserirInformationErro(UserModel usuario)
        {
            UserInformationModel userInformationModel = new UserInformationModel();
            userInformationModel.DateCreate = null;
            userInformationModel.DateLastInteraction = null;
            userInformationModel.isActive = usuario.isActive;
            userInformationModel.UserInformationId = usuario.Id;
            userInformationModel.NomeUsuario = usuario.Usuario;
            userInformationModel.GeneroUsuario = usuario.Genero;
            userInformationModel.PrimeiroNome = usuario.Name;
            userInformationModel.ProfileCreated = usuario.ProfileCreated;
            userInformationModel.ApprovedProfile = usuario.ApprovedProfile;
            userInformationModel.LinkFacebook = null;
            userInformationModel.LinkInstagram = null;
            userInformationModel.Country_str_code = 0;
            userInformationModel.Admin1_str_code = 0;
            userInformationModel.Admin1_str_name = "";
            userInformationModel.Feature_int_id = 0;
            userInformationModel.Admin1_str_name = "";
            userInformationModel.StatusRelacionamento = 1;
            userInformationModel.NomeStatusRelacionamento = "";
            userInformationModel.DescricaoRelacionamento = null;
            userInformationModel.IdGrupoProfissao = 1;
            userInformationModel.NomeGrupoProfissao = "";
            userInformationModel.IdProfissao = 1;
            userInformationModel.NomeProfissao = "";
            userInformationModel.IdRenaMensal = 0;
            userInformationModel.DescricaoRenaMensal = null;
            userInformationModel.IdPatrimonio = 0;
            userInformationModel.DescricaoPatrimonio = "";
            userInformationModel.DisponibilidadeViagens = false;
            userInformationModel.IdGenerosidade = 0;
            userInformationModel.DescricaoGenerosidade = null;
            userInformationModel.DetalheGenerosidade = null;
            userInformationModel.DescricaoGenerosidade = null;
            userInformationModel.IdMotivoBaby = 3;
            userInformationModel.DescricaoMotivoBaby = "";
            userInformationModel.DetalheMotivoBaby = null;
            userInformationModel.DesejoReceberComunicacao = false;
            userInformationModel.AceitoOsTermos = false;
            userInformationModel.Finalizado = false;
            userInformationModel.Foto = null;
            userInformationModel.FotoParaAprovacao = null;
            userInformationModel.OrientacaoSexual = null;
            userInformationModel.EstadoCivil = null;
            userInformationModel.Signo = null;
            userInformationModel.Etnia = null;
            userInformationModel.Cabelos = null;
            userInformationModel.TipoCabelos = null;
            userInformationModel.Olhos = null;
            userInformationModel.Altura = null;
            userInformationModel.Peso = null;
            userInformationModel.Corpo = null;
            userInformationModel.Fuma = null;
            userInformationModel.Bebe = null;
            userInformationModel.sugar_score = null;


            _UserBsn.AlterUserInformation(userInformationModel);

            return userInformationModel;
        }

        public int BlendClick()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            usuarioLogado.QtdClicks -= 1;

            _UserBsn.EditarUsuario(usuarioLogado);

            return usuarioLogado.QtdClicks;
        }

        public int AtualizaBlendClick()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            if (usuarioLogado.DataLiberacaoClicks < DateTime.Now)
            {
                usuarioLogado.QtdClicks = 20;
                usuarioLogado.DataLiberacaoClicks = DateTime.Now.AddHours(10);

                _UserBsn.EditarUsuario(usuarioLogado);
            }


            return usuarioLogado.QtdClicks;
        }

        public ActionResult LoadProfiles(int pageIndex, int pageSize, string url)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            //usuarioLogado.DateLastInteraction = DateTime.Now;
            //_UserBsn.EditarUsuario(usuarioLogado);


            ViewBag.TopUsers = false;

            List<UserModel> query = new List<UserModel>();

            string Genero = "";

            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
            }
            else
            {
                Genero = "Homem";
            }

            var userInformation = _UserBsn.GetInformationByUserId(usuarioLogado.Id);

            if(userInformation.sugar_score == null)
            {
                userInformation.sugar_score = 0;
            }


            var nivel = Scores.PegarPedraCartao(userInformation.sugar_score);

            int? min = 0;
            int? max = nivel.rangeMaximo + 500;

            var listUsers = _UserBsn.GetInformationByScore(min, max);

            Random rnd = new Random();
            Random rnd2 = new Random();

            query = (from c in _UserBsn.GetListUserRangeScore(listUsers, Genero).OrderByDescending(c => rnd.Next(pageSize)).ToList() select c).OrderBy(c => rnd2.Next(pageSize)).ToList()
                        //.Skip(pageIndex * pageSize)
                        .Take(pageSize).ToList();

            List<UserListViewModel> listUsuarios = new List<UserListViewModel>();
            foreach (var item in query)
            {
                var information =_UserBsn.GetInformationByUserId(item.Id);
                bool adicionar = true;


                if(String.IsNullOrEmpty(item.imagemPerfil) && item.Genero == "Homem")
                {
                    item.imagemPerfil = "https://app.kinkeesugar.com/Users/f2b9025262bb467e9b7af12ff00ebf09_9bee0d2c4958471f878d0cbb6bf2ea80.jpg";
                }
                else if (String.IsNullOrEmpty(item.imagemPerfil) && item.Genero == "Mulher")
                {
                    adicionar = false;
                }
                    

                UserListViewModel userListView = new UserListViewModel();

                userListView.id = item.Id.ToString();
                userListView.Usuario = item.Usuario;
                userListView.imagemPerfil = item.imagemPerfil;

                userListView.Elo = 0;

                DateTime? hoje = DateTime.Now;
                DateTime? DataCadastro = item.DateCreate;

                TimeSpan t = (TimeSpan)(hoje - DataCadastro);
                double NrOfDays = t.TotalDays;

                if(NrOfDays < 15)
                {
                    userListView.NewProfile = true;
                }
                else
                {
                    userListView.NewProfile = false;
                }
                
                userListView.Verify = item.PerfilVerificado;
                userListView.Premium = item.ContaGold;

                if (item.Connections != null)
                {
                    var connection = item.Connections.Select(c => c.Connected == true).FirstOrDefault();

                    userListView.Online = connection;
                }
                else
                {
                    userListView.Online = false;
                }

                
                if (item.GaleriaFotos != null)
                {
                    userListView.QntFotos = item.GaleriaFotos.Count;
                }
                else
                {
                    userListView.QntFotos = 0;
                }

                userListView.IdadeUsuario = CommonHelper.CalculateAge(item.DataAniversario).ToString();

                if(information == null)
                {
                    userListView.CidadeUsuario = "localizando...";

                    

                    userListView.url_image_score = "/modules/img/" + Scores.PegarEloGenero(null, item.Genero).urlImagemElo;
                    userListView.name_sugar_score = Scores.PegarEloGenero(null, item.Genero).nomeElo;
                    userListView.sugar_score = 0;
                }
                else
                {
                    userListView.CidadeUsuario = information.Feature_str_name;

                    userListView.url_image_score = "/modules/img/" + Scores.PegarEloGenero(information.sugar_score, item.Genero).urlImagemElo;
                    userListView.name_sugar_score = Scores.PegarEloGenero(information.sugar_score, item.Genero).nomeElo;
                    userListView.sugar_score = information.sugar_score;
                }

                if(usuarioLogado.Favoritos == null)
                {
                    userListView.displayActions = false;
                }
                else
                {
                    userListView.displayActions = usuarioLogado.Favoritos.Contains(userListView.id);
                }

                //ChatHub chatHub = new ChatHub();
                //userListView.Online = chatHub.GetActiveConnectionByUsername(userListView.Usuario).Result;


                if (adicionar)
                {
                    listUsuarios.Add(userListView);
                }
            }

            

            return Json(listUsuarios.ToList(), JsonRequestBehavior.AllowGet);
        }


        public ActionResult LoadProfilesLaunch(int pageIndex, int pageSize, string url)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ViewBag.TopUsers = false;

            List<UserModel> query = new List<UserModel>();

            string Genero = "";

            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
            }
            else
            {
                Genero = "Homem";
            }

            Random rnd = new Random();
            Random rnd2 = new Random();

            query = (from c in _UserBsn.GetListUserByTipo(Genero).OrderByDescending(c => rnd.Next(200)).ToList() select c).OrderBy(c => rnd2.Next(100)).ToList()
                        //.Skip(pageIndex * pageSize)
                        .Take(pageSize).ToList();

            List<UserListViewModel> listUsuarios = new List<UserListViewModel>();
            foreach (var item in query)
            {
                var information = _UserBsn.GetInformationByUserId(item.Id);
                bool adicionar = true;


                //if (String.IsNullOrEmpty(item.imagemPerfil) && item.Genero == "Homem")
                //{
                //    item.imagemPerfil = "https://app.kinkeesugar.com/Users/f2b9025262bb467e9b7af12ff00ebf09_9bee0d2c4958471f878d0cbb6bf2ea80.jpg";
                //}
                //else if (String.IsNullOrEmpty(item.imagemPerfil) && item.Genero == "Mulher")
                //{
                //    adicionar = false;
                //}


                UserListViewModel userListView = new UserListViewModel();

                userListView.id = item.Id.ToString();
                userListView.Usuario = item.Usuario;
                userListView.imagemPerfil = item.imagemPerfil;

                userListView.Elo = 0;

                DateTime? hoje = DateTime.Now;
                DateTime? DataCadastro = item.DateCreate;

                TimeSpan t = (TimeSpan)(hoje - DataCadastro);
                double NrOfDays = t.TotalDays;

                if (NrOfDays < 15)
                {
                    userListView.NewProfile = true;
                }
                else
                {
                    userListView.NewProfile = false;
                }

                userListView.Verify = item.PerfilVerificado;
                userListView.Premium = item.ContaGold;

                if (item.Connections != null)
                {
                    var connection = item.Connections.Select(c => c.Connected == true).FirstOrDefault();

                    userListView.Online = connection;
                }
                else
                {
                    userListView.Online = false;
                }


                if (item.GaleriaFotos != null)
                {
                    userListView.QntFotos = item.GaleriaFotos.Count;
                }
                else
                {
                    userListView.QntFotos = 0;
                }

                userListView.IdadeUsuario = CommonHelper.CalculateAge(item.DataAniversario).ToString();

                if (information == null)
                {
                    userListView.CidadeUsuario = "localizando...";



                    userListView.url_image_score = "/modules/img/" + Scores.PegarEloGenero(null, item.Genero).urlImagemElo;
                    userListView.name_sugar_score = Scores.PegarEloGenero(null, item.Genero).nomeElo;
                    userListView.sugar_score = 0;
                }
                else
                {
                    userListView.CidadeUsuario = information.Feature_str_name;

                    userListView.url_image_score = "/modules/img/" + Scores.PegarEloGenero(information.sugar_score, item.Genero).urlImagemElo;
                    userListView.name_sugar_score = Scores.PegarEloGenero(information.sugar_score, item.Genero).nomeElo;
                    userListView.sugar_score = information.sugar_score;
                }

                if (usuarioLogado.Favoritos == null)
                {
                    userListView.displayActions = false;
                }
                else
                {
                    userListView.displayActions = usuarioLogado.Favoritos.Contains(userListView.id);
                }

                //ChatHub chatHub = new ChatHub();
                //userListView.Online = chatHub.GetActiveConnectionByUsername(userListView.Usuario).Result;


                if (adicionar)
                {
                    listUsuarios.Add(userListView);
                }
            }



            return Json(listUsuarios.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Verificados()
        {

            ViewBag.Title = "Verificados";
            ViewBag.IsBusca = "0";

            return View();
        }

        public ActionResult VisitasRecebidas()
        {

            ViewBag.Title = "Visitas Recebidas";
            ViewBag.IsBusca = "0";

            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            if (usuarioLogado.ContaGold)
            {
                ViewBag.ContaGold = true;
            }
            else
            {
                ViewBag.ContaGold = false;
            }

            return View();
        }

        public ActionResult Gold()
        {

            ViewBag.Title = "Gold";
            ViewBag.IsBusca = "0";
            return View();
        }

        public ActionResult Black()
        {
            ViewBag.Title = "Black Diamond";
            ViewBag.IsBusca = "0";
            return View();
        }

        public ActionResult ResultadoBusca(string busca)
        {

            ViewBag.Title = "Resultado Busca";
            ViewBag.IsBusca = "1";
            ViewBag.Busca = busca;
            return View();
        }

        // GET: Dating
        public ActionResult Dating()
        {

            ViewBag.Title = "Destaques";
            ViewBag.IsBusca = "0";

            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            string img = " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png";
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);


            string aviso = "";

            if (
                //usuarioLogado.ContaGold == true
                usuarioLogado.PerfilTop == true
                //&& usuarioLogado.PerfilVerificado == true
                && usuarioLogado.imagemPerfil != img
                && usuarioLogado.visitasPerfil > 100
                && usuarioLogado.GaleriaFotos.Count > 0)
            {
                ViewBag.Aviso = "Parabéns! Você está aparecendo no EM ALTA";
            }
            else
            {
                string ImagemPerfil = "Você precisa alterar sua imagem de perfil. ";
                string TopPerfil = "Você precisa estar em TOP PERFIS. ";
                string VisitasPerfil = "Você precisa ter mais de 100 visitas no Perfil. ";
                string GaleriaFotos = "Você precisa Adicionar fotos a sua galeria de fotos. ";

                aviso = "Seu perfil não está sendo mostrado em TOP PERFIS pelos motivos: ";

                if (usuarioLogado.imagemPerfil == img)
                {
                    aviso += ImagemPerfil;
                }

                if (usuarioLogado.PerfilTop == false)
                {
                    aviso += TopPerfil;
                }

                if (usuarioLogado.visitasPerfil < 100)
                {
                    aviso += VisitasPerfil;
                }

                if (usuarioLogado.GaleriaFotos.Count <= 0)
                {
                    aviso += GaleriaFotos;
                }

                ViewBag.Aviso = aviso;
            }


            return View();
        }

        public ActionResult WhatsApp()
        {
            ViewBag.Title = "Nosso Grupos de WhatsApp";
            ViewBag.IsBusca = "0";

            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ViewBag.KinkeeSelect = false;
            ViewBag.KinkeeBabies = false;

            ViewBag.KinkeeSP = false;
            ViewBag.KinkeeInteriorSP = false;
            ViewBag.KinkeeBH = false;
            ViewBag.KinkeeRJ = false;
            ViewBag.NorteNordeste = false;
            ViewBag.CentroOeste = false;
            ViewBag.Sul = false;



            if (!String.IsNullOrEmpty(usuarioLogado.NumeroTelefone))
            {
                ViewBag.NumeroTelefone = usuarioLogado.NumeroTelefone;
                ViewBag.HabilitaCampo = "disabled";


                string _DDD = usuarioLogado.NumeroTelefone.Substring(1, 2);

                ViewBag.KinkeeSelect = ValidaGrupoSelect(usuarioLogado);
                ViewBag.KinkeeBabies = ValidaKinkeeBabies(usuarioLogado);


                switch (_DDD)
                {
                    case "11":
                    case "12":
                    case "13":
                        ViewBag.KinkeeSP = ValidaKinkeeSP(usuarioLogado);
                        break;

                    case "14":
                    case "15":
                    case "16":
                    case "17":
                    case "18":
                    case "19":
                        ViewBag.KinkeeInteriorSP = ValidaKinkeeInteriorSP(usuarioLogado);
                        break;

                    case "31":
                    case "32":
                    case "33":
                    case "34":
                    case "35":
                    case "36":
                    case "37":
                    case "38":
                    case "39":
                        ViewBag.KinkeeBH = ValidaKinkeeBH(usuarioLogado);
                        break;

                    case "21":
                    case "22":
                    case "23":
                    case "24":
                    case "25":
                    case "26":
                    case "27":
                    case "28":
                    case "29":
                        ViewBag.KinkeeRJ = ValidaKinkeeRJ(usuarioLogado);
                        break;

                    case "69":
                    case "68":
                    case "97":
                    case "92":
                    case "95":
                    case "93":
                    case "94":
                    case "91":
                    case "96":
                    case "63":
                    case "71":
                    case "73":
                    case "74":
                    case "75":
                    case "77":
                    case "79":
                    case "81":
                    case "82":
                    case "83":
                    case "84":
                    case "85":
                    case "86":
                    case "87":
                    case "88":
                    case "89":
                    case "98":
                    case "99":
                        ViewBag.NorteNordeste = ValidaNorteNordeste(usuarioLogado);
                        break;


                    case "61":
                    case "62":
                    case "64":
                    case "65":
                    case "66":
                    case "67":
                        ViewBag.CentroOeste = ValidaCentroOeste(usuarioLogado);
                        break;

                    case "41":
                    case "42":
                    case "43":
                    case "44":
                    case "45":
                    case "46":
                    case "47":
                    case "48":
                    case "49":
                    case "51":
                    case "53":
                    case "54":
                    case "55":

                        ViewBag.Sul = ValidaSul(usuarioLogado);
                        break;
                }



                if (usuarioLogado.ContaGold)
                {
                    if (usuarioLogado.DataVencimentoGold != null)
                    {
                        if (usuarioLogado.DataVencimentoGold > DateTime.Now.AddDays(1))
                        {
                            ViewBag.KinkeeSP = true;
                            ViewBag.KinkeeInteriorSP = true;
                            ViewBag.KinkeeBH = true;
                            ViewBag.KinkeeRJ = true;
                            ViewBag.NorteNordeste = true;
                            ViewBag.CentroOeste = true;
                            ViewBag.Sul = true;
                        }
                        else
                        {
                            ViewBag.KinkeeSP = false;
                            ViewBag.KinkeeInteriorSP = false;
                            ViewBag.KinkeeBH = false;
                            ViewBag.KinkeeRJ = false;
                            ViewBag.NorteNordeste = false;
                            ViewBag.CentroOeste = false;
                            ViewBag.Sul = false;
                        }
                    }
                    else
                    {
                        ViewBag.KinkeeSP = true;
                        ViewBag.KinkeeInteriorSP = true;
                        ViewBag.KinkeeBH = true;
                        ViewBag.KinkeeRJ = true;
                        ViewBag.NorteNordeste = true;
                        ViewBag.CentroOeste = true;
                        ViewBag.Sul = true;
                    }


                }

            }


            return View();
        }

        public bool ValidaGrupoSelect(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }

            if (!usuario.PerfilTop)
            {
                _permitirAcesso = false;
            }

            if (!usuario.PerfilVerificado)
            {
                _permitirAcesso = false;
            }


            return _permitirAcesso;
        }

        public bool ValidaKinkeeSP(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }


            return _permitirAcesso;
        }

        public bool ValidaKinkeeInteriorSP(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }


            return _permitirAcesso;
        }

        public bool ValidaKinkeeBH(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }


            return _permitirAcesso;
        }

        public bool ValidaKinkeeRJ(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }


            return _permitirAcesso;
        }

        public bool ValidaNorteNordeste(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }


            return _permitirAcesso;
        }

        public bool ValidaCentroOeste(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (!usuario.ContaGold)
            {
                _permitirAcesso = false;
            }

            if (usuario.TipoSugar == "2")
            {
                _permitirAcesso = true;
            }


            return _permitirAcesso;
        }

        public bool ValidaKinkeeBabies(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (usuario.TipoSugar != "2")
            {
                _permitirAcesso = false;
            }


            return _permitirAcesso;
        }

        public bool ValidaSul(UserModel usuario)
        {
            bool _permitirAcesso = true;

            if (usuario.TipoSugar != "2")
            {
                _permitirAcesso = false;
            }


            return _permitirAcesso;
        }

        public ActionResult SalvarTelefone(string telefone)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            usuarioLogado.NumeroTelefone = telefone;

            var retorno = UsuarioHelper.SalvarNumeroTelefone(usuarioLogado);

            WhatsApp();

            return View("WhatsApp");
        }

        public ActionResult OnlineRecentemente()
        {

            ViewBag.Title = "Online Recentemente";
            ViewBag.IsBusca = "0";
            return View();
        }

        public ActionResult NovosPerfis()
        {

            ViewBag.Title = "Novos Perfis";
            ViewBag.IsBusca = "0";
            return View();
        }

        public ActionResult Search(string SearchText)
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                return View("Dating");
            }

            ViewBag.Title = SearchText;

            var teste = SearchText;

            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);


            ViewBag.BuscarModel = _UserBsn.GetSearch(SearchText);

            //usuarioLogado.DateLastInteraction = DateTime.Now;
            //_UserBsn.EditarUsuario(usuarioLogado);


            return View();
        }

        public ActionResult GetData(int pageIndex, int pageSize, string url)
        {

            string lastPart = "";
            if (!String.IsNullOrEmpty(url.Split('/').Last()))
            {
                lastPart = url.Split('/').Last();
            }
            else
            {
                lastPart = url.Split('/')[4];
            }


            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            usuarioLogado.DateLastInteraction = DateTime.Now;
            _UserBsn.EditarUsuario(usuarioLogado);


            List<UserModel> listaUsuarios = new List<UserModel>();
            List<UserModel> listaTopUsuarios = new List<UserModel>();

            ViewBag.TopUsers = false;

            List<UserModel> query = new List<UserModel>();


            if (lastPart == "Index" || lastPart == "Dating" || lastPart == "")
            {
                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListTopUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListTopUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListTopUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListTopUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }

                else
                {
                    //listaUsuarios = _UserBsn.GetListUserByTipo();

                    query = (from c in _UserBsn.GetListUserByTipo() select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                }
            }
            else if (lastPart == "OnlineRecentemente")
            {

                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListAllUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListAllUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListAllUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListAllUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }
            }
            else if (lastPart == "NovosPerfis")
            {

                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListNewUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListNewUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListNewUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListNewUserByTipo(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }
            }
            else if (lastPart == "Verificados")
            {

                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListVeficidados(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListVeficidados(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListVeficidados(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListVeficidados(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }
            }
            else if (lastPart == "Gold")
            {

                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListGold(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListGold(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListGold(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListGold(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }
            }
            else if (lastPart == "VisitasRecebidas")
            {

                List<string> visitadoPor = new List<string>();

                if (usuarioLogado.visitadoPor != null)
                {
                    visitadoPor = usuarioLogado.visitadoPor.ToList();
                }



                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListVisitadoPor("2", visitadoPor.ToList()) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListVisitadoPor("2", visitadoPor.ToList());


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListVisitadoPor("1", visitadoPor.ToList()) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListVisitadoPor("1", visitadoPor.ToList());


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }
            }
            else if (lastPart == "Black")
            {
                if (usuarioLogado.Genero == "1")
                {
                    //CONTA GOLD
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListBlack(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListBlack(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                }

                else if (usuarioLogado.Genero == "2")
                {
                    if (usuarioLogado.Adm || usuarioLogado.ContaGold)
                    {

                        query = (from c in _UserBsn.GetListBlack(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse) select c)
                             .Skip(pageIndex * pageSize)
                             .Take(pageSize).ToList();

                        ViewBag.TopUsers = true;
                        ViewBag.Destaques = true;

                    }
                    else
                    {
                        listaUsuarios = _UserBsn.GetListBlack(usuarioLogado.Genero, usuarioLogado.TipoSugar, usuarioLogado.TenhoInteresse);


                        int porcentagem = 100;
                        var queryLimit = listaUsuarios.Take(listaUsuarios.Count * porcentagem / 100).ToList();

                        query = (from c in queryLimit select c)
                              .Skip(pageIndex * pageSize)
                              .Take(pageSize).ToList();
                    }

                    ViewBag.Destaques = false;
                }
            }

            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataSearch(int pageIndex, int pageSize, string busca)
        {
            string tipoBusca = busca.Split('_').First();
            busca = busca.Split('_').Last();

            UserModel u = new UserModel
            {
                Usuario = System.Web.HttpContext.Current.User.Identity.Name
            };

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            usuarioLogado.DateLastInteraction = DateTime.Now;
            _UserBsn.EditarUsuario(usuarioLogado);

            var listaUsuarios = new List<UserModel>();
            var query = new List<UserModel>();

            query = (from c in _UserBsn.GetListUserBySearch(usuarioLogado.Genero == "1" ? "2" : "1", tipoBusca, busca) select c)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize).ToList();

            return Json(query.ToList(), JsonRequestBehavior.AllowGet);
        }

        public string FotoPerfil()
        {
            return UsuarioHelper.GetFotoPerfil(System.Web.HttpContext.Current.User.Identity.Name);
        }

        public bool PerfilSugeridoAtivo()
        {
            DateTime? validade = UsuarioHelper.PerfilSugeridoAtivo(System.Web.HttpContext.Current.User.Identity.Name);
            DateTime? DataAtual = DateTime.Now;
            bool valido = false;

            if (validade >= DataAtual)
            {
                valido = true;
            }

            return valido;
        }

        public bool PerfilVerificadoAtivo()
        {
            var perfil = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);


            return perfil.PerfilVerificado;
        }

        public ActionResult EnviarMensagem(SharedViewModel model)
        {
            string de = System.Web.HttpContext.Current.User.Identity.Name;
            string para = model.MessageViewModel.Para.Split('/').Last();
            string mensagem = model.MessageViewModel.Mensagem;

            try
            {
                //_Inbox.NewInbox(de, para, mensagem);

                UserModel u = new UserModel();
                u.Usuario = de;
                var usuarioLogado = _UserBsn.GetUserByUsuario(u);

                UserModel deUsuario = new UserModel();
                deUsuario.Usuario = para;
                var paraUsuario = _UserBsn.GetUserByUsuario(deUsuario);

                var ChatRoomId = _chatBSN.InsertChatRoom(usuarioLogado.Id, paraUsuario.Id);
                _chatBSN.InsertMessage(ChatRoomId, usuarioLogado.Id, mensagem, false);



                Plataforma.Notifications not = new Plataforma.Notifications();

                if (paraUsuario.Players != null)
                {
                    if (paraUsuario.Players.Count > 0)
                    {
                        List<string> PlayerIds = new List<string>();
                        foreach (var player in paraUsuario.Players)
                        {
                            if (!String.IsNullOrEmpty(player))
                            {
                                PlayerIds.Add(player);
                            }
                        }

                        not.CreateNotification(PlayerIds, de + ": " + mensagem, "https://kinkeesugar.com/Dating/Inbox/", "Nova Mensagem!");
                    }

                    //List<string> emailUsuario = new List<string>();
                    //emailUsuario.Add(paraUsuario.Email);
                    //string urlAction = Url.Action("Inbox", "Dating", null, Request.Url.Scheme);
                    //Email.SendMailNewMessage(emailUsuario, paraUsuario.Usuario, urlAction, usuarioLogado.Usuario);


                    SendEmailAddress sendAddress = new SendEmailAddress();
                    sendAddress.Nome = paraUsuario.Usuario;
                    sendAddress.Email = paraUsuario.Email;

                    ProcessaEmails.SendMailMensagemRecebida(sendAddress, paraUsuario.Usuario, paraUsuario.imagemPerfil);
                }
                else
                {
                    SendEmailAddress sendAddress = new SendEmailAddress();
                    sendAddress.Nome = paraUsuario.Usuario;
                    sendAddress.Email = paraUsuario.Email;

                    ProcessaEmails.SendMailMensagemRecebida(sendAddress, paraUsuario.Usuario, paraUsuario.imagemPerfil);
                }

            }
            catch (Exception e)
            {

            }

            //hub.Send(para, mensagem, "/Dating//");

            return RedirectToAction("Inbox", "Dating");
        }

        public ActionResult BannerPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            var banner = UsuarioHelper.GetBannerByUsuarioID(usuarioLogado.Id.ToString(), usuarioLogado.Genero);
            BannerViewModel bannerModel = new BannerViewModel();

            if (banner != null)
            {
                bannerModel.Id = banner.Id.ToString();
                bannerModel.h1Text = banner.h1Text;
                bannerModel.pText = banner.pText;
                bannerModel.heightSize = banner.heightSize;
                bannerModel.TextoBotao = banner.TextoBotao;
                bannerModel.UrlBotao = banner.UrlBotao;
                bannerModel.Preco = banner.Preco;

                ViewBag.MostraBotao = banner.MostraBotao;
                ViewBag.ComBanner = true;
            }
            else
            {
                ViewBag.ComBanner = false;
            }

            var url = Request.Url.PathAndQuery;

            if (url == "/Dating/VisitasRecebidas/")
            {
                ViewBag.ComBanner = false;
            }

            if(usuarioLogado.ContaGold == true)
            {
                ViewBag.ComBanner = false;
            }

            return View(bannerModel);
        }

        public void AddNewViewerBanner(string idBanner)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            BannerModel banner = UsuarioHelper.GetBannerByID(idBanner);

            List<string> listaIdUsusario = new List<string>();

            if (banner.vistoPor != null)
            {
                foreach (var item in banner.vistoPor)
                {
                    listaIdUsusario.Add(item);
                }
            }

            listaIdUsusario.Add(usuarioLogado.Id.ToString());

            banner.vistoPor = listaIdUsusario;
            var retorno = UsuarioHelper.AddNewViewerBanner(banner);
        }

        public ActionResult _DestaquesPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            List<UserModel> listaUsuarios = new List<UserModel>();
            List<UserModel> Aux = new List<UserModel>();
            int pageIndex = 1;
            int pageSize = 10;

            //ViewBag.PromotionalCode = usuarioLogado.PromotionalCode;

            string Genero = "";

            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
                ViewBag.Subtitulo = "Sugar Babies";
            }
            else
            {
                Genero = "Homem";
                ViewBag.Subtitulo = "Sugar Daddies";
            }

            List<UserModel> query = new List<UserModel>();
            var userInformation = _UserBsn.GetInformationByUserId(usuarioLogado.Id);

            if (userInformation.sugar_score == null)
            {
                userInformation.sugar_score = 0;
            }

            int? min = userInformation.sugar_score - 500;
            int? max = userInformation.sugar_score + 500;

            var listUsers = _UserBsn.GetInformationByScore(min, max);
            query = (from c in _UserBsn.GetListDestaquesSugar(Genero) select c)
                        .Skip(pageIndex * pageSize)
                        .Take(pageSize).ToList();

            listaUsuarios = query;

            foreach (var item in listaUsuarios)
            {
                UserModel userAlter = new UserModel();
                userAlter = item;

                if (!string.IsNullOrEmpty(item.imagemPerfil))
                {
                    userAlter.imagemPerfil = item.imagemPerfil.Replace("\\", "/");
                }

                Aux.Add(userAlter);
            }

            ViewBag.Destaques = Aux;


            var usuarioAprovado = Convert.ToBoolean(TempData["tempUsuarioAprovado"]);
            TempData.Keep("tempUsuarioAprovado");
            ViewBag.usuarioAprovado = usuarioAprovado;

            return View();
        }

        public ActionResult _SugestaoPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            List<UserModel> listaUsuarios = new List<UserModel>();


            ViewBag.PromotionalCode = usuarioLogado.CodigoConvite;

            string Genero = "";


            if (usuarioLogado.Genero == "Homem")
            {
                Genero = "Mulher";
                ViewBag.Subtitulo = "Novas";
            }
            else
            {
                Genero = "Homem";
                ViewBag.Subtitulo = "Novos";
            }


            listaUsuarios = _UserBsn.GetListStoriesSugar(Genero).Take(10).ToList();

            ViewBag.Destaques = listaUsuarios;

            return View();
        }

        public ActionResult _ComplementoPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ViewBag.ComplementoCadastrado = usuarioLogado.ComplementoCadastrado;
            ViewBag.InserirImagemPerfil = false;
            ViewBag.InserirCidade = false;


            if (usuarioLogado.imagemPerfil == " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png" || usuarioLogado.imagemPerfil == "http://Kinkee.me/pulsar/Kinkee/assets/images/modules/sobre/profile.png")
            {
                ViewBag.InserirImagemPerfil = true;
            }
            else
            {
                ViewBag.InserirImagemPerfil = false;
            }


            if (usuarioLogado.Endereco != null)
            {
                if (String.IsNullOrEmpty(usuarioLogado.Endereco.Cidade))
                {
                    ViewBag.InserirCidade = true;
                }
                else
                {
                    ViewBag.InserirCidade = false;
                }
            }
            else
            {
                ViewBag.InserirCidade = false;
            }


            return View();
        }

        public ActionResult _PerfilListPartial(UserModel userModel)
        {
            var teste = userModel;
            return View();
        }

        public ActionResult _PerfilItemPartial()
        {
            var usuarioAprovado = Convert.ToBoolean(TempData["tempUsuarioAprovado"]);
            TempData.Keep("tempUsuarioAprovado");

            ViewBag.usuarioAprovado = usuarioAprovado;

            return View();
        }

        public string CadastrarInformacoes(string Genero, string TipoSugar, string TenhoInteresse)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var retorno = "true";

            try
            {
                var usuarioLogado = _UserBsn.GetUserByUsuario(u);
                usuarioLogado.Genero = Genero;
                usuarioLogado.TipoSugar = TipoSugar;
                usuarioLogado.TenhoInteresse = TenhoInteresse;
                usuarioLogado.ComplementoCadastrado = true;

                _UserBsn.EditarUsuario(usuarioLogado);
            }
            catch
            {
                retorno = "false";
            }



            return retorno;
        }

        public Double PegarSaldo()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ProcessSwitcher processSwitcher = new ProcessSwitcher();
            return processSwitcher.GetSaldoWallet(usuarioLogado.Id);
        }

        public int PegarNovasNotificacoes()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            return _notificationsBSN.GetCountNotificationByUser(usuarioLogado.Id.ToString(), Notificationtype.NotificacaoGeral);
        }

        public int PegarQuantidadeNovasMensagens()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            return _chatBSN.GetCountMessagesNotReadByUser(usuarioLogado.Id);
        }

        public int PegarQuantidadeNovasSolicitacoes()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            return _relationShipBSN.GetCountRequestNotificationsByUser(usuarioLogado.Id.ToString());
        }

        public void MarcaNotificacoesComoLidas()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            _notificationsBSN.UpdateReadNotifications(usuarioLogado.Id.ToString(), Notificationtype.NotificacaoGeral);
        }

        public void MarcaFriedsRequestComoLidas()
        {
            RelationShipBSN _relationShipBSN = new RelationShipBSN();

            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            _relationShipBSN.UpdateReadFriedRequestNotifications(usuarioLogado.Id.ToString());
        }
    }
}