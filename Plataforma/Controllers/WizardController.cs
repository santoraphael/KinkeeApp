using ImageResizer;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using MongoDB.Bson;
using Newtonsoft.Json;
using Plataforma.Helper;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Plataforma.Controllers
{
    public class WizardController : Controller
    {
        NotificationBSN _notificationsBSN = new NotificationBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        UserBSN _userBSN = new UserBSN();
        LocationHelper locationHelper = new LocationHelper();
        LogImageBSN _logImage = new LogImageBSN();


        // GET: Notification
        public ActionResult _IntroPartial()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            ViewBag.MostrarIntro = true;

            if (usuarioLogado.ConfiguracoesIniciais != true && usuarioLogado.ApprovedProfile == true)
            {
                
                ViewBag.GeneroUsuario = usuarioLogado.Genero;
            }
            else
            {
                ViewBag.MostrarIntro = false;
            }


            return View();
        }

        public ActionResult CondiguracoesIniciais(WizardViewModel model)
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            usuarioLogado.CategoriaBDSM = model.CategoriaBDSM;
            usuarioLogado.CategoriaPacksFotosFas = model.CategoriaPacksFotosFas;
            usuarioLogado.CategoriaRelacionamentSugar = model.CategoriaRelacionamentSugar;
            usuarioLogado.ConfiguracoesIniciais = true;

            _userBSN.EditarUsuario(usuarioLogado);

            return RedirectToAction("/Dating");
        }

        [HttpPost]
        public bool WizardCompleto()
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);

            usuarioLogado.ConfiguracoesIniciais = true;

            _userBSN.EditarUsuario(usuarioLogado);

            return true;
        }

        public ActionResult _CreateProfileWizardPartial(string user_id)
        {

            CreateProfileModel createProfileModel = new CreateProfileModel();


            var nomeUsuerioLogado = user_id;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);


            if (usuarioLogado == null)
            {
                return RedirectToAction("Login", "Account");
            }

            createProfileModel.GeneroUsuario = usuarioLogado.Genero;
            createProfileModel.NomeUsuario = usuarioLogado.Usuario;
            createProfileModel.PrimeiroNome = usuarioLogado.Name;
            createProfileModel.ProfileCreated = usuarioLogado.ProfileCreated;
            createProfileModel.ApprovedProfile = usuarioLogado.ApprovedProfile;

            ViewData["Countries"] = new SelectList(locationHelper.ListaPaises(), "Country_str_code", "Country_str_name");


            return View(createProfileModel);
        }

        [HttpPost]
        public ActionResult CreateNewProfile(CreateProfileModel model)
        {

            HttpPostedFileBase imagemCarregada = model.Foto;
            model.Foto = null;

            var serializedoubleClass = JsonConvert.SerializeObject(model);
            var UserInformationModel = JsonConvert.DeserializeObject<UserInformationModel>(serializedoubleClass);

            var nomeusuario = SecurityHash.Criptografar(model.NomeUsuario);
            var logedUser = _userBSN.GetUserByUsuario(model.NomeUsuario);


            CadastroSalvaFotoGaleria(imagemCarregada, logedUser);
            var userInformation = _userBSN.GetInformationByUserId(logedUser.Id);

            if(userInformation != null)
            {
                userInformation = UserInformationModel;
                _userBSN.InsertUserInformationCreateProfile(userInformation);
            }
            else
            {
                UserInformationModel.UserInformationId = logedUser.Id;
                _userBSN.InsertUserInformationCreateProfile(UserInformationModel);
            }


            SendEmailAddress to = new SendEmailAddress();
            to.Email = logedUser.Email;
            to.Nome = logedUser.Usuario;
            bool retorno = ProcessaEmails.SendEmailConfirmacaoCriacaoPerfil(to);

            var user_id = SecurityHash.Criptografar(model.NomeUsuario);


            AccountController account = new AccountController();
            FormsAuthentication.SetAuthCookie(logedUser.Usuario, false);
            AuthCookieGenerate(logedUser.Usuario, logedUser.PasswordHash, logedUser.Id.ToString());

            return RedirectToAction("Index", "Dating", new { user_id });
        }

        public void AuthCookieGenerate(string nomeUsuario, string password, string user_id)
        {
            //FormsAuthentication.Authenticate(nomeUsuario, password);

            HttpCookie AuthCookie;
            AuthCookie = FormsAuthentication.GetAuthCookie(nomeUsuario, true);
            AuthCookie.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Add(AuthCookie);
            //Response.Redirect(FormsAuthentication.GetRedirectUrl(nomeUsuario, true));

            //FormsAuthentication.SetAuthCookie(usuario.Usuario, true);
        }

        public JsonResult CadastroSalvaFotoGaleria(HttpPostedFileBase file, UserModel varUsuario)
        {
            var uri = new Uri(System.Web.HttpContext.Current.Request.Url.AbsoluteUri, UriKind.Absolute);
            var usuarioCriptografado = uri.PathAndQuery.Split('/').Last().Split('?').Last().Replace("user_id=", "");

            var checkBox = Request.Form["isSelectedPrivate"];

            bool isPrivate = false;

            if (checkBox == "on")
            {
                isPrivate = true;
            }


            if (file != null && file.ContentLength > 0)
                try
                {
                    if (IsImage(file))
                    {
                        //UserModel modelUsuario = new UserModel();
                        //UserModel usuarioRetorno = null;

                        //var nomeUsuario = SecurityHash.Descriptografar(usuarioCriptografado);

                        //var varUsuario = UsuarioHelper.GetUsuario(nomeUsuario);


                        var instructions = new Instructions
                        {
                            Width = 1024,
                            Height = 768,
                            Mode = FitMode.Max,
                            Format = "jpg",
                            JpegQuality = 70,
                        };

                        var i = new ImageJob(file, "~/Users/<guid>_<guid>",
                        instructions, false, true);
                        i.CreateParentDirectory = true;
                        i.Build();

                        var newVirtualPath = ImageResizer.Util.PathUtils.GuessVirtualPath(i.FinalPath);
                        varUsuario.imagemPerfil = newVirtualPath;


                        if (varUsuario.GaleriaFotos == null)
                        {
                            List<GaleriaFoto> galeriaFoto = new List<GaleriaFoto>();
                            GaleriaFoto foto = new GaleriaFoto();
                            foto.UrlFoto = newVirtualPath;
                            foto.isPrivate = isPrivate;

                            galeriaFoto.Add(foto);

                            varUsuario.GaleriaFotos = galeriaFoto;


                        }
                        else
                        {
                            GaleriaFoto foto = new GaleriaFoto();
                            foto.UrlFoto = newVirtualPath;
                            foto.isPrivate = isPrivate;

                            varUsuario.GaleriaFotos.Add(foto);
                        }


                        _logImage.InsertLogImage(varUsuario.Id, newVirtualPath, TypeImageSend.Galery);

                        _userBSN.EditarUsuario(varUsuario);
                    }

                }
                catch (Exception ex)
                {
                    var resultado = new
                    {
                        FotoSalva = false,
                        Retorno = "Não foi possível salvar sua foto. Verifique se a imagem não é muito grande e tente novamente.",
                        Erro = ex.Message.ToString()
                    };

                    return Json(resultado);
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }


            var resultadoOk = new
            {
                FotoSalva = true,
                Retorno = "Sua foto foi salva.",
                Erro = ""
            };

            return Json(resultadoOk);
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

        public ActionResult AprovacaoPerfil(CreateProfileModel model)
        {
            var nomeUsuerioLogado = System.Web.HttpContext.Current.User.Identity.Name;
            var usuarioLogado = UsuarioHelper.GetUsuario(nomeUsuerioLogado);


            _userBSN.EditarUsuario(usuarioLogado);

            return RedirectToAction("/Dating");
        }

        public ActionResult ListaEstados(string id)
        {
            return Json(locationHelper.ListaEstados(id), JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListaCidades(string id)
        {
            return Json(locationHelper.ListaCidades(id), JsonRequestBehavior.AllowGet);
        }
         
        public ActionResult ListaProfissoes(string id)
        {
            ProfissaoModel profissaoModel = new ProfissaoModel();
            return Json(profissaoModel.ListaProfissoes().FindAll(p=> p.group.ToString() == id).ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendEmailConviteCadastro(string email)
        {
            SendEmailAddress to = new SendEmailAddress();
            to.Email = email;
            to.Nome = "";
            bool retorno = ProcessaEmails.SendEmailConviteCadastro(to);

            return Json(retorno);
        }
    }
}