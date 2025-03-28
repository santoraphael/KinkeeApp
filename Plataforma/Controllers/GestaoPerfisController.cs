using Miscellaneous.ELOCalulate;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Models;
using Mongo.Models.Afiliados;
using Plataforma.Helper;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    [Authorize]
    public class GestaoPerfisController : Controller
    {
        UserBSN Usuario = new UserBSN();

        public ActionResult index()
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            if(varUsuarioContexto.Adm != true)
            {
                return RedirectToAction("Index", "Dating");
            }

            List<UsuariosGestao> lista = new List<UsuariosGestao>();



            var listaUsuarios = Usuario.GetListUserByCreatedProfile();


            foreach (var item in listaUsuarios)
            {


                //Aprovar(item.Usuario, "");

                UsuariosGestao usuariosGestao = new UsuariosGestao();
                usuariosGestao.userModel = item;
                usuariosGestao.userInformation = Usuario.GetInformationByUserId(item.Id);

                lista.Add(usuariosGestao);
            }


            ViewBag.ListaUsuarios = lista;

            return View();
        }


        public ActionResult Aprovar(string NomeUsuario, string comentarios)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

                varUsuarioContexto.ApprovedProfile = true;
                varUsuarioContexto.DateCreate = DateTime.Now;

                Usuario.EditarUsuario(varUsuarioContexto);

                var afiliado = Usuario.GetUsuarioByUserId(varUsuarioContexto.InvitedBy);

                AfiliadosBSN afiliadosBSN = new AfiliadosBSN();
                NomesTiposGanhos nomesTipos = NomesTiposGanhos.ValorPorBabyAprovada;

                if (varUsuarioContexto.Genero == "Homem")
                {
                    nomesTipos = NomesTiposGanhos.ValorPorDaddyAprovado;
                }

                if(afiliado != null)
                {
                    afiliadosBSN.GerarOperacaoOperacao("0000000", null, null, TipoOperacao.Item, nomesTipos, afiliado.Id, varUsuarioContexto.Id);
                }
                

                SendEmailAddress to = new SendEmailAddress();
                to.Email = varUsuarioContexto.Email;
                to.Nome = varUsuarioContexto.Usuario;

                bool retorno = ProcessaEmails.SendEmailRetornoValidacaoPerfil(to, true, "Seu perfil foi aprovado. Acesse nosso o site e conheça pessoas incríveis! Você irá adorar. Acompanhe em nosso instagram https://www.instagram.com/kinkeesugar/ também no instagram do nosso CEO https://www.instagram.com/raphaelsanto/");
            }
            catch(Exception ex)
            {
                ex.Message.ToString();
            }
           

            return RedirectToAction("Index", "GestaoPerfis");
        }

        public ActionResult Reprovar(string NomeUsuario, string comentarios)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

                varUsuarioContexto.ApprovedProfile = false;
                varUsuarioContexto.ProfileCreated = false;

                Usuario.EditarUsuario(varUsuarioContexto);

                //Usuario.GetInformationByUserId(varUsuarioContexto.Id);
                Usuario.RemoveInformationByUserId(varUsuarioContexto.Id);
                Usuario.RemoveUserBookingByUserId(varUsuarioContexto.Id);


                SendEmailAddress to = new SendEmailAddress();
                to.Email = varUsuarioContexto.Email;
                to.Nome = varUsuarioContexto.Usuario;

                bool retorno = ProcessaEmails.SendEmailRetornoValidacaoPerfil(to, false, comentarios);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }


            return RedirectToAction("Index", "GestaoPerfis");
        }

        public ActionResult ErroCadastro(string NomeUsuario, string comentarios)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

                varUsuarioContexto.ApprovedProfile = false;
                varUsuarioContexto.ProfileCreated = false;

                Usuario.EditarUsuario(varUsuarioContexto);

                Usuario.GetInformationByUserId(varUsuarioContexto.Id);
                Usuario.RemoveInformationByUserId(varUsuarioContexto.Id);

                SendEmailAddress to = new SendEmailAddress();
                to.Email = varUsuarioContexto.Email;
                to.Nome = varUsuarioContexto.Usuario;

                comentarios = "Durante o cadastro do seu perfil, o alto numero de acesso ao site pode ter causado uma lentidão e consequentemente uma perca em alguns dados do seu cadastro. Solicitamos que refaça seu perfil para que você não seja prejudicada em uma avaliação injusta.";

                bool retorno = ProcessaEmails.SendEmailRetornoErroValidacaoPerfil(to, false, comentarios);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }


            return RedirectToAction("Index", "GestaoPerfis");
        }



        public ActionResult AprovacaoFotos()
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            if (varUsuarioContexto.Adm != true)
            {
                return RedirectToAction("Index", "Dating");
            }

            List<UsuariosGestao> lista = new List<UsuariosGestao>();

            var listaUsuarios = Usuario.GetListUserPicToApprove();

            foreach (var item in listaUsuarios)
            {
                

                UsuariosGestao usuariosGestao = new UsuariosGestao();
                usuariosGestao.userModel = item;
                usuariosGestao.userInformation = Usuario.GetInformationByUserId(item.Id);

                lista.Add(usuariosGestao);
            }


            ViewBag.ListaUsuarios = lista;

            return View();
        }
        public ActionResult AprovarFotoPerfil(string NomeUsuario, string comentarios)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);
                var usuarioLogadoInformation = Usuario.GetInformationByUserId(varUsuarioContexto.Id);


                varUsuarioContexto.imagemPerfil = varUsuarioContexto.imagemPerfilPrivado;
                varUsuarioContexto.imagemPerfilPrivado = null;

                Usuario.EditarUsuario(varUsuarioContexto);


                SendEmailAddress to = new SendEmailAddress();
                to.Email = varUsuarioContexto.Email;
                to.Nome = varUsuarioContexto.Usuario;

                var pontuacao = EloHelper.SugarScorePreview(varUsuarioContexto.Id, varUsuarioContexto.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Basica, ELOVE.Acao.Positiva);

                bool retorno = ProcessaEmails.SendEmailAprovarFoto(to, 1, pontuacao.ToString());

                EloHelper.AtualizaMMR(varUsuarioContexto.Id, varUsuarioContexto.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Basica, ELOVE.Acao.Positiva);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return RedirectToAction("AprovacaoFotos", "GestaoPerfis");
        }
        public ActionResult RecusarFotoPerfil(string NomeUsuario, string comentarios)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

                varUsuarioContexto.imagemPerfilPrivado = null;

                Usuario.EditarUsuario(varUsuarioContexto);


                //SendEmailAddress to = new SendEmailAddress();
                //to.Email = varUsuarioContexto.Email;
                //to.Nome = varUsuarioContexto.Usuario;

                //bool retorno = ProcessaEmails.SendEmailRetornoValidacaoPerfil(to, true, comentarios);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return RedirectToAction("AprovacaoFotos", "GestaoPerfis");
        }


        public ActionResult AprovacaoFotosGaleria()
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

            if (varUsuarioContexto.Adm != true)
            {
                return RedirectToAction("Index", "Dating");
            }

            List<UsuariosGestao> lista = new List<UsuariosGestao>();

            var listaUsuarios = Usuario.GetListUserPicGaleryToApprove();






            foreach (var item in listaUsuarios)
            {
                //foreach (var foto in item.GaleriaFotos)
                //{
                //    if(foto.isApproved != true)
                //    {
                //        AprovarFotoGaleria(item.Usuario, foto.UrlFoto);
                //    }
                    
                //}

                

                UsuariosGestao usuariosGestao = new UsuariosGestao();
                usuariosGestao.userModel = item;
                usuariosGestao.userInformation = Usuario.GetInformationByUserId(item.Id);

                lista.Add(usuariosGestao);
            }


            ViewBag.ListaUsuarios = lista;

            return View();
        }

        public ActionResult AprovarFotoGaleria(string NomeUsuario, string urlfoto)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);
                var usuarioLogadoInformation = Usuario.GetInformationByUserId(varUsuarioContexto.Id);

                if (varUsuarioContexto.GaleriaFotos != null)
                {
                    foreach (var item in varUsuarioContexto.GaleriaFotos)
                    {
                        if (!String.IsNullOrEmpty(item.UrlFoto))
                        {
                            if (item.UrlFoto.ToUpper() == urlfoto.ToUpper())
                            {
                                item.isApproved = true;
                                break;
                            }
                        }
                    }
                }
                

                Usuario.EditarUsuario(varUsuarioContexto);


                SendEmailAddress to = new SendEmailAddress();
                to.Email = varUsuarioContexto.Email;
                to.Nome = varUsuarioContexto.Usuario;

                var pontuacao = EloHelper.SugarScorePreview(varUsuarioContexto.Id, varUsuarioContexto.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Basica, ELOVE.Acao.Positiva);
                bool retorno = ProcessaEmails.SendEmailAprovarFoto(to, 2, pontuacao.ToString());
                EloHelper.AtualizaMMR(varUsuarioContexto.Id, varUsuarioContexto.ContaGold, usuarioLogadoInformation.sugar_score.GetValueOrDefault(), ELOVE.TipoAcao.Basica, ELOVE.Acao.Positiva);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return RedirectToAction("AprovacaoFotos", "GestaoPerfis");
        }

        public ActionResult RecusarFotoGaleria(string NomeUsuario, string urlfoto)
        {
            UserModel usuarioContexto = new UserModel();
            usuarioContexto.Usuario = NomeUsuario;

            try
            {
                var varUsuarioContexto = Usuario.GetUserByUsuario(usuarioContexto);

                if(varUsuarioContexto.GaleriaFotos != null)
                {
                    foreach (var item in varUsuarioContexto.GaleriaFotos)
                    {
                        if (!String.IsNullOrEmpty(item.UrlFoto))
                        {
                            if (item.UrlFoto.ToUpper() == urlfoto.ToUpper())
                            {
                                varUsuarioContexto.GaleriaFotos.Remove(item);
                                break;
                            }
                        }
                    }
                }

                Usuario.EditarUsuario(varUsuarioContexto);


                //SendEmailAddress to = new SendEmailAddress();
                //to.Email = varUsuarioContexto.Email;
                //to.Nome = varUsuarioContexto.Usuario;

                //bool retorno = ProcessaEmails.SendEmailRetornoValidacaoPerfil(to, true, comentarios);
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

            return RedirectToAction("AprovacaoFotos", "GestaoPerfis");
        }

    }

    public class UsuariosGestao
    {
        public UserModel userModel { get; set; }

        public UserInformationModel userInformation { get; set; }
    }
}