using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using Newtonsoft.Json;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Services;

namespace Plataforma.Controllers
{
    public class HomeController : Controller
    {
        UserBSN _User = new UserBSN();
        MemoryCache memoryCache = MemoryCache.Default;

        public ActionResult Index()
        {
            var usuario = User.Identity.IsAuthenticated;

            if (usuario)
            {
                return RedirectToAction("Index", "Dating");
            }

            return RedirectToAction("Login", "Account");
        }

        //[WebMethod]
        public ActionResult LoginModal()
        {
            return View();
        }

        [WebMethod]
        public ActionResult EsqueciSenhaModal()
        {
            SharedViewModel model = new SharedViewModel();

            model.booleanVariable = false;


            return View(model);
        }

        [Route("Cadastro/{codigoConvite}")]
        public ActionResult Cadastro(string CodigoConvite)
        {
            //Email.SendGridEmail();

            ViewBag.CodigoConvite = false;

            if (!String.IsNullOrEmpty(CodigoConvite))
            {
                SharedViewModel sharedViewModel = new SharedViewModel();
                CadastroViewModel cadastroViewModel = new CadastroViewModel();
                cadastroViewModel.CodigoConvite = CodigoConvite;
                sharedViewModel.CadastroViewModel = cadastroViewModel;

                ViewBag.CodigoConvite = true;

                return View(sharedViewModel);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [Route("Cadastro")]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastro(SharedViewModel sharedViewModel, FormCollection form)
        {
            Genero genero = 0;
            if (sharedViewModel.CadastroViewModel != null)
            {
                string generoString = form["Genero"].ToString();

                if (!String.IsNullOrEmpty(generoString))
                {
                    genero = (Genero)Enum.Parse(typeof(Genero), generoString);
                }

                if (genero == 0)
                {
                    ModelState.AddModelError("Genero", "Escolha um Genero");
                }
            }


            if (string.IsNullOrEmpty(sharedViewModel.CadastroViewModel.NomeUsuario))
            {
                ModelState.AddModelError("Usuario", "Nome de Usuário não pode estar vazio");
            }
            //Verifica se usuário é Valido True é valido
            else if (!_User.NomeUsuarioValido(sharedViewModel.CadastroViewModel.NomeUsuario))
            {
                ModelState.AddModelError("Usuario", "Nome de Usuário Já Cadastrado, escolha outro!");
            }
            else if (sharedViewModel.CadastroViewModel.NomeUsuario.Length < 3)
            {
                ModelState.AddModelError("Usuario", "Nome de Usuário não deve ser menor que 6 caracteres");
            }


            if (_User.GetUserByEmail(sharedViewModel.CadastroViewModel.Email) != null)
            {
                ModelState.AddModelError("Email", "E-mail já cadastrado.");
            }


            if (string.IsNullOrEmpty(sharedViewModel.CadastroViewModel.CodigoConvite))
            {
                sharedViewModel.CadastroViewModel.CodigoConvite = "KINKEE";
            }

            var UsuarioBasicData = VerifyInvite(sharedViewModel.CadastroViewModel.CodigoConvite);

            //if (UsuarioBasicData == null)
            //{
            //    ModelState.AddModelError("Codigo de Convite", "O Código de Convite está é invalido ou já foi utilizado. Vefique seu e-mail (e caixa de Spam), e também seu SMS.");
            //}


            if (ModelState.IsValid)
            {
                var ModelUser = new UserModel();
                if (UsuarioBasicData != null)
                {
                    ModelUser = BookingUser(UsuarioBasicData);
                }
                else
                {
                    ModelUser.DateCreate = DateTime.Now;
                    ModelUser.DateLastInteraction = DateTime.Now;
                    ModelUser.isActive = true;
                    ModelUser.DateLastLogin = DateTime.Now;
                    ModelUser.Name = sharedViewModel.CadastroViewModel.NomeUsuario;
                    ModelUser.Lastname = " -- ";
                    ModelUser.SecondEmail = sharedViewModel.CadastroViewModel.Email;
                    ModelUser.LockedOut = false;
                    ModelUser.NumeroTelefone = "11111111";
                }


                ModelUser.Usuario = sharedViewModel.CadastroViewModel.NomeUsuario;
                ModelUser.Genero = genero.ToString();
                ModelUser.Email = sharedViewModel.CadastroViewModel.Email;
                ModelUser.PasswordHash = SecurityHash.EncryptHashMD5(sharedViewModel.CadastroViewModel.Senha);
                ModelUser.AcceptServicesTerms = true;
                ModelUser.CodigoConvite = sharedViewModel.CadastroViewModel.CodigoConvite;

                _User.InsertUser(ModelUser);


                //SendGridAPIHelper.AddContactToList();

                //INVALIDA CONVITE
                var invalidado = InvalidarConviteUsado(ModelUser);

                //ENVIANDO NOVOS CONVITES
                //EnviaNovosConvites();

                if (invalidado)
                {
                    string HashSecurity = ModelUser.Id + "&" + ModelUser.Email + "&" + DateTime.Now;

                    //List<string> emailUsuario = new List<string>();
                    //emailUsuario.Add(ModelUser.Email);

                    //string urlAction = Url.Action("ConfirmEmail", "Account", new { s = SecurityHash.Criptografar(HashSecurity) }, Request.Url.Scheme);

                    //Email.SendMailAtivarConta(emailUsuario, ModelUser.Usuario, urlAction);

                    SendEmailAddress to = new SendEmailAddress();
                    to.Email = ModelUser.Email;
                    to.Nome = ModelUser.Name;
                    ProcessaEmails.SendMailBemVindo(to);

                    //=====PARA LOG=====//
                    //SendEmailAddress paraRaphael = new SendEmailAddress();
                    //paraRaphael.Email = "raphael.esanto@gmail.com";
                    //paraRaphael.Nome = "Raphael Santo";
                    //ProcessaEmails.SendMailAVISORAPHAELNOVOUSUARIO(paraRaphael, ModelUser.Usuario, ModelUser.Email, ModelUser.Genero);
                    //=====PARA LOG=====//

                    AccountController account = new AccountController();
                    SharedViewModel model = new SharedViewModel();
                    LoginViewModel loginViewModel = new LoginViewModel();

                    loginViewModel.Email = ModelUser.Email;
                    loginViewModel.Password = sharedViewModel.CadastroViewModel.Senha;

                    model.LoginViewModel = loginViewModel;


                    AccountController accountController = new AccountController();

                    accountController.Login(model, "/Dating");

                    return RedirectToAction("Index", "Dating");
                }
            }


            return View();
        }

        public void EnviaNovosConvites()
        {
            var usuarios = UsuarioHelper.GetUserBasicToSendInvite();

            try
            {
                foreach (var item in usuarios)
                {
                    item.CodigoConvite = GerarCodigoConvite();
                    item.ConviteInvalido = false;
                    UsuarioHelper.AlterarUserBasic(item);

                    string SMSMessage = "Seu convite chegou, acesse https://app.kinkeesugar.com/home/cadastro e digite: {0}";
                    SMSMessage = String.Format(SMSMessage, item.CodigoConvite);

                    string numeroTelefone = "";

                    if (item.Mobile.Contains("+55"))
                    {
                        numeroTelefone = item.Mobile;

                    }
                    else
                    {
                        var numeroTel = item.Mobile.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                        numeroTelefone = "+55" + numeroTel;
                    }

                    SMSHelper.SendSMS(SMSMessage, numeroTelefone);

                    SendEmailAddress to = new SendEmailAddress();
                    to.Email = item.Email;
                    to.Nome = item.FirstName;
                    ProcessaEmails.SendMailSeuConviteChegou(to, item.CodigoConvite);

                    //Email.SendGridEmail();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public string GerarCodigoConvite()
        {
            Random random = new Random();
            String source = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Int32 length = 6;

            StringBuilder builder = new StringBuilder(length);

            while (length-- > 0)
                builder.Append(source[random.Next(source.Length)]);

            if (UsuarioHelper.ValidarNovoConvite(builder.ToString()))
            {
                return builder.ToString();
            }
            else
            {
                return GerarCodigoConvite();
            }
        }


        public bool InvalidarConviteUsado(UserModel usuarioModel)
        {
            var userBasicData = UsuarioHelper.GetUserBasicByCodigoConvite(usuarioModel.CodigoConvite);

            if (userBasicData != null)
            {
                userBasicData.ConviteInvalido = true;

                UsuarioHelper.AlterarUserBasic(userBasicData);
            }


            return true;
        }

        public ActionResult CadastroModal()
        {
            return View();
        }

        public ActionResult _CadastroPartial()
        {
            return View();
        }

        //[WebMethod]
        public void GetForm(string EmailUsuario, string Usuario, string Genero, string Senha, string Invite)
        {
            UserModel usuario = new UserModel();
            usuario.Email = EmailUsuario;
            usuario.Usuario = Usuario;
            usuario.Genero = Genero;
            usuario.PasswordHash = Senha;
            //usuario.ContaGold = true;
            //usuario.DataVencimentoGold = DateTime.Now.AddHours(1);

            if (!String.IsNullOrEmpty(Invite))
            {
                //usuario.InvitedBy = null;//VerifyInvite(Invite);
                //usuario.DateLimitSugerido = DateTime.Now.AddDays(1);

                //UserModel userInvite = UsuarioHelper.GetUsuarioByObjetcID(usuario.InvitedBy);

                //userInvite.DateLimitSugerido = DateTime.Now.AddDays(1);

                //UsuarioHelper.SavePromotionalCode(userInvite);
            }


            _User.InsertUser(usuario);

            string HashSecurity = usuario.Id + "&" + usuario.Email + "&" + DateTime.Now;

            List<string> emailUsuario = new List<string>();
            emailUsuario.Add(usuario.Email);

            string urlAction = Url.Action("ConfirmEmail", "Account", new { s = SecurityHash.Criptografar(HashSecurity) }, Request.Url.Scheme);

            Email.SendMailAtivarConta(emailUsuario, usuario.Usuario, urlAction);

            AccountController account = new AccountController();
            SharedViewModel model = new SharedViewModel();
            LoginViewModel loginViewModel = new LoginViewModel();

            loginViewModel.Email = usuario.Email;
            loginViewModel.Password = Senha;

            model.LoginViewModel = loginViewModel;


            AccountController accountController = new AccountController();

            //Response.Redirect("https://kinkeesugar.com");
            accountController.Login(model, "/Dating");

            RedirectToAction("Index", "Dating");
        }

        public UserBasicData VerifyInvite(String CodigoConvite)
        {
            var user = UsuarioHelper.GetUserBasicByCodigoConvite(CodigoConvite);

            return user;
        }

        public UserModel BookingUser(UserBasicData userBasicData)
        {
            UserModel userModel = new UserModel()
            {
                DateCreate = userBasicData.DataInclusao,
                DateLastInteraction = userBasicData.DataAlteracao,
                isActive = true,
                DateLastLogin = DateTime.Now,
                Name = userBasicData.FirstName,
                Lastname = userBasicData.LastName,
                SecondEmail = userBasicData.Email,
                LockedOut = false,
                NumeroTelefone = userBasicData.Mobile,
            };

            return userModel;
        }

        public String ValidaUsuario(string Usuario)
        {
            UserModel usuario = new UserModel();
            UserModel usuarioRetorno = null;
            usuario.Usuario = Usuario;

            usuarioRetorno = _User.GetUserByUsuario(usuario);

            string retorno = "1";

            if (usuarioRetorno != null)
            {
                retorno = "0";

            }

            return retorno;
        }

        public String ValidaEmail(string Email)
        {
            UserModel usuarioRetorno = null;

            usuarioRetorno = _User.GetUserByEmail(Email);

            string retorno = "1";

            if (usuarioRetorno != null)
            {
                retorno = "0";

            }

            return retorno;
        }

        //public String GetLogarUsuario(string usuarioOrEmail, string password)
        //{
        //    bool usuarioRetorno = false;

        //    usuarioRetorno = User.GetLogarUsuario(usuarioOrEmail, password);

        //    string retorno = "1";

        //    if (usuarioRetorno == false)
        //    {
        //        retorno = "0";

        //    }

        //    return retorno;
        //}

        public String ValidaEmailRegex(string Email)
        {
            Regex rg = new Regex(@"^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9_]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$");

            string retorno = "0";

            if (rg.IsMatch(Email))
            {
                retorno = "1";
            }

            return retorno;
        }

        public String ValidaUsuarioRegex(string Usuario)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9_]*$");

            string retorno = "0";

            if (rg.IsMatch(Usuario)
                && !string.IsNullOrEmpty(Usuario.Trim()))
            {
                retorno = "1";
            }

            return retorno;
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetConvite()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public IEnumerable<UserModel> SharedSearch(string query = "")
        {
            var users = new List<UserModel>();
            var result = new List<String>();
            if (query.Length > 3)
            {
                var all = _User.GetListAllUserByTipo("1").Where(s => s.Name != null);
                users = all.Where(s => s.Name.Contains(query)).ToList();
                foreach (var user in users)
                {
                    result.Add($"{user.Name} {user.Lastname} - {user.Usuario}");
                }
            }

            return users;
        }
        public ActionResult CommitSearch(string user, string partialName, string tipoPessoa)
        {
            string userResult = "";
            if (String.IsNullOrEmpty(user) && !String.IsNullOrEmpty(partialName))
            {
                var result = GetCachedSearch(tipoPessoa);

                var filtered = result.Where(
                    item => item.Nome.IndexOf(partialName, StringComparison.InvariantCultureIgnoreCase) >= 0).FirstOrDefault();
                if (filtered != null)
                {
                    userResult = filtered.Codigo;
                }
            }
            else if (!String.IsNullOrEmpty(user))
            {
                userResult = user.Split('_').Last();
            }
            return Json(userResult, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetGenero()
        {
            var u = new UserModel
            {
                Usuario = System.Web.HttpContext.Current.User.Identity.Name
            };

            var usuarioLogado = _User.GetUserByUsuario(u);

            // return Json(usuarioLogado.Genero, JsonRequestBehavior.AllowGet);
            return Json(usuarioLogado);
        }

        public ActionResult GetCountUsuariosAtivos()
        {
            return Json(UsuarioHelper.GetCountUsuariosAtivos(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetUsuariosOnline()
        {
            return Json(UsuarioHelper.GetCountUsuariosOnline(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Autocomplete(string term, string tipoPessoa)
        {
            //if (String.IsNullOrEmpty(tipoPessoa))
            //{
            //    tipoPessoa = _User.GetUserByUsuario(new UserModel {
            //                    Usuario = System.Web.HttpContext.Current.User.Identity.Name,
            //                  }).Genero;
            //}
            //var usuarios = GetCachedSearch(tipoPessoa);

            var estados = GetCachedEstados();

            var filteredEstados = estados.Where(
                item => RemoveAccentos(item.Estado).IndexOf(RemoveAccentos(term), StringComparison.InvariantCultureIgnoreCase) >= 0);

            //var filteredItems = usuarios.Where(
            //    item => RemoveAccentos(item.Nome).IndexOf(RemoveAccentos(term), StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();

            var filteredItems = new List<SearchModel>();

            foreach (var estado in filteredEstados)
            {
                filteredItems.Add(new SearchModel
                {
                    Codigo = estado.UF,
                    Nome = $"Estado de {estado.Estado}, Brasil",
                    Tipo = (int)SearchTipo.Estado
                });
            }

            foreach (var estado in estados)
            {
                var cidadesFiltered = estado.Cidades.Where(
                item => RemoveAccentos(item).IndexOf(RemoveAccentos(term), StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();

                foreach (var cidade in cidadesFiltered)
                {
                    filteredItems.Add(new SearchModel
                    {
                        Codigo = cidade,

                        Nome = $"{cidade} / {estado.UF}, Brasil",
                        Tipo = (int)SearchTipo.Cidade
                    });
                }

            }

            return Json(filteredItems.OrderByDescending(i => i.Tipo).ThenBy(i => i.Nome), JsonRequestBehavior.AllowGet);
        }

        public List<EstadoModel> GetCachedEstados()
        {
            var expiration = DateTimeOffset.UtcNow.AddMinutes(2880);

            if (!memoryCache.Contains("estados"))
            {
                var estados = (List<EstadoModel>)JsonConvert.DeserializeObject(
                System.IO.File.ReadAllText(Server.MapPath("~/App_Data/Estados.txt")),
                typeof(List<EstadoModel>));

                foreach (var estado in estados)
                {
                    estado.Cidades = ((List<String>)JsonConvert.DeserializeObject(
                        System.IO.File.ReadAllText(Server.MapPath($"~/App_Data/Cidades/{estado.UF}.txt")),
                        typeof(List<String>)));
                }

                memoryCache.Add("estados", estados, expiration);
            }

            return GetCachedData<List<EstadoModel>>("estados", memoryCache);
        }

        public ActionResult LocationAutocomplete(string term)
        {

            var estados = GetCachedEstados();

            var filteredEstados = estados.Where(
                item => RemoveAccentos(item.Estado).IndexOf(RemoveAccentos(term), StringComparison.InvariantCultureIgnoreCase) >= 0);

            //var filteredItems = usuarios.Where(
            //    item => RemoveAccentos(item.Nome).IndexOf(RemoveAccentos(term), StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();

            var filteredItems = new List<SearchModel>();

            //foreach (var estado in filteredEstados)
            //{
            //    filteredItems.Add(new SearchModel
            //    {
            //        Codigo = estado.UF,
            //        Nome = $"Estado de {estado.Estado}, Brasil",
            //        Tipo = (int)SearchTipo.Estado
            //    });
            //}

            foreach (var estado in estados)
            {
                var cidadesFiltered = estado.Cidades.Where(
                item => RemoveAccentos(item).IndexOf(RemoveAccentos(term), StringComparison.InvariantCultureIgnoreCase) >= 0).ToList();

                foreach (var cidade in cidadesFiltered)
                {
                    filteredItems.Add(new SearchModel
                    {
                        Codigo = cidade,

                        Nome = $"{cidade}, {estado.UF}",
                        Tipo = (int)SearchTipo.Cidade
                    });
                }

            }

            return Json(filteredItems.OrderByDescending(i => i.Tipo).ThenBy(i => i.Nome), JsonRequestBehavior.AllowGet);
        }

        public List<SearchModel> GetCachedSearch(string tipoPessoa)
        {
            var users = new List<UserModel>();
            var search = new List<SearchModel>();
            var expiration = DateTimeOffset.UtcNow.AddMinutes(30);

            if (!memoryCache.Contains("search_1") && tipoPessoa == "1")
            {
                var all = _User.GetSearchAllUserByTipo(tipoPessoa == "1" ? "2" : "1");

                foreach (var user in all)
                {
                    search.Add(new SearchModel()
                    {
                        Codigo = user.Usuario,
                        Nome = ($"{user.Name} {user.Lastname ?? ""} ({user.Usuario})").Trim(),
                        Tipo = (int)SearchTipo.Usuario
                    });
                }

                memoryCache.Add("search_1", search, expiration);
            }
            else if (!memoryCache.Contains("search_2") && tipoPessoa == "2")
            {
                var all = _User.GetSearchAllUserByTipo(tipoPessoa == "1" ? "2" : "1");

                foreach (var user in all)
                {
                    search.Add(new SearchModel()
                    {
                        Codigo = user.Usuario,
                        Nome = ($"{user.Name} {user.Lastname ?? ""} ({user.Usuario})").Trim(),
                        Tipo = (int)SearchTipo.Usuario
                    });
                }

                memoryCache.Add("search_2", search, expiration);
            }

            //memoryCache.Get("search", null)
            string cachename = $"search_{tipoPessoa}";
            return GetCachedData<List<SearchModel>>(cachename, memoryCache);
        }

        public string RemoveAccentos(string text)
        {
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }

        public T GetCachedData<T>(string key, ObjectCache cache)
        {
            try
            {
                if (cache.Contains(key))
                    return (T)cache[key];
                return default(T);
            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (InvalidCastException ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                return default(T);
            }
        }
    }
}