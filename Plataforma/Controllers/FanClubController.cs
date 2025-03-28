using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ImageResizer;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using MongoDB.Bson;
using Plataforma.Models;

namespace Plataforma.Controllers
{
    [Authorize]
    public class FanClubController : Controller
    {
        UserBSN _UserBsn = new UserBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        LogImageBSN _logImage = new LogImageBSN();
        readonly String APIURL = "https://paymentsecure.kinkeesugar.com/";

        public ActionResult profile()
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var user = uri.PathAndQuery.Split('/').Last();

            SharedViewModel model = new SharedViewModel();
            PerfilViewModel modelPerfil = new PerfilViewModel();

            bool habilitaEdicao = false;

            ViewBag.VisaoAceosAmizade = false;
            ViewBag.AssinaturaDeFan = false;
            try
            {
                UserModel modelUsuario = new UserModel();

                modelUsuario.Usuario = user;

                var varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);

                if (varUsuario == null)
                {
                    modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                    varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);
                    ViewBag.VisaoAceosAmizade = false;
                }


                if (varUsuario.visitasPerfil == null)
                {
                    modelPerfil.visitasPerfil = 0;
                }
                else
                {
                    modelPerfil.visitasPerfil = varUsuario.visitasPerfil;
                }


                Plataforma.Models.Endereco endereco = new Plataforma.Models.Endereco();
                if (varUsuario.Endereco != null)
                {
                    endereco.CEP = varUsuario.Endereco.CEP;
                    endereco.Estado = varUsuario.Endereco.Estado;
                    endereco.Cidade = varUsuario.Endereco.Cidade;
                    endereco.Bairro = varUsuario.Endereco.Bairro;
                    endereco.Endereço = varUsuario.Endereco.Endereço;

                    modelPerfil.Endereco = endereco;
                }


                if (varUsuario.imagemPerfil != null)
                {
                    modelPerfil.imagemPerfil = varUsuario.imagemPerfil.Replace("\\", "/");
                }


                modelPerfil.Name = varUsuario.Name;
                modelPerfil.Lastname = varUsuario.Lastname;
                //modelPerfil.Descricao = varUsuario.Descricao;
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

                model.PerfilViewModel = modelPerfil;


                if (varUsuario.GaleriaFotos != null)
                {
                    ViewBag.GaleriaFotos = varUsuario.GaleriaFotos;
                }
                else
                {
                    ViewBag.GaleriaFotos = null;
                }


                if (System.Web.HttpContext.Current.User.Identity.Name == modelUsuario.Usuario.ToUpper())
                {
                    habilitaEdicao = true;
                    ViewBag.Gold = true;
                    ViewBag.UsuarioLogadoFotoPrivada = true;
                    ViewBag.Adm = varUsuario.Adm;
                    ViewBag.AssinaturaDeFan = true;
                    ViewBag.DescricaoPerfilPrivado = varUsuario.DescricaoPerfilPrivado;
                    ViewBag.ImagemCapaPrivado = varUsuario.imagemCapaPrivado;
                }
                else
                {


                    //ViewBag.RelationShip = GetRelationShipModel(modelUsuario.Usuario);

                    ViewBag.DescricaoPerfilPrivado = modelUsuario.DescricaoPerfilPrivado;
                    ViewBag.ImagemCapaPrivado = modelUsuario.imagemCapaPrivado;

                    if (ViewBag.RelationShip == null)
                    {
                        ViewBag.StatusRelationShip = 0;
                    }
                    else
                    {
                        ViewBag.StatusRelationShip = (byte)ViewBag.RelationShip.StatusRelationShip;
                    }
                    ViewBag.IdUsuarioComparation = varUsuario.Id;
                    ViewBag.VisaoAceosAmizade = true;

                    UsuarioHelper.AddVisita(varUsuario, 1);

                    //Usuario Logado
                    modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                    var varUsuarioLogado = _UserBsn.GetUserByUsuario(modelUsuario);
                    ViewBag.AssinaturaDeFan = VerificarAssinatura(varUsuarioLogado.Id, varUsuario.Id);

                    ViewBag.Adm = varUsuarioLogado.Adm;
                    ViewBag.UsuarioLogadoContaGold = varUsuarioLogado.ContaGold;
                    ViewBag.UsuarioLogadoTipoSugar = varUsuarioLogado.TipoSugar;

                    if (ViewBag.UsuarioLogadoContaGold == null)
                    {
                        ViewBag.UsuarioLogadoContaGold = false;
                    }

                    if (ViewBag.UsuarioLogadoTipoSugar == null)
                    {
                        ViewBag.UsuarioLogadoTipoSugar = false;
                    }

                    ViewBag.Gold = varUsuarioLogado.ContaGold;

                    UsuarioHelper.AddPerfilVisita(varUsuario, varUsuarioLogado);


                    if (varUsuarioLogado.Adm != true)
                    {
                        SendEmailAddress to = new SendEmailAddress();
                        to.Email = varUsuario.Email;
                        to.Nome = varUsuario.Name;

                        ProcessaEmails.SendMailVisitaPrivadaRecebida(to, varUsuario.Usuario, varUsuario.imagemPerfil);


                        Plataforma.Notifications not = new Plataforma.Notifications();
                        List<string> PlayerIds = new List<string>();
                        foreach (var player in varUsuario.Players)
                        {
                            if (!String.IsNullOrEmpty(player))
                            {
                                PlayerIds.Add(player);
                            }
                        }

                        not.CreateNotification(PlayerIds, "Tem alguém interessado em fazer uma assinatura " + varUsuarioLogado.Usuario, "https://app.kinkeesugar.com/fanclub/profile" + System.Web.HttpContext.Current.User.Identity.Name, "Visita Privada");
                    }

                }
            }
            catch
            {

            }

            ViewBag.habilitaEdicao = habilitaEdicao;


            return View(model);
            //return Redirect("/Dating/Perfil/" + System.Web.HttpContext.Current.User.Identity.Name);
        }

        public ActionResult _TimelinePartial()
        {
            ViewBag.NomeUsuario = System.Web.HttpContext.Current.User.Identity.Name;
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var user = uri.PathAndQuery.Split('/').Last();
            var Logado = System.Web.HttpContext.Current.User.Identity.Name;

            ViewBag.HabilitaEdicao = false;

            if (Logado.ToUpper() == user.ToUpper() || user.ToUpper() == "PROFILE")
            {
                ViewBag.HabilitaEdicao = true;
            }

            return View();
        }

        [HttpPost]
        public ActionResult _TimelinePartial(string nomeUsuario)
        {
            ViewBag.NomeUsuario = System.Web.HttpContext.Current.User.Identity.Name;
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = nomeUsuario.Replace("\n", "").Replace(" ", "");

            var varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);
            var Logado = System.Web.HttpContext.Current.User.Identity.Name;

            ViewBag.HabilitaEdicao = false;

            if (Logado.ToUpper() == varUsuario.Usuario.ToUpper())
            {
                ViewBag.HabilitaEdicao = true;
            }

            return View();
        }

        public ActionResult _BloqueioSemAssinaturaPartial()
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var user = uri.PathAndQuery.Split('/').Last();

            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = user;

            var varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);

            List<ProductSubscriptionModel> produtos = ProductHelper.GetListProductSubscriptionByUserIdStatus(varUsuario.Id, ProductStatus.Active);

            if (produtos.Count == 0)
            {
                ViewBag.Produtos = null;
            }
            else
            {
                ViewBag.Produtos = produtos;
            }

            ViewBag.Fas = varUsuario.visitasPerfil;
            ViewBag.PostsFotosVideos = varUsuario.GaleriaFotos.Count;

            return View();
        }

        public ActionResult _AssinaturasPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            var assinaturas = ProductHelper.GetListSubscriptionByUserIdStatus(usuarioLogado.Id, SubscriptionStatus.Active);
            List<UserModel> listaUsuarios = new List<UserModel>();


            foreach (var item in assinaturas)
            {
                var user = UsuarioHelper.GetUsuarioByObjetcID(item.Product.OwnerUserId);
                listaUsuarios.Add(user);
            }

            ViewBag.ListaUsuarios = listaUsuarios;

            return View();
        }

        public ActionResult _SugestaoAssinaturaPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            var assinaturas = ProductHelper.GetFirstProductSubscriptionActive();
            List<UserModel> listaUsuarios = new List<UserModel>();

            foreach (var item in assinaturas)
            {
                var user = UsuarioHelper.GetUsuarioByObjetcID(item.OwnerUserId);

                if(usuarioLogado.Genero == "Homem")
                {
                    if(usuarioLogado.Genero != user.Genero)
                    {
                        listaUsuarios.Add(user);
                    }
                }
                else
                {
                    listaUsuarios.Add(user);
                }
            }

            ViewBag.ListaUsuarios = listaUsuarios;

            return View();
        }


        public ActionResult _AssinaturasMeusFansPartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            var assinaturas = ProductHelper.GetListSubscriptionByUserIdStatus(usuarioLogado.Id, SubscriptionStatus.Active);
            List<UserModel> listaUsuarios = new List<UserModel>();


            foreach (var item in assinaturas)
            {
                var user = UsuarioHelper.GetUsuarioByObjetcID(item.Product.OwnerUserId);
                listaUsuarios.Add(user);
            }

            ViewBag.ListaUsuarios = listaUsuarios;

            return View();
        }

        [HttpPost]
        public ActionResult _GaleriaPartial(string nomeUsuario)
        {
            SharedViewModel model = new SharedViewModel();
            PerfilViewModel modelPerfil = new PerfilViewModel();

            bool habilitaEdicao = false;

            ViewBag.VisaoAceosAmizade = false;

            try
            {
                UserModel modelUsuario = new UserModel();

                modelUsuario.Usuario = nomeUsuario.Replace("\n", "").Replace(" ", "");

                var varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);

                if (varUsuario == null)
                {
                    modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

                    varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);
                    ViewBag.VisaoAceosAmizade = false;
                }


                if (varUsuario.visitasPerfil == null)
                {
                    modelPerfil.visitasPerfil = 0;
                }
                else
                {
                    modelPerfil.visitasPerfil = varUsuario.visitasPerfil;
                }


                Plataforma.Models.Endereco endereco = new Plataforma.Models.Endereco();
                if (varUsuario.Endereco != null)
                {
                    endereco.CEP = varUsuario.Endereco.CEP;
                    endereco.Estado = varUsuario.Endereco.Estado;
                    endereco.Cidade = varUsuario.Endereco.Cidade;
                    endereco.Bairro = varUsuario.Endereco.Bairro;
                    endereco.Endereço = varUsuario.Endereco.Endereço;

                    modelPerfil.Endereco = endereco;
                }


                if (varUsuario.imagemPerfil != null)
                {
                    modelPerfil.imagemPerfil = varUsuario.imagemPerfil.Replace("\\", "/");
                }


                modelPerfil.Name = varUsuario.Name;
                modelPerfil.Lastname = varUsuario.Lastname;
                //modelPerfil.Descricao = varUsuario.Descricao;
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

                model.PerfilViewModel = modelPerfil;


                if (varUsuario.GaleriaFotos != null)
                {
                    ViewBag.GaleriaFotos = varUsuario.GaleriaFotos;
                }
                else
                {
                    ViewBag.GaleriaFotos = null;
                }


                if (System.Web.HttpContext.Current.User.Identity.Name == varUsuario.Usuario.ToUpper())
                {
                    habilitaEdicao = true;
                    ViewBag.Gold = true;
                    ViewBag.UsuarioLogadoFotoPrivada = true;
                    ViewBag.AssinaturaDeFan = true;
                    ViewBag.Adm = varUsuario.Adm;

                    ViewBag.DescricaoPerfilPrivado = varUsuario.DescricaoPerfilPrivado;
                }
                else
                {
                    //ViewBag.RelationShip = GetRelationShipModel(modelUsuario.Usuario);

                    ViewBag.DescricaoPerfilPrivado = modelUsuario.DescricaoPerfilPrivado;

                    if (ViewBag.RelationShip == null)
                    {
                        ViewBag.StatusRelationShip = 0;
                    }
                    else
                    {
                        ViewBag.StatusRelationShip = (byte)ViewBag.RelationShip.StatusRelationShip;
                    }
                    ViewBag.IdUsuarioComparation = varUsuario.Id;
                    ViewBag.VisaoAceosAmizade = true;

                    UsuarioHelper.AddVisita(varUsuario, 1);

                    //Usuario Logado
                    modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                    var varUsuarioLogado = _UserBsn.GetUserByUsuario(modelUsuario);


                    ViewBag.Adm = varUsuarioLogado.Adm;
                    ViewBag.UsuarioLogadoContaGold = varUsuarioLogado.ContaGold;
                    ViewBag.UsuarioLogadoTipoSugar = varUsuarioLogado.TipoSugar;

                    if (ViewBag.UsuarioLogadoContaGold == null)
                    {
                        ViewBag.UsuarioLogadoContaGold = false;
                    }

                    if (ViewBag.UsuarioLogadoTipoSugar == null)
                    {
                        ViewBag.UsuarioLogadoTipoSugar = false;
                    }

                    ViewBag.Gold = varUsuarioLogado.ContaGold;

                    UsuarioHelper.AddPerfilVisita(varUsuario, varUsuarioLogado);

                    //if(System.Web.HttpContext.Current.User.Identity.Name != "RAPHAELSANTO")
                    //{
                    //    List<string> emailUsuario = new List<string>();
                    //    emailUsuario.Add(varUsuario.Email);
                    //    string urlAction = "https://kinkeesugar.com/Dating/Perfil/" + System.Web.HttpContext.Current.User.Identity.Name;
                    //    Email.SendMailNewVisit(emailUsuario, varUsuario.Usuario, urlAction, System.Web.HttpContext.Current.User.Identity.Name);
                    //}


                    //DESCOMENTAR CÓDIGO A SEGUIR QUANDO FOR HABILITAR ENVIO DE E-MAIL DE NOTIFICAÇÃO!
                    //List<string> emailUsuario = new List<string>();
                    //emailUsuario.Add(varUsuario.Email);
                    //string urlAction = "https://kinkeesugar.com/Dating/Perfil/" + System.Web.HttpContext.Current.User.Identity.Name;
                    //Email.SendMailNewVisit(emailUsuario, varUsuario.Usuario, urlAction, System.Web.HttpContext.Current.User.Identity.Name);


                    //List<string> emailUsuario = new List<string>();
                    //emailUsuario.Add(varUsuario.Email);


                    //Email.SendMailVisitaPerfil(emailUsuario, varUsuario.visitasPerfil);


                    //Plataforma.Notifications not = new Plataforma.Notifications();
                    //List<string> PlayerIds = new List<string>();
                    //foreach (var player in varUsuario.Players)
                    //{
                    //    if(!String.IsNullOrEmpty(player))
                    //    {
                    //        PlayerIds.Add(player);
                    //    }
                    //}

                    //not.CreateNotification(PlayerIds, "Você recebeu uma visita de " + varUsuarioLogado.Usuario, "https://kinkeesugar.com/Dating/Perfil/" + System.Web.HttpContext.Current.User.Identity.Name, "Visita Recebida");
                }
            }
            catch
            {

            }

            ViewBag.habilitaEdicao = habilitaEdicao;
            return View(model);
        }

        public ActionResult _ConfiguracoesPartial()
        {
            ViewBag.NomeUsuario = System.Web.HttpContext.Current.User.Identity.Name;
            UserModel modelUsuario = new UserModel();
            modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(modelUsuario);
            ViewBag.DescricaoPerfilPrivado = usuarioLogado.DescricaoPerfilPrivado;

            var dadosBancarios = UsuarioHelper.GetDadosBancarios(usuarioLogado.Id);

            if (dadosBancarios != null)
            {
                ViewBag.NomeCompleto = dadosBancarios.NomeCompleto;
                ViewBag.CPF = dadosBancarios.CPF;
                ViewBag.NomeBanco = dadosBancarios.NomeBanco;
                ViewBag.NumeroAgencia = dadosBancarios.NumeroAgencia;
                ViewBag.NumeroConta = dadosBancarios.NumeroConta;
            }
            else
            {
                ViewBag.NomeCompleto = "";
                ViewBag.CPF = "";
                ViewBag.NomeBanco = "";
                ViewBag.NumeroAgencia = "";
                ViewBag.NumeroConta = "";
            }


            ViewBag.ProductList = ProductHelper.GetListProductByUserOwnerId(usuarioLogado.Id);


            return View();
        }


        [HttpPost]
        public void EnviarDescricaoPerfilPrivado(string txtPublicacao)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            usuarioLogado.DescricaoPerfilPrivado = txtPublicacao;

            UsuarioHelper.SaveUser(usuarioLogado);
        }

        [HttpPost]
        public void EnviarDadosBancarios(string NomeCompleto,
                                                 string CPF,
                                                 string NomeBanco,
                                                 string TipoConta,
                                                 string NumeroAgencia,
                                                 string NumeroConta)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            var dadosBancarios = UsuarioHelper.GetDadosBancarios(usuarioLogado.Id);

            if (dadosBancarios == null)
            {
                dadosBancarios = new DadosBancariosModel()
                {
                    UserId = usuarioLogado.Id,
                    NomeCompleto = NomeCompleto,
                    CPF = CPF,
                    NomeBanco = NomeBanco,
                    TipoConta = TipoConta,
                    NumeroAgencia = NumeroAgencia,
                    NumeroConta = NumeroConta,
                };
            }


            UsuarioHelper.SaveDadosBancarios(dadosBancarios);
        }


        [HttpPost]
        public void SalvarNovoPlano(double ValorPlano, int TipoCobranca)
        {
            UserModel u = new UserModel();
            SubscriptionType type = (SubscriptionType)TipoCobranca;
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);
            var amount = ValorPlano;

            string periodicidade = "MONTHLY";
            string codeplan = "";

            switch (TipoCobranca)
            {
                case 0:
                    periodicidade = "WEEKLY";
                    break;
                case 1:
                    periodicidade = "MONTHLY";
                    break;
                case 2:
                    periodicidade = "BIMONTHLY";
                    break;
                case 3:
                    periodicidade = "TRIMONTHLY";
                    break;
                case 4:
                    periodicidade = "SEMIANNUALLY";
                    break;
                case 5:
                    periodicidade = "YEARLY";
                    break;
                default:
                    break;
            }

            if (TipoCobranca != 1 && usuarioLogado.ContaGold != true)
            {
                //
            }
            else
            {

                try
                {
                    string url = this.APIURL + "criaplano";
                    string data = "{\"planreference\":\" " + usuarioLogado.Usuario + ' ' + type.GetDescription() + " - R$" + amount.ToString("#.##") + "\",\"planname\":\" " + usuarioLogado.Usuario + ' ' + type.GetDescription() + " - R$" + amount.ToString("#.##") + "\",\"planperiod\": \"" + periodicidade + "\",\"expiration\":{\"planexpirationvalue\": \"10\",\"planexpirationiunit\": \"YEARS\"},\"planprice\": \"" + amount + ".00\"}";
                    WebRequest request = WebRequest.Create(url);
                    request.Method = "POST";
                    string postData = data;
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentType = "application/json; charset=UTF-8";
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                    WebResponse response = request.GetResponse();


                    using (dataStream = response.GetResponseStream())
                    {
                        StreamReader streamReader = new StreamReader(dataStream);
                        string responseFromServer = streamReader.ReadToEnd();
                        JavaScriptSerializer serializer = new JavaScriptSerializer();
                        dynamic item = serializer.Deserialize<object>(responseFromServer);
                        codeplan = item["code"];
                    }
                    response.Close();
                }
                catch (Exception ex)
                {

                }
            }


            var productSubscription = new ProductSubscriptionModel()
            {
                NameProduct = type.GetDescription() + " - R$" + amount.ToString("#.##"),
                OwnerUserId = usuarioLogado.Id,
                Type = type,
                Amount = amount,
                Status = String.IsNullOrEmpty(codeplan) ? usuarioLogado.ContaGold == false && TipoCobranca != 1 ? ProductStatus.Canceled : ProductStatus.InProvation : ProductStatus.Active,
                StatusComentary = String.IsNullOrEmpty(codeplan) ? "Para criar planos com cobrança diferente de mensal é necessário que você seja assinante Premium." : "Seu plano está ativo e já pode ser compartilhado.",
                HashPagSeguroPlan = codeplan
            };


            var retorno = ProductHelper.InsertProduct(productSubscription);

            if (retorno && productSubscription.Status != ProductStatus.Canceled)
            {


                //=====PARA LOG=====//
                SendEmailAddress paraRaphael = new SendEmailAddress();
                paraRaphael.Email = "raphael.esanto@gmail.com";
                paraRaphael.Nome = "Raphael Santo";
                ProcessaEmails.SendMailSemTempalte(paraRaphael, usuarioLogado.Usuario + " cadastrou um novo plano", productSubscription.NameProduct);
                //=====PARA LOG=====//
            }
        }

        [HttpPost]
        public void CompraAssinatura(string HashPagSeguro)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            //ObjectId objectId = ObjectId.Parse(idAssinatura);
            var produto = ProductHelper.GetProductByHashPagSeguroPlan(HashPagSeguro);
            int days = 0;


            switch (produto.Type)
            {
                case (SubscriptionType)0:
                    days = 7;
                    break;

                case (SubscriptionType)1:
                    days = 31;
                    break;

                case (SubscriptionType)2:
                    days = 60;
                    break;

                case (SubscriptionType)3:
                    days = 90;
                    break;

                case (SubscriptionType)4:
                    days = 180;
                    break;

                case (SubscriptionType)5:
                    days = 365;
                    break;
            }

            var newSubscription = new SubscriptionModel()
            {
                UserId = usuarioLogado.Id,
                Product = produto,
                Expire = DateTime.Now.AddDays(days),
                Status = SubscriptionStatus.InProcess
            };

            ProductHelper.InsertSubcription(newSubscription);
        }

        public bool VerificarAssinatura(ObjectId usuarioId, ObjectId usuarioVisitado)
        {
            List<SubscriptionModel> subscription = ProductHelper.GetListSubscriptionByUserId(usuarioId);

            bool retorno = false;

            foreach (var item in subscription)
            {
                if (item.Product.OwnerUserId == usuarioVisitado && item.Status == SubscriptionStatus.Active)
                {
                    retorno = true;
                    break;
                }
                else
                {
                    retorno = false;
                }
            }

            return retorno;
        }

        public RelationShipModel GetRelationShipModel(string FriendName)
        {
            var friend = _UserBsn.GetUserByUsuario(FriendName);
            var User = _UserBsn.GetUserByUsuario(System.Web.HttpContext.Current.User.Identity.Name);

            var RelationShip = _relationShipBSN.GetRelationShip(User.Id, friend.Id, StatusRelationShip.All);

            return RelationShip;
        }

        public ActionResult _PulsacaoPublicadaPartial(string UsuarioPublicacaoID, string Id)
        {
            var usuarioLogado = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            var Publicacao = PublicacaoPrivadaHelper.GetPublicacaoByID(Id);
            var usuarioPublicacao = UsuarioHelper.GetUsuarioByObjetcID(ObjectId.Parse(UsuarioPublicacaoID));
            ViewBag.HabilitaEdicaoExclusao = false;
            ViewBag.IdPublicacao = Id;
            ViewBag.TempodePublicacao = RetornaTempoDaInteracao((DateTime)Publicacao.DateCreate);
            ViewBag.KinkeeGoldValid = usuarioLogado.ContaGold;
            if (usuarioLogado.ContaGold)
            {
                ViewBag.KinkeeGold = "True";
            }
            else
            {
                ViewBag.KinkeeGold = "False";
            }


            if (Publicacao.Comentarios != null)
            {
                //Publicacao.Comentarios.Reverse();
                ViewBag.Comentarios = Publicacao.Comentarios;
            }
            else
            {
                List<UsuarioComentarioPublicacaoModel> comentarios = new List<UsuarioComentarioPublicacaoModel>();
                ViewBag.Comentarios = comentarios;
            }



            if (Publicacao.usuarioCurtiuPublicacao != null)
            {
                Publicacao.usuarioCurtiuPublicacao.Reverse();
                ViewBag.usuarioCurtiuPublicacao = Publicacao.usuarioCurtiuPublicacao;
            }
            else
            {
                List<string> imagensPerfilUsuarioLike = new List<string>();
                ViewBag.imagensPerfilUsuarioLike = imagensPerfilUsuarioLike;
            }


            if (Publicacao.Likes < 1)
            {
                ViewBag.likes = 0;
            }
            else
            {
                ViewBag.likes = Publicacao.Likes;
            }

            if (usuarioLogado.Id.ToString() == UsuarioPublicacaoID)
            {
                ViewBag.HabilitaEdicaoExclusao = true;
            }

            if (!String.IsNullOrEmpty(usuarioPublicacao.Name))
            {
                ViewBag.NomeSobrenome = "(" + usuarioPublicacao.Name + " " + usuarioPublicacao.Lastname + ")";
            }
            else
            {
                ViewBag.NomeSobrenome = "";
            }

            ViewBag.Usuario = usuarioPublicacao.Usuario;

            ViewBag.imagemPerfil = usuarioPublicacao.imagemPerfil;
            ViewBag.Publicacao = Publicacao.Publicacao;


            return View();
        }

        public ActionResult TodasAsPublicacoesAtivasDoUsuario(string usuario, int pageIndex, int pageSize, int ordenacao)
        {
            var user = Regex.Replace(usuario, @"\t|\n|\r", "").Replace(" ", "");
            var usuarioConsulta = UsuarioHelper.GetUsuario(user);

            if (usuarioConsulta == null)
            {
                usuarioConsulta = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            }


            List<PublicacaoViewModel> listPublicacao = new List<PublicacaoViewModel>();


            var query = (from c in PublicacaoPrivadaHelper.TodasAsPublicacoesAtivasDoUsuario(usuarioConsulta.Id, ordenacao) select c)
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize).ToList();

            foreach (var item in query)
            {
                PublicacaoViewModel publicacao = new PublicacaoViewModel();
                publicacao.Id = item.Id.ToString();
                publicacao.UsuarioPublicacaoID = item.UsuarioPublicacaoID.ToString();

                listPublicacao.Add(publicacao);
            }


            return Json(listPublicacao.ToList(), JsonRequestBehavior.AllowGet);
        }

        public string RetornaTempoDaInteracao(DateTime dataDaIntaracao)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dataDaIntaracao.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            string textTime = "";

            if (delta < 1 * MINUTE)
            {
                textTime = ts.Seconds == 1 ? "há 1 seg" : ts.Seconds + " segundos";
            }
            else if (delta < 2 * MINUTE)
            {
                textTime = "há 1 min";
            }
            else if (delta < 45 * MINUTE)
            {
                textTime = ts.Minutes + " minutos";
            }
            else if (delta < 90 * MINUTE)
            {
                textTime = "há 1 hora";
            }
            else if (delta < 24 * HOUR)
            {
                textTime = ts.Hours + " horas";
            }
            else if (delta < 48 * HOUR)
            {
                textTime = "há 1 dia";
            }
            else if (delta < 30 * DAY)
            {
                textTime = ts.Days + " dias";
            }
            else if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                textTime = months <= 1 ? "há 1 mês" : months + " meses";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                textTime = years <= 1 ? "há 1 ano" : years + " anos";
            }

            return textTime;
        }

        public ActionResult _WriteMessagePartial()
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;

            var usuarioLogado = _UserBsn.GetUserByUsuario(u);

            ViewBag.KinkeeGold = usuarioLogado.ContaGold;

            if (!String.IsNullOrEmpty(usuarioLogado.Name))
            {
                ViewBag.UserName = usuarioLogado.Name + " " + usuarioLogado.Lastname;
            }
            else
            {
                ViewBag.UserName = usuarioLogado.Usuario;
            }

            ViewBag.imagemPerfil = usuarioLogado.imagemPerfil;

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EnviarPublicacao(string txtPublicacao)
        {
            UserModel u = new UserModel();
            u.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = _UserBsn.GetUserByUsuario(u);


            PublicacaoModel publicacao = new PublicacaoModel();
            publicacao.UsuarioPublicacaoID = usuarioLogado.Id;
            publicacao.Publicacao = txtPublicacao;

            publicacao.DateCreate = DateTime.Now;
            publicacao.DateLastInteraction = DateTime.Now;
            publicacao.isActive = true;

            var publicacaoFeita = PublicacaoPrivadaHelper.AddPublicacao(publicacao);

            return Json(new { publicacaoFeita = publicacaoFeita }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddComentario(string publicacaoID, string comentario)
        {
            var usuarioLogado = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            PublicacaoPrivadaHelper.AddComentario(publicacaoID, usuarioLogado.Usuario, usuarioLogado.imagemPerfil, comentario);

            UsuarioComentarioPublicacaoModel retorno = new UsuarioComentarioPublicacaoModel();

            retorno.imagemPerfilUsuarioComentario = usuarioLogado.imagemPerfil;
            retorno.NomeUsuarioComentario = usuarioLogado.Usuario;
            retorno.Comentario = comentario;


            var Publicacao = PublicacaoPrivadaHelper.GetPublicacaoByID(publicacaoID);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public string UploadImagePost(HttpPostedFileBase File)
        {
            if (File != null && File.ContentLength > 0)
            {
                try
                {
                    if (IsImage(File))
                    {
                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);

                        var instructions = new Instructions
                        {
                            Width = 1024,
                            Height = 768,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(File, "~/ImagesPosts/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        try
                        {
                            _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.PostTimeLine);

                            if (varUsuario.GaleriaFotos == null)
                            {
                                List<GaleriaFoto> galeriaFoto = new List<GaleriaFoto>();
                                GaleriaFoto foto = new GaleriaFoto();
                                foto.UrlFoto = newVirtualPath;
                                foto.isPrivate = true;

                                galeriaFoto.Add(foto);

                                varUsuario.GaleriaFotos = galeriaFoto;
                            }
                            else
                            {
                                GaleriaFoto foto = new GaleriaFoto();
                                foto.UrlFoto = newVirtualPath;
                                foto.isPrivate = true;

                                varUsuario.GaleriaFotos.Add(foto);
                            }


                            UserBSN Usuario = new UserBSN();
                            Usuario.EditarUsuario(varUsuario);

                        }
                        catch
                        {
                            return newVirtualPath;
                        }

                        return newVirtualPath;
                    }
                    else
                    {
                        return "";
                    }

                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        public string UploadImageCapaPerfilPrivado(HttpPostedFileBase File)
        {
            if (File != null && File.ContentLength > 0)
            {
                try
                {
                    if (IsImage(File))
                    {
                        UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;
                        modelUsuario.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
                        var varUsuario = _UserBsn.GetUserByUsuario(modelUsuario);

                        var instructions = new Instructions
                        {
                            Width = 1024,
                            Height = 768,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 90,
                        };

                        var i = new ImageJob(File, "~/ImagesPosts/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);

                        try
                        {
                            _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.PostTimeLine);

                            if (varUsuario.GaleriaFotos == null)
                            {
                                List<GaleriaFoto> galeriaFoto = new List<GaleriaFoto>();
                                GaleriaFoto foto = new GaleriaFoto();
                                foto.UrlFoto = newVirtualPath;
                                foto.isPrivate = true;

                                galeriaFoto.Add(foto);

                                varUsuario.GaleriaFotos = galeriaFoto;
                            }
                            else
                            {
                                GaleriaFoto foto = new GaleriaFoto();
                                foto.UrlFoto = newVirtualPath;
                                foto.isPrivate = true;

                                varUsuario.GaleriaFotos.Add(foto);
                            }

                            varUsuario.imagemCapaPrivado = newVirtualPath;
                            UserBSN Usuario = new UserBSN();
                            Usuario.EditarUsuario(varUsuario);

                        }
                        catch
                        {
                            return newVirtualPath;
                        }

                        return newVirtualPath;
                    }
                    else
                    {
                        return "";
                    }

                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        private bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
            {
                return true;
            }

            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" }; // add more if u like...

            // linq from Henrik Stenbæk
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}