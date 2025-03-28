
using Microsoft.Owin.Security;
using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using Plataforma.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Plataforma.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        UserBSN Usuario = new UserBSN();

        public AccountController()
        {
        }



        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            var Usuario = System.Web.HttpContext.Current.User.Identity.Name;
            

            if(!string.IsNullOrEmpty(Usuario))
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }
        }

        //
        // GET: /Account/Login
        [Authorize]
        public ActionResult CreateProfile(string user_id)
        {
            if(String.IsNullOrEmpty(user_id))
            {
                return View("Login");
            }

            ViewBag.vbUser_id = SecurityHash.Descriptografar(user_id);
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(SharedViewModel model, string returnUrl)
        {
            //VALIDAR MODEL ENVIADO PELO USUÁRIO
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //BUSCAR PELO USUÁRIO NA BASE DE DADOS. 
            var usuario = Usuario.GetLogarUsuario(model.LoginViewModel.Email, model.LoginViewModel.Password);

            Int32 retorno = (int)SignInStatus.Failure;

            if (usuario != null)
            {
                retorno = (int)SignInStatus.Success;

                if(!usuario.ProfileCreated)
                {
                    var user_id = SecurityHash.Criptografar(usuario.Usuario);
                    //AuthCookieGenerate(usuario.Usuario, usuario.PasswordHash, usuario.Id.ToString());
                    return RedirectToAction("CreateProfile", new { user_id });
                }
                else if (usuario.ProfileCreated && !usuario.ApprovedProfile)
                {
                    FormsAuthentication.SetAuthCookie(usuario.Usuario, false);
                    AuthCookieGenerate(usuario.Usuario, usuario.PasswordHash, usuario.Id.ToString());
                    retorno = (int)SignInStatus.Success;
                }
                //else if (!usuario.Adm)
                //{
                //    var user_id = SecurityHash.Criptografar(usuario.Usuario);
                //    //AuthCookieGenerate(usuario.Usuario, usuario.PasswordHash, usuario.Id.ToString());
                //    return RedirectToAction("CreateProfile", new { user_id });
                //}
                else if (usuario.LockedOut == true)
                {
                    retorno = (int)SignInStatus.LockedOut;
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(usuario.Usuario, false);
                    AuthCookieGenerate(usuario.Usuario, usuario.PasswordHash, usuario.Id.ToString());
                    retorno = (int)SignInStatus.Success;
                }
            }
            else
            {
                ModelState.AddModelError("Login", "Seu usuário e/ou senha estão incorretos. Caso esteja tendo problemas para logar, tente trocar a sua senha.");
                retorno = (int)SignInStatus.Failure;
            }


            switch ((SignInStatus)retorno)
            {
                case SignInStatus.Success:

                    if (usuario.isActive == false)
                    {
                        usuario.isActive = true;

                        UsuarioHelper.AtivarDesativarContaUsuario(usuario);
                    }

                    //Plataforma.Notifications not = new Plataforma.Notifications();
                    //List<string> PlayerIds = new List<string>();

                    //PlayerIds.Add("d10c8ab3-8f5f-4c93-aa19-9f65e46abb85");
                    //PlayerIds.Add("c52756b3-5ecb-49fa-a868-fccbbc6fc16b");
                    //PlayerIds.Add("cd810000-2932-4bb2-a592-db556304990e");

                    //not.CreateNotification(PlayerIds);


                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:
                    return View("Lockout");

                case SignInStatus.RequiresVerification:

                    string HashSecurity = usuario.Id + "&" + usuario.Email + "&" + DateTime.Now;

                    List<string> emailUsuario = new List<string>();
                    emailUsuario.Add(usuario.Email);

                    string urlAction = Url.Action("ConfirmEmail", "Account", new { s = SecurityHash.Criptografar(HashSecurity) }, Request.Url.Scheme);

                    Email.SendMailAtivarConta(emailUsuario, usuario.Usuario, urlAction);

                    var pl = SecurityHash.Criptografar(usuario.Email);
                    return RedirectToAction("SendCode", new { pl });


                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Dados inválidos.");
                    return View(model);
            }
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


        public ActionResult DesativarContaUsuario()
        {
            var Usuario = UsuarioHelper.GetUsuario(System.Web.HttpContext.Current.User.Identity.Name);
            Usuario.isActive = false;
            UsuarioHelper.AtivarDesativarContaUsuario(Usuario);


            FormsAuthentication.SignOut();

            return Json(Url.Action("Index", "Home"));
        }


        //
        // GET: /Account/VerifyCode
        //public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        //{
        //    // Require that the user has already logged in via username/password or external login
        //    if (!await SignInManager.HasBeenVerifiedAsync())
        //    {
        //        return View("Error");
        //    }
        //    return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        //}

        //
        // POST: /Account/VerifyCode
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }

        //    // The following code protects for brute force attacks against the two factor codes. 
        //    // If a user enters incorrect codes for a specified amount of time then the user account 
        //    // will be locked out for a specified amount of time. 
        //    // You can configure the account lockout settings in IdentityConfig
        //    var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(model.ReturnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.Failure:
        //        default:
        //            ModelState.AddModelError("", "Invalid code.");
        //            return View(model);
        //    }
        //}

        //
        // GET: /Account/Register'
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user, model.Password);
        //        if (result.Succeeded)
        //        {
        //            await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

        //            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
        //            // Send an email with this link
        //            // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
        //            // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
        //            // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

        //            return RedirectToAction("Index", "Home");
        //        }
        //        AddErrors(result);
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}


        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            SharedViewModel model = new SharedViewModel();

            model.booleanVariable = false;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail()
        {
            string HashSecurity = Request.QueryString["s"].ToString();
            string qr = SecurityHash.Descriptografar(HashSecurity);
            string[] query = qr.Split('&');
            string token = query[0];
            string email = query[1];
            DateTime date = Convert.ToDateTime(query[2]);


            var usuario = Usuario.GetUserByEmail(email);
            if (usuario != null)
            {
                usuario.isActive = true;
                //db.Users.Add(usuario);
                Usuario.ConfirmarCadastro(usuario);

                FormsAuthentication.SetAuthCookie(usuario.Usuario, false);
                return RedirectToAction("Index", "Dating");
            }

            return RedirectToAction("Index", "Dating");

        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(SharedViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = Usuario.GetUserByEmail(model.ForgotPasswordViewModel.Email);

                if (usuario != null)
                {
                    if (usuario.Email.ToUpper() == model.ForgotPasswordViewModel.Email.ToUpper())
                    {
                        string token = usuario.Id.ToString();
                        string email = usuario.Email;

                        string HashSecurity = token + "&" + email;

                        string urlAction = Url.Action("Reset", "Account", new { s = SecurityHash.Criptografar(HashSecurity) }, Request.Url.Scheme);

                        List<string> emailUsuario = new List<string>();
                        emailUsuario.Add(usuario.Email);
                        ProcessaEmails.SendMailForgot(emailUsuario, usuario.Usuario, urlAction);
                        model.booleanVariable = true;
                    }
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Reset(UserModel model)
        {
            @ViewBag.mostrarAviso = false;

            if (Request.QueryString["s"] != null)
            {
                string HashSecurity = Request.QueryString["s"].ToString();
                string qr = SecurityHash.Descriptografar(HashSecurity);
                string[] query = qr.Split('&');
                string token = query[0];
                string email = query[1];
                //DateTime date = Convert.ToDateTime(query[2]);


                var usuario = Usuario.GetUserByEmail(email);
                Session["usuario"] = usuario;

                if (usuario == null)
                {
                    return RedirectToAction("Index", "Dating");
                }
                //string HashSecurity = "Token=" + token + "&Email=" + email;

                return View();
            }
            else
            {
                return RedirectToAction("Index", "Dating");
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Reset(ResetPasswordViewModel model, string Email, string Token)
        {

            @ViewBag.mostrarAviso = false;
            if (ModelState.IsValid)
            {
                var usuario = (UserModel)Session["usuario"];
                if (usuario != null)
                {
                    if (model.Password == model.ConfirmPassword)
                    {
                        usuario.PasswordHash = model.Password;
                        Usuario.AlterarSenhaUser(usuario);
                    }
                }

                Session["usuario"] = null;
                return RedirectToAction("Index", "Dating");
            }
            else
            {
                @ViewBag.mostrarAviso = true;
                @ViewBag.Aviso = "Verifique se você digita a mesma senha nos dois campos";
            }

            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var user = await UserManager.FindByNameAsync(model.Email);
        //    if (user == null)
        //    {
        //        // Don't reveal that the user does not exist
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return RedirectToAction("ResetPasswordConfirmation", "Account");
        //    }
        //    AddErrors(result);
        //    return View();
        //}

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public ActionResult SendCode(string pl)
        {
            var userId = User.Identity;
            if (userId == null)
            {
                return View("Error");
            }

            try
            {
                var Email = SecurityHash.Descriptografar(pl);
                @ViewBag.Email = "****" + Email.Substring(4);
            }
            catch
            {

            }



            return View();
        }

        //
        // POST: /Account/SendCode
        //[HttpPost]
        //[AllowAnonymous]
        //public async Task<ActionResult> SendCode(SendCodeViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View();
        //    }

        //    // Generate the token and send it
        //    if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
        //    {
        //        return View("Error");
        //    }
        //    return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        //}

        //
        // GET: /Account/ExternalLoginCallback
        //[AllowAnonymous]
        //public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        //{
        //    var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
        //    if (loginInfo == null)
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    // Sign in the user with this external login provider if the user already has a login
        //    var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        //    switch (result)
        //    {
        //        case SignInStatus.Success:
        //            return RedirectToLocal(returnUrl);
        //        case SignInStatus.LockedOut:
        //            return View("Lockout");
        //        case SignInStatus.RequiresVerification:
        //            return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
        //        case SignInStatus.Failure:
        //        default:
        //            // If the user does not have an account, then prompt the user to create an account
        //            ViewBag.ReturnUrl = returnUrl;
        //            ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
        //            return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
        //    }
        //}

        //
        // POST: /Account/ExternalLoginConfirmation
        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Index", "Manage");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        // Get the information about the user from the external login provider
        //        var info = await AuthenticationManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //        {
        //            return View("ExternalLoginFailure");
        //        }
        //        var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
        //        var result = await UserManager.CreateAsync(user);
        //        if (result.Succeeded)
        //        {
        //            result = await UserManager.AddLoginAsync(user.Id, info.Login);
        //            if (result.Succeeded)
        //            {
        //                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
        //                return RedirectToLocal(returnUrl);
        //            }
        //        }
        //        AddErrors(result);
        //    }

        //    ViewBag.ReturnUrl = returnUrl;
        //    return View(model);
        //}

        //
        // POST: /Account/LogOff
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        if (_userManager != null)
        //        {
        //            _userManager.Dispose();
        //            _userManager = null;
        //        }

        //        if (_signInManager != null)
        //        {
        //            _signInManager.Dispose();
        //            _signInManager = null;
        //        }
        //    }

        //    base.Dispose(disposing);
        //}

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        private ActionResult RedirectToLocal(string returnUrl)
        {
            try
            {
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
            }
            catch
            {

            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        public enum SignInStatus
        {
            /// <summary>
            /// Sign in was successful
            /// </summary>
            Success,

            /// <summary>
            /// User is locked out
            /// </summary>
            LockedOut,

            /// <summary>
            /// Sign in requires addition verification (i.e. two factor)
            /// </summary>
            RequiresVerification,

            /// <summary>
            /// Sign in failed
            /// </summary>
            Failure
        }

    }
}