using ImageResizer;
using Miscellaneous.ELOCalulate;
using Miscellaneous.SugarScore;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using Mongo.Services;
using MongoDB.Bson;
using Plataforma.Helper;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    [Authorize]
    public partial class DatingController : Controller
    {
        UserBSN Usuario = new UserBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        LocationHelper locationHelper = new LocationHelper();

        // GET: Perfil
        public ActionResult _Perfil()
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var user = uri.PathAndQuery.Split('/').Last();
            var usuarioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuario = new UserModel();

            SharedViewModel model = new SharedViewModel();
            PerfilViewModel modelPerfil = new PerfilViewModel();
            

            ViewBag.UsuarioLogadoContaGold = false;
            ViewBag.Genero = "";
            var enviaEmail = true;


            try
            {
                UserModel modelUsuario = new UserModel();


                if (user.ToUpper() == usuarioLogado.ToUpper() || String.IsNullOrEmpty(user) || user.ToUpper() == "PERFIL")
                {
                    modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                    varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                    ViewBag.habilitaEdicao = true;
                    enviaEmail = false;
                    ViewBag.Gold = true;
                    ViewBag.UsuarioLogadoFotoPrivada = true;
                    ViewBag.Adm = varUsuario.Adm;
                }
                else
                {
                    var usuarioAprovado = Convert.ToBoolean(TempData["tempUsuarioAprovado"]);
                    TempData.Keep("tempUsuarioAprovado");
                    if (!usuarioAprovado)
                    {
                        return View("Index");
                    }

                    modelUsuario.Usuario = user;
                    varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                    ViewBag.habilitaEdicao = false;
                }
                   
                if(varUsuario == null)
                {
                    return View();
                }

                if (varUsuario.visitasPerfil == null)
                {
                    modelPerfil.visitasPerfil = 0;
                }
                else
                {
                    modelPerfil.visitasPerfil = varUsuario.visitasPerfil;
                }

                if (varUsuario.imagemPerfil != null)
                {
                    modelPerfil.imagemPerfil = varUsuario.imagemPerfil.Replace("\\", "/");
                }

                var information = _UserBsn.GetInformationByUserId(varUsuario.Id);

                modelPerfil.Name = varUsuario.Name;
                modelPerfil.Lastname = varUsuario.Lastname;
                modelPerfil.Descricao_Curta = varUsuario.Descricao;

                
                

                if (information != null)
                {
                    modelPerfil.Descricao_Longa = varUsuario.Genero == "Homem" ? (String.IsNullOrEmpty(information.DescricaoGenerosidade) ? "" : information.DescricaoGenerosidade) : (String.IsNullOrEmpty(information.DescricaoMotivoBaby) ? "" : information.DescricaoMotivoBaby);
                    modelPerfil.Localizacao_Estado = information.Admin1_str_name;
                    modelPerfil.Localizacao_Cidade = information.Feature_str_name;


                    modelPerfil.Idade = CommonHelper.CalculateAge(varUsuario.DataAniversario).ToString();
                    modelPerfil.OrientacaoSexual = information.OrientacaoSexual;
                    modelPerfil.Profissao = information.NomeGrupoProfissao;
                    modelPerfil.EstadoCivil = information.EstadoCivil;
                    modelPerfil.Signo = information.Signo;
                    modelPerfil.Etnia = information.Etnia;
                    modelPerfil.Cabelos = information.Cabelos;
                    modelPerfil.TipoCabelos = information.TipoCabelos;
                    modelPerfil.Olhos = information.Olhos;
                    modelPerfil.Altura = information.Altura;
                    modelPerfil.Peso = information.Peso;
                    modelPerfil.Corpo = information.Corpo;
                    modelPerfil.Fuma = information.Fuma;
                    modelPerfil.Bebe = information.Bebe;


                    if (information.sugar_score == null)
                    {
                        modelPerfil.url_image_score = Scores.PegarEloGenero(null, varUsuario.Genero).urlImagemElo;
                        modelPerfil.name_sugar_score = Scores.PegarEloGenero(null, varUsuario.Genero).nomeElo;
                        modelPerfil.sugar_score = 0;
                    }
                    else
                    {
                        modelPerfil.url_image_score = Scores.PegarEloGenero(information.sugar_score, varUsuario.Genero).urlImagemElo;
                        modelPerfil.name_sugar_score = Scores.PegarEloGenero(information.sugar_score, varUsuario.Genero).nomeElo;
                        modelPerfil.sugar_score = information.sugar_score;
                    }

                }
                else
                {
                    modelPerfil.Descricao_Longa = "";
                    modelPerfil.Localizacao_Estado = "Localizando...";
                    modelPerfil.Localizacao_Cidade = "";


                    modelPerfil.url_image_score = Scores.PegarEloGenero(null, varUsuario.Genero).urlImagemElo;
                    modelPerfil.name_sugar_score = Scores.PegarEloGenero(null, varUsuario.Genero).nomeElo;
                    modelPerfil.sugar_score = 0;
                }
                

                modelPerfil.Usuario = varUsuario.Usuario;
                modelPerfil.ContaGold = varUsuario.ContaGold;
                modelPerfil.ContaSelectBlack = varUsuario.ContaSelectBlack;
                modelPerfil.PerfilVerificado = varUsuario.PerfilVerificado;
                modelPerfil.PerfilTop = varUsuario.PerfilTop;
                modelPerfil.PromotionalCode = varUsuario.CodigoConvite;
                modelPerfil.Genero = varUsuario.Genero;
                modelPerfil.TipoSugar = varUsuario.TipoSugar;
                modelPerfil.TenhoInteresse = varUsuario.TenhoInteresse;
                modelPerfil.Adm = varUsuario.Adm;



                //if(MOSTRAR NOME)
                //{
                //    modelPerfil.NomeCompleto = varUsuario.Name +" "+ varUsuario.Lastname;
                //}
                //else
                //{
                //    modelPerfil.NomeCompleto = " - ";
                //}

                

                model.PerfilViewModel = modelPerfil;

                if (varUsuario.GaleriaFotos != null)
                {
                    ViewBag.GaleriaFotos = varUsuario.GaleriaFotos;
                }
                else
                {
                    ViewBag.GaleriaFotos = null;
                }

                PixBSN pixBSN = new PixBSN();
                ViewBag.lisPix = pixBSN.PegarPixPorUsuarioPagador(varUsuario.Id);


                if (System.Web.HttpContext.Current.User.Identity.Name != modelPerfil.Usuario.ToUpper())
                {

                    UsuarioHelper.AddVisita(varUsuario, 1);

                    //Usuario Logado
                    modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                    var varUsuarioLogado = Usuario.GetUserByUsuario(modelUsuario);


                    //FOTO PRIVADA
                    if (varUsuario.PedidosVisualizarFtPrivadas != null)
                    {
                        if (varUsuario.PedidosVisualizarFtPrivadas.Count > 0)
                        {
                            var usuarioAcessado = varUsuario.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == varUsuarioLogado.Id.ToString()).FirstOrDefault();

                            if (usuarioAcessado != null)
                            {
                                ViewBag.UsuarioLogadoFotoPrivada = usuarioAcessado.Aprovado;
                            }
                            else
                            {
                                ViewBag.UsuarioLogadoFotoPrivada = false;
                            }
                        }
                        else
                        {
                            ViewBag.UsuarioLogadoFotoPrivada = false;
                        }

                    }
                    else
                    {
                        ViewBag.UsuarioLogadoFotoPrivada = false;
                    }

                    ViewBag.Adm = varUsuarioLogado.Adm;
                    ViewBag.UsuarioLogadoContaGold = varUsuarioLogado.ContaGold;
                    ViewBag.Genero = varUsuarioLogado.Genero;
                    ViewBag.UsuarioLogadoTipoSugar = varUsuarioLogado.TipoSugar;


                    

                    AfiliadosConfiguracaoPagamentoBSN afiliadosConfiguracaoPagamentoBSN = new AfiliadosConfiguracaoPagamentoBSN();
                    var configuracoesPagamento = afiliadosConfiguracaoPagamentoBSN.GetAllCofiguracaoPagamento(varUsuario.Id).FirstOrDefault();

                    if(configuracoesPagamento != null)
                    {
                        ViewBag.MostraBotaoEnviaPix = true;
                    }
                    else
                    {
                        ViewBag.MostraBotaoEnviaPix = false;
                    }
                    

                    if (ViewBag.UsuarioLogadoContaGold == null)
                    {
                        ViewBag.UsuarioLogadoContaGold = false;
                    }

                    ViewBag.Gold = varUsuarioLogado.ContaGold;

                    UsuarioHelper.AddPerfilVisita(varUsuario, varUsuarioLogado);

                    if (enviaEmail)
                    {
                        SendEmailAddress to = new SendEmailAddress();
                        to.Email = varUsuario.Email;
                        to.Nome = varUsuario.Name;

                        ProcessaEmails.SendMailVisitaRecebida(to, varUsuarioLogado.Usuario, varUsuarioLogado.imagemPerfil);


                        Plataforma.Notifications not = new Plataforma.Notifications();
                        List<string> PlayerIds = new List<string>();

                        if(varUsuario.Players == null)
                        {
                            varUsuario.Players = PlayerIds;
                        }

                        foreach (var player in varUsuario.Players)
                        {
                            if (!String.IsNullOrEmpty(player))
                            {
                                PlayerIds.Add(player);
                            }
                        }

                        if(PlayerIds.Count > 0)
                        {
                            not.CreateNotification(PlayerIds, "Você recebeu uma visita de " + varUsuarioLogado.Usuario, "https://app.kinkeesugar.com/Dating/Perfil/" + System.Web.HttpContext.Current.User.Identity.Name, "Visita Recebida");
                        }
                    }
                }
            }
            catch
            {

            }

            

            return View(model);
            //return Redirect("/Dating/Perfil/" + System.Web.HttpContext.Current.User.Identity.Name);
        }


        public ActionResult Perfil()
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var nomeUsuarioUrl = uri.PathAndQuery.Split('/').Last();
            var nomeUsuarioLogado = System.Web.HttpContext.Current.User.Identity.Name;

            SharedViewModel model = new SharedViewModel();

            UserModel consulta = new UserModel();
            consulta.Usuario = nomeUsuarioUrl;
            var usuarioPagina = Usuario.GetUserByUsuario(consulta);


            if (isUserLoggedInPage(usuarioPagina, nomeUsuarioLogado))
            {
                consulta.Usuario = nomeUsuarioLogado;
                usuarioPagina = Usuario.GetUserByUsuario(consulta);

                if (usuarioPagina == null)
                {
                    return View("Index");
                }

                ViewBag.habilitaEdicao = true;
                ViewBag.Gold = true;
                ViewBag.UsuarioLogadoFotoPrivada = true;
                ViewBag.Adm = usuarioPagina.Adm;


                var profileViewModel = modelViewProfileData(usuarioPagina);
                var information = _UserBsn.GetInformationByUserId(usuarioPagina.Id);
                profileViewModel = modeViewInformationData(profileViewModel, information);
                profileViewModel = modelViewFotoPrivadaData(profileViewModel, usuarioPagina);
                profileViewModel = modelViewMostraBotaoPix(profileViewModel);
                profileViewModel = modelViewPixData(profileViewModel);


                model.PerfilViewModel = profileViewModel;
            }
            else
            {
                consulta.Usuario = nomeUsuarioLogado;
                var usuarioLogado = Usuario.GetUserByUsuario(consulta);

                TempData["tempUsuarioAprovado"] = usuarioLogado.ApprovedProfile;
                var usuarioAprovado = Convert.ToBoolean(TempData["tempUsuarioAprovado"]);
                TempData.Keep("tempUsuarioAprovado");
                if (!usuarioAprovado)
                {
                    return View("Index");
                }

                ViewBag.habilitaEdicao = false;
                ViewBag.Adm = usuarioLogado.Adm;

                var profileViewModel = modelViewProfileData(usuarioPagina);
                var information = _UserBsn.GetInformationByUserId(usuarioPagina.Id);
                profileViewModel = modeViewInformationData(profileViewModel, information);
                profileViewModel = modelViewFotoPrivadaData(profileViewModel, usuarioPagina);
                profileViewModel = modelViewMostraBotaoPix(profileViewModel);
                profileViewModel = modelViewPixData(profileViewModel);

                //if(!usuarioLogado.Adm)
                //{
                modelViewNotificarVisita(usuarioPagina, usuarioLogado);
                //}


                model.PerfilViewModel = profileViewModel;
            }


            return View(model);
        }

        public bool isUserLoggedInPage(UserModel usuarioPagina, string usuarioLogado)
        {
            bool isUserLoggerInPage = true;

            if(usuarioPagina == null)
            {
                isUserLoggerInPage =  true;
            }
            else if(usuarioPagina.Usuario.ToUpper() == usuarioLogado.ToUpper() || String.IsNullOrEmpty(usuarioPagina.Usuario) || usuarioPagina.Usuario.ToUpper() == "PERFIL")
            {
                isUserLoggerInPage = true;
            }
            else
            {
                isUserLoggerInPage = false;
            }

            return isUserLoggerInPage;
        }

        public PerfilViewModel modelViewProfileData(UserModel usuarioPagina)
        {
            PerfilViewModel modelPerfil = new PerfilViewModel();

            modelPerfil.Id = usuarioPagina.Id;
            modelPerfil.Name = usuarioPagina.Name;
            modelPerfil.Lastname = usuarioPagina.Lastname;
            modelPerfil.Descricao_Curta = usuarioPagina.Descricao;
            modelPerfil.Usuario = usuarioPagina.Usuario;
            modelPerfil.ContaGold = usuarioPagina.ContaGold;
            modelPerfil.ContaSelectBlack = usuarioPagina.ContaSelectBlack;
            modelPerfil.PerfilVerificado = usuarioPagina.PerfilVerificado;
            modelPerfil.PerfilTop = usuarioPagina.PerfilTop;
            modelPerfil.PromotionalCode = usuarioPagina.CodigoConvite;
            modelPerfil.Genero = usuarioPagina.Genero;
            modelPerfil.TipoSugar = usuarioPagina.TipoSugar;
            modelPerfil.TenhoInteresse = usuarioPagina.TenhoInteresse;
            modelPerfil.Adm = usuarioPagina.Adm;
            modelPerfil.DataNascimento = usuarioPagina.DataAniversario;



            if (usuarioPagina.visitasPerfil == null)
            {
                modelPerfil.visitasPerfil = 0;
            }
            else
            {
                modelPerfil.visitasPerfil = usuarioPagina.visitasPerfil;
            }

            if (usuarioPagina.imagemPerfil != null)
            {
                modelPerfil.imagemPerfil = usuarioPagina.imagemPerfil.Replace("\\", "/");
            }

            if (usuarioPagina.GaleriaFotos != null)
            {
                List<GaleriaFotoViewModel> list = new List<GaleriaFotoViewModel>();

                foreach (var item in usuarioPagina.GaleriaFotos)
                {
                    GaleriaFotoViewModel galeriaFotoViewModel = new GaleriaFotoViewModel();
                    galeriaFotoViewModel.UrlFoto = item.UrlFoto;
                    galeriaFotoViewModel.isPrivate = item.isPrivate;
                    galeriaFotoViewModel.isApproved = item.isApproved;

                    list.Add(galeriaFotoViewModel);
                }

                modelPerfil.GaleriaFotos = list;
            }
            else
            {
                modelPerfil.GaleriaFotos = null;
            }


            if (usuarioPagina.PedidosVisualizarFtPrivadas != null)
            {
                List<PedidoUsuarioFotoPrivadaViewModel> list = new List<PedidoUsuarioFotoPrivadaViewModel>();

                foreach (var item in usuarioPagina.PedidosVisualizarFtPrivadas)
                {
                    PedidoUsuarioFotoPrivadaViewModel pedidoFotoPrivadaView = new PedidoUsuarioFotoPrivadaViewModel();
                    pedidoFotoPrivadaView.idUsuario = item.idUsuario;
                    pedidoFotoPrivadaView.Aprovado = item.Aprovado;

                    list.Add(pedidoFotoPrivadaView);
                }

                modelPerfil.PedidosVisualizarFtPrivadas = list;
            }
            else
            {
                modelPerfil.PedidosVisualizarFtPrivadas = null;
            }

            return modelPerfil;
        }

        public PerfilViewModel modeViewInformationData(PerfilViewModel modelPerfil,  UserInformationModel information)
        {
            if (information != null)
            {
                modelPerfil.Descricao_Longa = modelPerfil.Genero == "Homem" ? (String.IsNullOrEmpty(information.DescricaoGenerosidade) ? "" : information.DescricaoGenerosidade) : (String.IsNullOrEmpty(information.DescricaoMotivoBaby) ? "" : information.DescricaoMotivoBaby);
                modelPerfil.Localizacao_Estado = information.Admin1_str_name;
                modelPerfil.Localizacao_Cidade = information.Feature_str_name;


                modelPerfil.Idade = CommonHelper.CalculateAge(modelPerfil.DataNascimento).ToString();
                modelPerfil.OrientacaoSexual = information.OrientacaoSexual;
                modelPerfil.Profissao = information.NomeGrupoProfissao;
                modelPerfil.EstadoCivil = information.EstadoCivil;
                modelPerfil.Signo = information.Signo;
                modelPerfil.Etnia = information.Etnia;
                modelPerfil.Cabelos = information.Cabelos;
                modelPerfil.TipoCabelos = information.TipoCabelos;
                modelPerfil.Olhos = information.Olhos;
                modelPerfil.Altura = information.Altura;
                modelPerfil.Peso = information.Peso;
                modelPerfil.Corpo = information.Corpo;
                modelPerfil.Fuma = information.Fuma;
                modelPerfil.Bebe = information.Bebe;


                if (information.sugar_score == null)
                {
                    modelPerfil.url_image_score = Scores.PegarEloGenero(null, modelPerfil.Genero).urlImagemElo;
                    modelPerfil.name_sugar_score = Scores.PegarEloGenero(null, modelPerfil.Genero).nomeElo;
                    modelPerfil.sugar_score = 0;
                }
                else
                {
                    modelPerfil.url_image_score = Scores.PegarEloGenero(information.sugar_score, modelPerfil.Genero).urlImagemElo;
                    modelPerfil.name_sugar_score = Scores.PegarEloGenero(information.sugar_score, modelPerfil.Genero).nomeElo;
                    modelPerfil.sugar_score = information.sugar_score;
                }

            }
            else
            {
                modelPerfil.Descricao_Longa = "";
                modelPerfil.Localizacao_Estado = "Localizando...";
                modelPerfil.Localizacao_Cidade = "";


                modelPerfil.url_image_score = Scores.PegarEloGenero(null, modelPerfil.Genero).urlImagemElo;
                modelPerfil.name_sugar_score = Scores.PegarEloGenero(null, modelPerfil.Genero).nomeElo;
                modelPerfil.sugar_score = 0;
            }

            return modelPerfil;
        }

        public PerfilViewModel modelViewPixData(PerfilViewModel modelPerfil)
        {
            PixBSN pixBSN = new PixBSN();

            var listaBanco = pixBSN.PegarPixPorUsuarioPagador(modelPerfil.Id);
            List<PixViewModel> listaPix = new List<PixViewModel>();


            foreach (var item in listaBanco)
            {
                PixViewModel pixViewModel = new PixViewModel();

                pixViewModel.valor = item.DadosQRCode.valor;

                listaPix.Add(pixViewModel);
            }

            modelPerfil.ListaPix = listaPix;

            return modelPerfil;
        }

        public static T ConvertValue<T, U>(U value) where U : IConvertible
        {
            return (T)ConvertValue(value, typeof(T));
        }

        public static object ConvertValue(IConvertible value, Type targetType)
        {
            return Convert.ChangeType(value, targetType);
        }

        public PerfilViewModel modelViewFotoPrivadaData(PerfilViewModel modelPerfil, UserModel usuarioLogado)
        {
            if (modelPerfil.PedidosVisualizarFtPrivadas != null)
            {
                if (modelPerfil.PedidosVisualizarFtPrivadas.Count > 0)
                {
                    var usuarioAcessado = modelPerfil.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == usuarioLogado.Id.ToString()).FirstOrDefault();

                    if (usuarioAcessado != null)
                    {
                        modelPerfil.UsuarioLogadoFotoPrivada = usuarioAcessado.Aprovado;
                    }
                    else
                    {
                        modelPerfil.UsuarioLogadoFotoPrivada = false;
                    }
                }
                else
                {
                    modelPerfil.UsuarioLogadoFotoPrivada = false;
                }

            }
            else
            {
                modelPerfil.UsuarioLogadoFotoPrivada = false;
            }

            return modelPerfil;
        }

        public PerfilViewModel modelViewMostraBotaoPix(PerfilViewModel modelPerfil)
        {
            AfiliadosConfiguracaoPagamentoBSN afiliadosConfiguracaoPagamentoBSN = new AfiliadosConfiguracaoPagamentoBSN();
            var configuracoesPagamento = afiliadosConfiguracaoPagamentoBSN.GetAllCofiguracaoPagamento(modelPerfil.Id).FirstOrDefault();

            if (configuracoesPagamento != null)
            {
                modelPerfil.MostraBotaoEnviaPix = true;
            }
            else
            {
                modelPerfil.MostraBotaoEnviaPix = false;
            }

            return modelPerfil;
        }

        public void modelViewNotificarVisita(UserModel usuarioPagina, UserModel usuarioLogado)
        {
            UsuarioHelper.AddPerfilVisita(usuarioPagina, usuarioLogado);

            SendEmailAddress to = new SendEmailAddress();
            to.Email = usuarioPagina.Email;
            to.Nome = usuarioPagina.Name;

            ProcessaEmails.SendMailVisitaRecebida(to, usuarioLogado.Usuario, usuarioLogado.imagemPerfil);

            Plataforma.Notifications not = new Plataforma.Notifications();
            List<string> PlayerIds = new List<string>();

            if (usuarioPagina.Players == null)
            {
                usuarioPagina.Players = PlayerIds;
            }

            foreach (var player in usuarioPagina.Players)
            {
                if (!String.IsNullOrEmpty(player))
                {
                    PlayerIds.Add(player);
                }
            }

            if (PlayerIds.Count > 0)
            {
                not.CreateNotification(PlayerIds, "Você recebeu uma visita de " + usuarioLogado.Usuario, "https://app.kinkeesugar.com/Dating/Perfil/" + usuarioLogado.Usuario, "Visita Recebida");
            }
        }



        public ActionResult _ProfileModalCertificado()
        {
            MinhaContaController assinatura = new MinhaContaController();
            ViewBag.Assinatura = assinatura.ValidarAssinatura();

            return View();
        }

        public ActionResult _ProfileModalClassificacaoHomem()
        {
            MinhaContaController assinatura = new MinhaContaController();
            ViewBag.Assinatura = assinatura.ValidarAssinatura();

            return View();
        }

        public ActionResult _ProfileModalClassificacaoMulher()
        {
            MinhaContaController assinatura = new MinhaContaController();
            ViewBag.Assinatura = assinatura.ValidarAssinatura();

            return View();
        }


        public ActionResult _EditarPerfilModal()
        {
            PerfilViewModel modelPerfil = new PerfilViewModel();
            try
            {
                UserModel modelUsuario = new UserModel();
                //UserModel usuarioRetorno = null;
                modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                var information = Usuario.GetInformationByUserId(varUsuario.Id);


                modelPerfil.Name = varUsuario.Name;
                modelPerfil.Lastname = varUsuario.Lastname;
                modelPerfil.Descricao_Curta = varUsuario.Descricao;
                modelPerfil.Descricao_Longa = varUsuario.Genero == "Homem" ? (String.IsNullOrEmpty(information.DescricaoGenerosidade) ? "" : information.DescricaoGenerosidade) : (String.IsNullOrEmpty(information.DescricaoMotivoBaby) ? "" : information.DescricaoMotivoBaby);

                modelPerfil.Usuario = varUsuario.Usuario;
                modelPerfil.ContaGold = varUsuario.ContaGold;
                modelPerfil.ContaSelectBlack = varUsuario.ContaSelectBlack;
                modelPerfil.PerfilVerificado = varUsuario.PerfilVerificado;
                modelPerfil.PerfilTop = varUsuario.PerfilTop;
                modelPerfil.PromotionalCode = varUsuario.CodigoConvite;
                modelPerfil.Genero = varUsuario.Genero;
                modelPerfil.TipoSugar = varUsuario.TipoSugar;
                modelPerfil.TenhoInteresse = varUsuario.TenhoInteresse;
                modelPerfil.Adm = varUsuario.Adm;

                modelPerfil.Idade = CommonHelper.CalculateAge(varUsuario.DataAniversario).ToString();

                modelPerfil.DataNascimento = varUsuario.DataAniversario;
                modelPerfil.OrientacaoSexual = information.OrientacaoSexual;
                
                modelPerfil.EstadoCivil = information.EstadoCivil;
                modelPerfil.Signo = information.Signo;
                modelPerfil.Etnia = information.Etnia;
                modelPerfil.Cabelos = information.Cabelos;
                modelPerfil.TipoCabelos = information.TipoCabelos;
                modelPerfil.Olhos = information.Olhos;
                modelPerfil.Altura = information.Altura;
                modelPerfil.Peso = information.Peso;
                modelPerfil.Corpo = information.Corpo;
                modelPerfil.Fuma = information.Fuma;
                modelPerfil.Bebe = information.Bebe;

                CreateProfileModel createProfileModel = new CreateProfileModel();

                createProfileModel.AceitoOsTermos = information.AceitoOsTermos;

                createProfileModel.Country_str_code = information.Country_str_code;
                createProfileModel.Admin1_str_code = information.Admin1_str_code;
                createProfileModel.Admin1_str_name = information.Admin1_str_name;
                createProfileModel.Feature_int_id = information.Feature_int_id;
                createProfileModel.Feature_str_name = information.Feature_str_name;

                createProfileModel.ApprovedProfile = information.ApprovedProfile;
                
                createProfileModel.DescricaoGenerosidade = information.DescricaoGenerosidade;
                createProfileModel.DescricaoMotivoBaby = information.DescricaoMotivoBaby;
                createProfileModel.DescricaoPatrimonio = information.DescricaoPatrimonio;
                createProfileModel.DescricaoRelacionamento = information.DescricaoRelacionamento;
                createProfileModel.DescricaoRenaMensal = information.DescricaoRenaMensal;
                createProfileModel.DesejoReceberComunicacao = information.DesejoReceberComunicacao;
                createProfileModel.DetalheGenerosidade = information.DetalheGenerosidade;
                createProfileModel.DetalheMotivoBaby = information.DetalheMotivoBaby;
                createProfileModel.DisponibilidadeViagens = information.DisponibilidadeViagens;
                
                createProfileModel.Finalizado = information.Finalizado;
                createProfileModel.Foto = information.Foto;
                createProfileModel.GeneroUsuario = information.GeneroUsuario;
                createProfileModel.IdGenerosidade = information.IdGenerosidade;
                createProfileModel.IdGrupoProfissao = information.IdGrupoProfissao;
                createProfileModel.IdMotivoBaby = information.IdMotivoBaby;
                createProfileModel.IdPatrimonio = information.IdPatrimonio;
                createProfileModel.IdProfissao = information.IdProfissao;
                createProfileModel.IdRenaMensal = information.IdRenaMensal;
                createProfileModel.LinkFacebook = information.LinkFacebook;
                createProfileModel.LinkInstagram = information.LinkInstagram;
                createProfileModel.NomeGrupoProfissao = information.NomeGrupoProfissao;
                createProfileModel.NomeProfissao = information.NomeProfissao;
                createProfileModel.NomeStatusRelacionamento = information.NomeStatusRelacionamento;
                createProfileModel.NomeUsuario = information.NomeUsuario;
                createProfileModel.PrimeiroNome = information.PrimeiroNome;
                createProfileModel.ProfileCreated = information.ProfileCreated;
                createProfileModel.StatusRelacionamento = information.StatusRelacionamento;

                modelPerfil.createProfileModel = createProfileModel;

                ViewData["Countries"] = new SelectList(locationHelper.ListaPaises(), "Country_str_code", "Country_str_name");


                if (varUsuario.imagemPerfil != null)
                {
                    modelPerfil.imagemPerfil = varUsuario.imagemPerfil.Replace("\\", "/");
                }

                ViewBag.InserirImagemPerfil = false;
                ViewBag.InserirCidade = false;

                if (varUsuario.imagemPerfil == " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png" || varUsuario.imagemPerfil == "http://Kinkee.me/pulsar/Kinkee/assets/images/modules/sobre/profile.png")
                {
                    ViewBag.InserirImagemPerfil = true;
                }
                else
                {
                    ViewBag.InserirImagemPerfil = false;
                }
            }
            catch
            {

            }


            return View(modelPerfil);
        }

        public ActionResult EnviarEdicaoPerfil(PerfilViewModel model)
        {
            try
            {
                UserModel modelUsuario = new UserModel();
                //UserModel usuarioRetorno = null;
                modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                var varUsuario = Usuario.GetUserByUsuario(modelUsuario);
                var information = Usuario.GetInformationByUserId(varUsuario.Id);


                varUsuario.Name = model.Name;
                varUsuario.Lastname = model.Lastname;
                varUsuario.Descricao = model.Descricao_Curta;


                if(model.createProfileModel.Foto != null)
                {
                    FileUploadController fileUpload = new FileUploadController();

                    var url = fileUpload.SalvaImagemRetornaURL(model.createProfileModel.Foto);

                    if (!String.IsNullOrEmpty(url))
                    {
                        varUsuario.imagemPerfilPrivado = url;
                    }
                }


                if(CommonHelper.CalculateAge(model.DataNascimento) >= 18 && CommonHelper.CalculateAge(model.DataNascimento) < 90)
                {
                    varUsuario.DataAniversario = model.DataNascimento;
                }
                

                if (varUsuario.Genero == "Homem")
                {
                    information.DescricaoGenerosidade = model.Descricao_Longa;
                }
                else
                {
                    information.DescricaoMotivoBaby = model.Descricao_Longa;
                }


                information.IdGrupoProfissao = model.createProfileModel.IdGrupoProfissao;
                information.NomeGrupoProfissao = model.createProfileModel.NomeGrupoProfissao;

                information.IdProfissao = model.createProfileModel.IdProfissao;
                information.NomeProfissao = model.createProfileModel.NomeProfissao;


                information.OrientacaoSexual = model.OrientacaoSexual;
                information.EstadoCivil = model.EstadoCivil;
                information.Signo = model.Signo;
                information.Etnia = model.Etnia;
                information.Cabelos = model.Cabelos;
                information.TipoCabelos = model.TipoCabelos;
                information.Olhos = model.Olhos;
                information.Altura = model.Altura;
                information.Peso = model.Peso;
                information.Corpo = model.Corpo;
                information.Fuma = model.Fuma;
                information.Bebe = model.Bebe;

                Usuario.AlterUserInformation(information);
                Usuario.EditarUsuario(varUsuario);
            }
            catch
            {

            }

            return RedirectToAction("Perfil", "Dating");
        }


        public string valorExpectativaRelacionamento(int ValorRenda)
        {
            string retorno = "";
            switch (ValorRenda)
            {
                case 1:
                    retorno = "Até R$ 500,00/mês";
                    break;
                case 2:
                    retorno = "Até R$ 1.500,00/mês";
                    break;
                case 3:
                    retorno = "Até R$ 2.000,00/mês";
                    break;
                case 4:
                    retorno = "Até R$ 2.500,00/mês";
                    break;
                case 5:
                    retorno = "Até R$ 3.000,00/mês";
                    break;
                case 6:
                    retorno = "Até R$ 3.500,00/mês";
                    break;
                case 7:
                    retorno = "Até R$ 4.000,00/mês";
                    break;
                case 8:
                    retorno = "Até R$ 5.000,00/mês";
                    break;
                case 10:
                    retorno = "Até R$ 5.000,00/mês";
                    break;
                case 11:
                    retorno = "Até R$ 5.000,00/mês";
                    break;
                case 12:
                    retorno = "Até R$ 5.000,00/mês";
                    break;
                case 13:
                    retorno = "Até R$ 5.000,00/mês";
                    break;
                case 9:
                    retorno = "Mais de R$ 10.000,00/mês";
                    break;

                default:
                    retorno = "Negociável";
                    break;

            }

            return retorno;
        }

        public SelectList Valores()
        {
            var valores = new SelectList(new List<SelectListItem>
                            {
                                new SelectListItem { Text = "Até 300", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 400", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 500", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 1000", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 2000", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 3000", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 4000", Value = (200).ToString()},
                                new SelectListItem { Text = "Até 5000", Value = (200).ToString()},
                                new SelectListItem { Text = "Mais 5000", Value = (200).ToString()}

                            }, "Value", "Text");


            return valores;
        }

        [HttpPost]
        public void AddUserFavorite(string nomeUsuarioFavorito)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            UserModel usuarioFavorito = new UserModel();
            usuarioFavorito.Usuario = nomeUsuarioFavorito;
            var varUsuarioFavorito = Usuario.GetUserByUsuario(usuarioFavorito);

            if(varUsuarioContexto.Favoritos == null)
            {
                varUsuarioContexto.Favoritos = new List<string>();
            }
            
            var favorito = varUsuarioContexto.Favoritos.Contains(varUsuarioFavorito.Id.ToString());

            if (!favorito)
            {
                varUsuarioContexto.Favoritos.Add(varUsuarioFavorito.Id.ToString());

                var usuarioLogadoInformation = Usuario.GetInformationByUserId(varUsuarioContexto.Id);
                EloHelper.AtualizaMMR(varUsuarioFavorito.Id, varUsuarioFavorito.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Basica, ELOVE.Acao.Positiva);
            }

            UsuarioHelper.AddAndRemoveUserFavorite(varUsuarioContexto);
        }


        [HttpPost]
        public void RemoveUserFavorite(string nomeUsuarioFavorito)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            UserModel usuarioFavorito = new UserModel();
            usuarioFavorito.Usuario = nomeUsuarioFavorito;
            var varUsuarioFavorito = Usuario.GetUserByUsuario(usuarioFavorito);

            if (varUsuarioContexto.Favoritos == null)
            {
                varUsuarioContexto.Favoritos = new List<string>();
            }

            var favorito = varUsuarioContexto.Favoritos.Contains(varUsuarioFavorito.Id.ToString());

            if (!favorito)
            {
                varUsuarioContexto.Favoritos.Add(varUsuarioFavorito.Id.ToString());

                var usuarioLogadoInformation = Usuario.GetInformationByUserId(varUsuarioContexto.Id);
                EloHelper.AtualizaMMR(varUsuarioFavorito.Id, varUsuarioFavorito.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Basica, ELOVE.Acao.Negativa);
            }

            UsuarioHelper.AddAndRemoveUserFavorite(varUsuarioContexto);
        }

        public void __RemoveUserFavorite(string nomeUsuarioFavorito)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);


            UserModel usuarioFavorito = new UserModel();
            usuarioFavorito.Usuario = nomeUsuarioFavorito;
            var varUsuarioFavorito = Usuario.GetUserByUsuario(usuarioContexto);

            varUsuarioContexto.Favoritos.Remove(varUsuarioFavorito.Id.ToString());

            UsuarioHelper.AddAndRemoveUserFavorite(varUsuarioContexto);
        }

        public void AddUserReport(string ReportedUser, string ReportedType, string ReportDetails, bool blockUser)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);


            UsuarioHelper.AddUserReport(varUsuarioContexto.Id.ToString(), ReportedUser, ReportedType, ReportDetails);

            if (blockUser)
            {
                AddUserBlocked(varUsuarioContexto.Id.ToString(), ReportedUser);
            }
        }

        public void AddUserBlocked(string varUsuarioContexto, string BlockedUser)
        {
            UsuarioHelper.AddUserBlocked(varUsuarioContexto, BlockedUser);
        }

        //public ActionResult MensagemModal()
        //{
        //    return View();
        //}

        public ActionResult MensagemModal(string NameUser)
        {
            ViewBag.Usuario = NameUser;


            return View();
        }

        public ActionResult UploadFotoModal()
        {
            return View();
        }

        #region Fotos Privadas

        public void SolicitarVisualizarFotoPrivada(string nomeUsuarioSolicitado)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            UserModel solicitadoPara = new UserModel();
            solicitadoPara.Usuario = nomeUsuarioSolicitado;
            var varSolicitadoPara = Usuario.GetUserByUsuario(solicitadoPara);

            PedidoUsuarioFotoPrivada pedido = new PedidoUsuarioFotoPrivada();
            pedido.idUsuario = varUsuarioContexto.Id.ToString();
            pedido.Aprovado = false;

            if (varSolicitadoPara.PedidosVisualizarFtPrivadas == null)
            {
                varSolicitadoPara.PedidosVisualizarFtPrivadas = new List<PedidoUsuarioFotoPrivada>();
                varSolicitadoPara.PedidosVisualizarFtPrivadas.Add(pedido);
            }
            else
            {
                var retorno = varSolicitadoPara.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == varUsuarioContexto.Id.ToString()).FirstOrDefault();

                if (retorno == null)
                {
                    varSolicitadoPara.PedidosVisualizarFtPrivadas.Add(pedido);
                }
            }

            UsuarioHelper.EditarUsuario(varSolicitadoPara);

            SharedViewModel model = new SharedViewModel();
            MessageViewModel mensagemModel = new MessageViewModel();

            mensagemModel.Mensagem = "MENSAGEM AUTOMATICA: Gostaria de ter acesso as suas fotos privadas. Va ate a sua tela de Seguranca e Privacidade > Fotos Privadas para dar acessos.";
            mensagemModel.Para = "/Perfil/" + nomeUsuarioSolicitado;

            model.MessageViewModel = mensagemModel;

            EnviarMensagem(model);
        }

        public void PermitirUsuarioFotoPrivada(string idUsuarioPedido)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            varUsuarioContexto.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == idUsuarioPedido).FirstOrDefault().Aprovado = true;

            UsuarioHelper.EditarUsuario(varUsuarioContexto);

            SharedViewModel model = new SharedViewModel();
            model.MessageViewModel.Mensagem = "TESTE";
            model.MessageViewModel.Para = "/Perfil/";

            EnviarMensagem(model);
        }

        public void TirarPermissaoUsuarioFotoPrivada(string idUsuarioPedido)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            varUsuarioContexto.PedidosVisualizarFtPrivadas.Where(p => p.idUsuario == idUsuarioPedido).FirstOrDefault().Aprovado = false;

            UsuarioHelper.EditarUsuario(varUsuarioContexto);
        }

        public List<UserModel> ListarUsuariorios()
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            List<UserModel> listaUsuarios = new List<UserModel>();

            foreach (var item in varUsuarioContexto.PedidosVisualizarFtPrivadas)
            {
                UserModel usuario = new UserModel();

                usuario = UsuarioHelper.GetUsuarioByID(item.idUsuario);

                listaUsuarios.Add(usuario);
            }

            return listaUsuarios;
        }

        #endregion

        #region RelationShip

        public bool RequestFriendShip(string NomeUsuario)
        {
            try
            {
                var friend = Usuario.GetUserByUsuario(NomeUsuario);
                var User = Usuario.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);

                var retorno = _relationShipBSN.InsertRelationShip(User.Id, friend.Id, StatusRelationShip.Pending, User.Id);
                if (retorno)
                {
                    SendEmailAddress to = new SendEmailAddress();
                    to.Email = friend.Email;
                    to.Nome = friend.Name;

                    ProcessaEmails.SendMailNovaSolicitacaoAmizade(to, User.Usuario, User.imagemPerfil);
                }


                return retorno;
            }
            catch
            {
                return false;
            }

        }

        public StatusRelationShip GetRelationShip(string FriendName)
        {
            var friend = Usuario.GetUserByUsuario(FriendName);
            var User = Usuario.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);

            var RelationShip = _relationShipBSN.GetRelationShip(User.Id, friend.Id, StatusRelationShip.All).StatusRelationShip;

            return RelationShip;
        }

        public RelationShipModel GetRelationShipModel(string FriendName)
        {
            var friend = Usuario.GetUserByUsuario(FriendName);
            var User = Usuario.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);

            var RelationShip = _relationShipBSN.GetRelationShip(User.Id, friend.Id, StatusRelationShip.All);

            return RelationShip;
        }

        public List<RelationShipModel> GetListRelationShipByUserID(Int16 StatusRelationShip)
        {
            var User = Usuario.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            return _relationShipBSN.GetListRelationShipByUserID(User.Id, (StatusRelationShip)StatusRelationShip);
        }

        [HttpPost]
        public JsonResult ResponseFriendRequest(string Responde, string Uid)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            var getRelation = GetRelationShipModel(usuarioContexto.Usuario);
            if (getRelation == null)
            {
                var id = ObjectId.Parse(Uid);

                var EnvioAmizade = _relationShipBSN.InsertRelationShip(varUsuarioContexto.Id, id, StatusRelationShip.Pending, usuarioContexto.Id);

                if (EnvioAmizade)
                {
                    var UserEmail = UsuarioHelper.GetUsuarioAtivoByObjetcID(id);

                    SendEmailAddress to = new SendEmailAddress();
                    to.Email = UserEmail.Email;
                    to.Nome = UserEmail.Name;

                    ProcessaEmails.SendMailRespostaSolicitacaoAmizade(to, varUsuarioContexto.Usuario, varUsuarioContexto.imagemPerfil);
                }
            }


            var statusRelationShip = StatusRelationShip.Declined;

            if (Responde == "1")
            {
                statusRelationShip = StatusRelationShip.Pending;
            }
            else if (Responde == "2")
            {
                statusRelationShip = StatusRelationShip.Accepted;
            }
            else if (Responde == "3")
            {
                statusRelationShip = StatusRelationShip.Declined;
            }

            var retorno = _relationShipBSN.UpdateResponseFriedRequest(Uid, statusRelationShip, varUsuarioContexto.Id.ToString());

            var resultado = new
            {
                Retorno = retorno
            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        public ActionResult _ProfileModalPix(string nomeUsuarioRecebedor)
        {
            ViewBag.nomeUsuarioRecebedor = nomeUsuarioRecebedor;

            return View();
        }

        public JsonResult ConsultaPixRecebido()
        {
            var recebedor = Usuario.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            PixBSN pixBSN = new PixBSN();

            PixModel pix = pixBSN.PegarPixPorUsuarioRecebedor(recebedor.Id).FirstOrDefault();

            if(pix != null)
            {
                var resultado = new
                {
                    Pix = pix,
                    IdPix = pix.Id.ToString()
                };

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var resultado = pix;

                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
            

            
        }

        public ActionResult _ProfileModalPixRecebimento(string valor, string IdPix)
        {
            PixBSN pixBSN = new PixBSN();
            var pix = pixBSN.PegarPixPorId(new ObjectId(IdPix));
            var pagador = Usuario.GetUsuarioByUserId(pix.UsuarioIdPagador);

            ViewBag.valor = Convert.ToDouble(valor.Replace(".",","));
            ViewBag.NomePagador = pagador.Usuario;
            ViewBag.IdPix = IdPix;

            return View();
        }

        public void ConfirmarPixRecebido(bool confirmado, string IdPix)
        {
            PixBSN pixBSN = new PixBSN();
            var pix = pixBSN.PegarPixPorId(new ObjectId(IdPix));

            if(confirmado)
            {
                pix.ConfirmacaoPagamento = true;
                pixBSN.SalvarPix(pix);
            }
            else
            {
                pixBSN.RemoverPixPorId(pix.Id);
            }
        }

        [HttpPost]
        public JsonResult GerarQrCode(string valor, string NomeUsuarioRecebedor)
        {
            AfiliadosConfiguracaoPagamentoBSN afiliadosConfiguracaoPagamentoBSN = new AfiliadosConfiguracaoPagamentoBSN();

            var pagador = Usuario.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            var recebedor = Usuario.GetUserByUsuario(NomeUsuarioRecebedor);
            var infos = Usuario.GetInformationByUserId(recebedor.Id);
            var configuracoesPagamento = afiliadosConfiguracaoPagamentoBSN.GetAllCofiguracaoPagamento(recebedor.Id).FirstOrDefault();

            QrCode qrCode = new QrCode();
            QrCodeEstaticoModel qrCodeEstaticoModel = new QrCodeEstaticoModel();
            bool chaveCriada = false;

            if (configuracoesPagamento == null)
            {
                qrCodeEstaticoModel.chave = "";
                chaveCriada = false;
            }
            else
            {
                chaveCriada = true;
                qrCodeEstaticoModel.chave = configuracoesPagamento.ChavePix;
            }

            qrCodeEstaticoModel.nome = recebedor.Name+" "+ recebedor.Lastname;
            qrCodeEstaticoModel.cidade = infos.Feature_str_name;
            qrCodeEstaticoModel.valor = Convert.ToDouble(valor);
            qrCodeEstaticoModel.txid = "KINKEE-PIX-";
            qrCodeEstaticoModel.saida = SaidaPix.br;

            var codigo = qrCode.RequestGeraQrCodeEstatico(qrCodeEstaticoModel);

            qrCodeEstaticoModel.saida = SaidaPix.qr;
            var imagem = qrCode.RequestGeraQrCodeEstatico(qrCodeEstaticoModel);


            var qrCodeCriado = Newtonsoft.Json.JsonConvert.DeserializeObject<CodigoBrModel>(codigo);

            

            if(chaveCriada)
            {
                PixBSN pixBSN = new PixBSN();
                PixModel pixModel = new PixModel();
                pixModel.UsuarioIdPagador = pagador.Id;
                pixModel.UsuarioIdRecebedor = recebedor.Id;
                pixModel.DadosQRCode = qrCodeEstaticoModel;
                pixModel.Codigo = qrCodeCriado.brcode;
                pixModel.Imagem = imagem;

                pixBSN.InsertPix(pixModel);
            }
            

            var resultado = new
            {
                Codigo = qrCodeCriado.brcode,
                Imagem = imagem,
                ChaveCriada = chaveCriada
            };

            return Json(resultado, JsonRequestBehavior.AllowGet);
        }


        #endregion
    }
}