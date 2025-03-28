using Mongo.BSN;
using Mongo.INFRA;
using Mongo.Infrastruture.Helper;
using Mongo.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace Plataforma.Controllers
{
    public class EnviarConviteController : Controller
    {
        NotificationBSN _notificationsBSN = new NotificationBSN();
        RelationShipBSN _relationShipBSN = new RelationShipBSN();
        UserBSN _userBSN = new UserBSN();

        // GET: 
        public ActionResult Index()
        {
            ViewBag.Quantidade = UsuarioHelper.GetCountBasicToSendInvite();

            return View();
        }

        public ActionResult EnviarConvite()
        {
            //var usuario = UsuarioHelper.GetUserBasicToSendInvite(Email);

            var usuarios = UsuarioHelper.GetUserBasicToSendInvite();

            try
            {
                foreach (var item in usuarios)
                {
                    item.CodigoConvite = GerarCodigoConvite();
                    item.ConviteInvalido = false;
                    UsuarioHelper.AlterarUserBasic(item);

                    string SMSMessage = "Você foi convidada para experimentar a Kinkee. Acesse https://app.kinkeesugar.com/home/cadastro e digite: {0}";
                    SMSMessage = String.Format(SMSMessage, item.CodigoConvite);

                    string numeroTelefone = "";

                    if (item.Mobile != null)
                    {
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
                    }



                    SendEmailAddress to = new SendEmailAddress();
                    to.Email = item.Email;
                    to.Nome = item.FirstName;
                    ProcessaEmails.SendMailSeuConviteChegou(to, item.CodigoConvite);

                    //Email.SendGridEmail();
                }

                ViewBag.Quantidade = UsuarioHelper.GetCountBasicToSendInvite();

                return View("Index");
            }
            catch (Exception ex)
            {
                return View("Index");
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
    }
}