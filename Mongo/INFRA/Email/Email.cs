using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using Mongo.Models;
using RestSharp;
using RestSharp.Authenticators;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Mongo.INFRA
{
    public class Email
    {
        //BACKUP
        //public static bool SendGridEmail(string subject, string htmlContent, SendEmailAddress from, SendEmailAddress to)
        //{
        //    //var subject = "Kinkee Co - Email de Teste";
        //    //var htmlContent = "<strong>Oieeeeeee!</strong>";

        //    SendGridMessage sendMail = new SendGridMessage();
        //    sendMail.SetFrom(new EmailAddress(from.Email, from.Nome));

        //    var recipients = new List<EmailAddress>
        //    {
        //        new EmailAddress(to.Email, to.Nome),
        //    };
        //    sendMail.AddTos(recipients);
        //    sendMail.SetSubject(subject);
        //    sendMail.AddContent(MimeType.Html, htmlContent);

        //    Execute(sendMail);

        //    return true;
        //}

        //private static void Execute(SendGridMessage msg)
        //{
        //    var client = new SendGridClient("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");
        //    var response = client.SendEmailAsync(msg);
        //}

        public static bool SendGridEmail(string subject, string htmlContent, SendEmailAddress from, SendEmailAddress to)
        {
            
            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            mailMessage.From = new MailAddress(from.Email, from.Nome);

            mailMessage.Bcc.Add(to.Email);

            mailMessage.Subject = subject;
            mailMessage.Body = htmlContent;
            mailMessage.IsBodyHtml = true;

            smtpClient.Host = "smtp.sendgrid.net";
            smtpClient.Port = 587;//25 ou então (465 for SSL connections)
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("apikey", "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");

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            //Execute(subject, htmlContent, from, to);

            try
            {
                smtpClient.Send(mailMessage);
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public static async Task<bool> SendGridEmailAsync(string subject, string htmlContent, SendEmailAddress from, SendEmailAddress to)
        {

            MailMessage mailMessage = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            mailMessage.From = new MailAddress(from.Email, from.Nome);

            mailMessage.Bcc.Add(to.Email);

            mailMessage.Subject = subject;
            mailMessage.Body = htmlContent;
            mailMessage.IsBodyHtml = true;

            smtpClient.Host = "smtp.sendgrid.net";
            smtpClient.Port = 587;//25 ou então (465 for SSL connections)
            smtpClient.EnableSsl = false;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new System.Net.NetworkCredential("apikey", "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");

            //smtpClient.Send(mailMessage);

            await smtpClient.SendMailAsync(mailMessage).ConfigureAwait(false);

            return true;
        }


        private static bool SendMail(string from, string fromDisplayName, string password, List<string> emailsRecipient, string subject, string body, bool isBodyHtml)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = new MailAddress(from,fromDisplayName);
                
                foreach(string item in emailsRecipient)
                {
                    mailMessage.Bcc.Add(item);
                }

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                //smtpClient.Host = "smtp.Kinkee.me";
                //smtpClient.Port = 587;
                //smtpClient.EnableSsl = false;
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new System.Net.NetworkCredential("notificacao@Kinkee.me", "KinkeeDS123098");

                smtpClient.Host = "smtp.sparkpostmail.com";
                smtpClient.Port = 587;//25 ou então (465 for SSL connections)
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("", "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");

                smtpClient.Send(mailMessage);

                return true;
            }
            catch(Exception e)
            {
                e.ToString();

                return false;
            }
        }

        private static bool SendMail_(string from, string fromDisplayName, string password, List<string> emailsRecipient, string subject, string body, bool isBodyHtml)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = new MailAddress(from, fromDisplayName);

                foreach (string item in emailsRecipient)
                {
                    mailMessage.Bcc.Add(item);
                }

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                smtpClient.Host = "smtp.mailgun.org";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("postmaster@Kinkee.me", "R330p908");

                //smtpClient.Host = "smtp.mandrillapp.com";
                //smtpClient.Port = 587;
                //smtpClient.EnableSsl = false;
                //smtpClient.UseDefaultCredentials = false;
                //smtpClient.Credentials = new System.Net.NetworkCredential("dodoit", "_dUta70FBOcZmMCeIXkuWg");

                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception e)
            {
                e.ToString();

                return false;
            }
        }

        public static bool SendMail(string from, string fromDisplayName,  List<string> emailsRecipient, string subject, string body, bool isBodyHtml)
        {
            try
            {
                MailMessage mailMessage = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();
                mailMessage.From = new MailAddress(from, fromDisplayName);

                foreach (string item in emailsRecipient)
                {
                    mailMessage.Bcc.Add(item);
                }

                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = isBodyHtml;

                smtpClient.Host = "smtp.mandrillapp.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = false;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential("raphael.esanto@gmail.com", "VPCCOeEhJP5JJ3gwF0LG3w");

                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception e)
            {
                e.ToString();

                return false;
            }
        }

        public static void SendMailAtivarConta(List<string> emailUsuario, string usuario)
        {
            throw new NotImplementedException();
        }

        public static bool SendMailAtivarConta(List<string> emailsRecipient, string userName, string urlAction)
        {
            string from = "nao-responda@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = "Comece a Pulsar Agora Mesmo";


            string query = @"<div id=':37t' class='ii gt adP adO'>
                                <div id=':3f1' class='a3s aXjCH m15ad7ff09064b499'>
                                    <u></u>
                                    <div style='background:#f9f9f9;color:#373737;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:17px;line-height:24px;max-width:100%;width:100%!important;margin:0 auto;padding:0'>

                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;line-height:24px;margin:0;padding:0;width:100%;font-size:17px;color:#373737;background:#f9f9f9'>
                                            <tbody>
                                                <tr>
                                                    <td valign='top' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse'>
                                                            <tbody>
                                                                <tr>
                                                                    <td valign='bottom' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse;padding:20px 16px 12px'>
                                                                        <div style='text-align:center'>
                                                                            <a href='https://kinkeesugar.com' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'>
                                                                                <img src='https://kinkeesugar.com/modules/img/novologo_pequeno.png' width='193' height='71' style='outline:none;text-decoration:none;border:none' class='CToWUd'>
                                                                            </a>
                                                                        </div>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign='top' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                        <table cellpadding='32' cellspacing='0' border='0' align='center' style='border-collapse:collapse;background:white;border-radius:0.5rem;margin-bottom:1rem'>
                                                            <tbody>
                                                                <tr>
                                                                    <td width='546' valign='top' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                                        <div style='max-width:600px;margin:0 auto'>

                                                                            <div style='background:white;border-radius:0.5rem;margin-bottom:1rem'>
                                                                                <h2 style='color:#c5243c;line-height:30px;margin-bottom:12px;margin:0 0 12px'>Informações sobre a sua Conta <span class='il'>Kinkee</span>.</h2>

                                                                                <p style='font-size:17px;line-height:24px;margin:0 0 16px'>

                                                                                    Olá {0},
                                                                                </p>

                                                                                <p style='font-size:17px;line-height:24px;margin:0 0 16px'>
                                                                                    Bem vindo(a) à <strong>Kinkee</strong>!. Aqui você encontrará muitas pessoas interessantes.
                                                                                </p>

                                                                                <p style='font-size:17px;line-height:24px;margin:0 0 16px'>
                                                                                    Para editar o seu perfil (incluindo sua foto), acesse sua <a href='https://kinkeesugar.com/MinhaConta/EditarPerfil/' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'>Conta</a>.
                                                                                </p>

                                                                                <p style='font-size:17px;line-height:24px;margin:0 0 16px'>
                                                                                    Se você tiver qualquer duvida, envie-nos um email para <a href='mailto:contato@Kinkee.me' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'>contato@<span class='il'>Kinkee</span>.me</a>. Nós amaremos ajudar você!
                                                                                </p>

                                                                                <p style='font-size:17px;line-height:24px;margin:0 0 16px'>
                                                                                    Felicidades,<br>
                                                                                    <span class='il'>Kinkee</span>
                                                                                </p>
                                                                            </div>

                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' align='center' style='border-collapse:collapse;margin-top:1rem;background:white;color:#989ea6'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse;height:5px;background-image:url('https://kinkeesugar.com/modules/img/barra.png');background-repeat:repeat-x;background-size:auto 5px'></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign='top' align='center' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse;padding:16px 8px 24px'>
                                                                        <div style='max-width:600px;margin:0 auto'>
                                                                            <p style='font-size:12px;line-height:20px;margin:0 0 16px;margin-top:16px'>

                                                                                Feito por <a href='https://kinkeesugar.com' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'><span class='il'>Kinkee</span>, LTDA.</a>
                                                                                &nbsp;•&nbsp;
                                                                                <a href='http://blog.Kinkee.me' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'>
                                                                                    Nosso Blog
                                                                                </a><br><a href='#m_5069734924244884779_' style='color:#989ea6;font-weight:normal;text-decoration:none;word-break:break-word'>

                                                                                    Av. Pedroso de Morais, 457 &nbsp;•&nbsp; São Paulo, SP &nbsp;•&nbsp; CEP 05419-000

                                                                                </a>
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div><div class='yj6qo'></div><div class='adL'>

                                    </div>
                                </div>
                            </div>";


            //string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Uaau! {0}<BR/>Você acabou de receber uma mensagem de "+ MensagemDE + " veja agora: <br/><a href=\"{1}\" title=\"User Email Confirm\">{1}</a></div>", userName, urlAction);
            string body = string.Format(query, userName);


            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }

        public static bool SendMailNewMessageInInbox(List<string> emailsRecipient, string userName, string urlAction, string MensagemDE)
        {
            string from = "nao-responda@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = MensagemDE + " respondeu uma mensagem sua na Kinkee";


            string query = @"<div id=':37t' class='ii gt adP adO'>
                                <div id=':3f1' class='a3s aXjCH m15ad7ff09064b499'>
                                    <u></u>
                                    <div style='background:#f9f9f9;color:#373737;font-family:'Helvetica Neue',Helvetica,Arial,sans-serif;font-size:17px;line-height:24px;max-width:100%;width:100%!important;margin:0 auto;padding:0'>

                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse;line-height:24px;margin:0;padding:0;width:100%;font-size:17px;color:#373737;background:#f9f9f9'>
                                            <tbody>
                                                <tr>
                                                    <td valign='top' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' style='border-collapse:collapse'>
                                                            <tbody>
                                                                <tr>
                                                                    <td valign='bottom' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse;padding:20px 16px 12px'>
                                                                        <div style='text-align:center'>
                                                                            <a href='https://kinkeesugar.com' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'>
                                                                                <img src='https://kinkeesugar.com/modules/img/novologo_pequeno.png' width='193' height='71' style='outline:none;text-decoration:none;border:none' class='CToWUd'>
                                                                            </a>
                                                                        </div>

                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>

                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign='top' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                        <table cellpadding='32' cellspacing='0' border='0' align='center' style='border-collapse:collapse;background:white;border-radius:0.5rem;margin-bottom:1rem'>
                                                            <tbody>
                                                                <tr>
                                                                    <td width='546' valign='top' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                                        <div style='max-width:600px;margin:0 auto'>

                                                                            <div style='background:white;border-radius:0.5rem;margin-bottom:1rem'>
                                                                                <h2 style='color:#c5243c;line-height:30px;margin-bottom:12px;margin:0 0 12px'>Você recebeu uma mensagem na <span class='il'>Kinkee</span>.</h2>

                                                                                <p style='font-size:17px;padding-right:30px;padding-left:30px'>
                                                                                    {0} acabou de enviar uma mensagem para você na <span class='il'>Kinkee</span>. Acesse o seu perfil e responsa a mensagem!
                                                                                </p>
                                                                                <div style='padding-right:30px;padding-left:30px'>
                                                                                    <a href='{1}' class='m_5499545708256744460sm_auto_width m_5499545708256744460sm_block m_5499545708256744460button_link' style='min-width:234px;border:13px solid #53b987;border-radius:4px;background-color:#53b987;font-size:20px;color:#ffffff;display:inline-block;text-align:center;vertical-align:top;font-weight:900;text-decoration:none!important' target='_blank'>
                                                                                        Responder Mensagem
                                                                                    </a>
                                                                                </div>

                                                                                <div style='padding-right:30px;padding-left:30px'>
                                                                                    <div style='padding:30px 0 22px;margin:0;padding-top:20px'></div>
                                                                                </div>

                                                                                <p style='font-size:17px;line-height:24px;margin:0 0 16px'>
                                                                                    Felicidades,<br>
                                                                                    <span class='il'>Kinkee</span>
                                                                                </p>
                                                                            </div>

                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse'>
                                                        <table width='100%' cellpadding='0' cellspacing='0' border='0' align='center' style='border-collapse:collapse;margin-top:1rem;background:white;color:#989ea6'>
                                                            <tbody>
                                                                <tr>
                                                                    <td style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse;height:5px;background-image:url('https://kinkeesugar.com/modules/img/barra.png');background-repeat:repeat-x;background-size:auto 5px'></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign='top' align='center' style='font-family:'Helvetica Neue',Helvetica,Arial,sans-serif!important;border-collapse:collapse;padding:16px 8px 24px'>
                                                                        <div style='max-width:600px;margin:0 auto'>
                                                                            <p style='font-size:12px;line-height:20px;margin:0 0 16px;margin-top:16px'>

                                                                                Feito por <a href='https://kinkeesugar.com' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'><span class='il'>Kinkee</span>, LTDA.</a>
                                                                                &nbsp;•&nbsp;
                                                                                <a href='http://blog.Kinkee.me' style='color:#439fe0;font-weight:bold;text-decoration:none;word-break:break-word' target='_blank'>
                                                                                    Nosso Blog
                                                                                </a><br><a href='#m_5069734924244884779_' style='color:#989ea6;font-weight:normal;text-decoration:none;word-break:break-word'>

                                                                                    Av. Pedroso de Morais, 457 &nbsp;•&nbsp; São Paulo, SP &nbsp;•&nbsp; CEP 05419-000

                                                                                </a>
                                                                            </p>
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </tbody>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div><div class='yj6qo'></div><div class='adL'>

                                    </div>
                                </div>
                            </div>";


            //string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Uaau! {0}<BR/>Você acabou de receber uma mensagem de "+ MensagemDE + " veja agora: <br/><a href=\"{1}\" title=\"User Email Confirm\">{1}</a></div>", userName, urlAction);
            string body = string.Format(query, MensagemDE, urlAction);


            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }


        public static bool SendMailNewMessage(List<string> emailsRecipient, string userName, string urlAction, string MensagemDE)
        {
            string from = "nao-responda@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = MensagemDE + " enviou uma mensagem para você na Kinkee";


            string query = @"<div style='background-color:#f7f7f7'>
                                <div style='display:none;font-size:1px;color:#ffffff;line-height:1px;max-height:0px;max-width:0px;opacity:0;overflow:hidden'></div><div style='background-color:#f7f7f7'>
                                    <div style='Margin:0px auto;max-width:600px'> <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'> <tbody> <tr><td style='direction:ltr;font-size:0px;padding:10px;padding-left:40px;padding-right:40px;text-align:center;vertical-align:top'></td></tr></tbody></table></div><div class='m_-6575732997404725114dropShadow-1 m_-6575732997404725114mainContainer' style='border:1px solid #d9d9d9;background:white;background-color:white;Margin:0px auto;max-width:600px'>
                                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:white;background-color:white;width:100%'>
                                            <tbody>
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:20px 0;padding-bottom:30px;text-align:center;vertical-align:top'>

                                                        <div style='Margin:0px auto;max-width:600px'>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='direction:ltr;font-size:0px;padding:20px 0;padding-left:40px;padding-right:40px;padding-top:0;text-align:center;vertical-align:top'>
                                                                            <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word'>
                                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px'>
                                                                                                    <tbody>
                                                                                                        <tr>
                                                                                                            <td style='width:70px'>
                                                                                                                <img height='auto' src='https://kinkeesugar.com/Content/images/emails/logo-kinkee-1.png' style='border:0;display:block;outline:none;text-decoration:none;height:auto;width:100%' width='80' class='CToWUd'>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </tbody>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>


                                                        <div style='Margin:0px auto;max-width:600px'>


                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='direction:ltr;font-size:0px;padding:40px 40px 0 40px;padding-left:40px;padding-right:40px;text-align:center;vertical-align:top'>
                                                                            <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;padding-bottom:24px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:28px;line-height:150%;text-align:left;color:#000'>
                                                                                                    <strong>
                                                                                                        Você recebeu uma mensagem de {0}
                                                                                                    </strong>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;padding-bottom:24px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:16px;line-height:150%;text-align:left;color:#000'>
                                                                                                    Responda a mensagem acessando o site.
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;padding-bottom:24px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:16px;line-height:150%;text-align:left;color:#000'>
                                                                                                    <p style='padding:1em 0'>
                                                                                                        <span style='background:#000;color:white;font-weight:bold;border-radius:20px;padding:8px 20px'>
                                                                                                            <a href='{1}' style='color: #fff;'>Responder Mensagem</a>
                                                                                                        </span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div style='Margin:0px auto;max-width:600px'>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='direction:ltr;font-size:0px;padding:20px 0;padding-bottom:0;padding-left:40px;padding-right:40px;text-align:center;vertical-align:top'>
                                                                            <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:16px;line-height:150%;text-align:center;color:#000'>
                                                                                                    Abraços,
                                                                                                    <br>Equipe <span class='il'>Kinkee</span>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>


                                    <div style='Margin:0px auto;max-width:600px'>


                                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                            <tbody>
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:20px 0;padding-bottom:0;padding-left:40px;padding-right:40px;padding-top:0;text-align:center;vertical-align:top'>
                                                        <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                <tbody>

                                                                    <tr>
                                                                        <td align='center' style='font-size:0px;padding:10px 40px 20px 40px;word-break:break-word'>
                                                                            <div style='font-family:sans-serif;font-size:13px;line-height:150%;text-align:center;color:#777777'>
                                                                                Em caso de qualquer dúvida,
                                                                                fique à vontade para nos contatar no <a href='http://contato@kinkeesugar.com' style='color:#8c43c5;font-weight:bold' target='_blank'>contato@<span class='il'>kinkee</span>.co</a>.

                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>


                                    <div style='Margin:0px auto;max-width:600px'>
                                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                            <tbody>
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:20px 0;padding-left:40px;padding-right:40px;padding-top:0;text-align:center;vertical-align:top'>
                                                        <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td align='center' style='font-size:0px;word-break:break-word'>
                                                                            <div style='font-family:sans-serif;font-size:13px;line-height:150%;text-align:center;color:#777777'>
                                                                                <span class='il'>Kinkee</span> 2021
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>
                            </div>";


            //string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Uaau! {0}<BR/>Você acabou de receber uma mensagem de "+ MensagemDE + " veja agora: <br/><a href=\"{1}\" title=\"User Email Confirm\">{1}</a></div>", userName, urlAction);
            string body = string.Format(query, MensagemDE, urlAction);


            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }

        public static bool SendMailNewVisit(List<string> emailsRecipient, string userName, string urlAction, string MensagemDE)
        {
            string from = "nao-responda@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = MensagemDE+ " visitou o seu perfil na Kinkee";

            string query = @"<div style='background-color:#f7f7f7'>
                                <div style='display:none;font-size:1px;color:#ffffff;line-height:1px;max-height:0px;max-width:0px;opacity:0;overflow:hidden'></div><div style='background-color:#f7f7f7'>
                                    <div style='Margin:0px auto;max-width:600px'> <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'> <tbody> <tr><td style='direction:ltr;font-size:0px;padding:10px;padding-left:40px;padding-right:40px;text-align:center;vertical-align:top'></td></tr></tbody></table></div><div class='m_-6575732997404725114dropShadow-1 m_-6575732997404725114mainContainer' style='border:1px solid #d9d9d9;background:white;background-color:white;Margin:0px auto;max-width:600px'>
                                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='background:white;background-color:white;width:100%'>
                                            <tbody>
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:20px 0;padding-bottom:30px;text-align:center;vertical-align:top'>

                                                        <div style='Margin:0px auto;max-width:600px'>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='direction:ltr;font-size:0px;padding:20px 0;padding-left:40px;padding-right:40px;padding-top:0;text-align:center;vertical-align:top'>
                                                                            <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:10px 25px;word-break:break-word'>
                                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='border-collapse:collapse;border-spacing:0px'>
                                                                                                    <tbody>
                                                                                                        <tr>
                                                                                                            <td style='width:70px'>
                                                                                                                <img height='auto' src='https://kinkeesugar.com/Content/images/emails/logo-kinkee-1.png' style='border:0;display:block;outline:none;text-decoration:none;height:auto;width:100%' width='80' class='CToWUd'>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </tbody>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>


                                                        <div style='Margin:0px auto;max-width:600px'>


                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='direction:ltr;font-size:0px;padding:40px 40px 0 40px;padding-left:40px;padding-right:40px;text-align:center;vertical-align:top'>
                                                                            <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;padding-bottom:24px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:28px;line-height:150%;text-align:left;color:#000'>
                                                                                                    <strong>
                                                                                                        {0} Acabou de acessar o seu perfil.
                                                                                                    </strong>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;padding-bottom:24px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:16px;line-height:150%;text-align:left;color:#000'>
                                                                                                    Retribua a visita clicando aqui e inicie uma conversa gentil e cordial.
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;padding-bottom:24px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:16px;line-height:150%;text-align:left;color:#000'>
                                                                                                    <p style='padding:1em 0'>
                                                                                                        <span style='background:#000;color:white;font-weight:bold;border-radius:20px;padding:8px 20px'>
                                                                                                            <a href='{1}' style='color: #fff;'>Acessar perfil</a>
                                                                                                        </span>
                                                                                                    </p>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>

                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>

                                                        <div style='Margin:0px auto;max-width:600px'>
                                                            <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td style='direction:ltr;font-size:0px;padding:20px 0;padding-bottom:0;padding-left:40px;padding-right:40px;text-align:center;vertical-align:top'>
                                                                            <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                                                <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                                    <tbody>
                                                                                        <tr>
                                                                                            <td align='center' style='font-size:0px;padding:0px;word-break:break-word'>
                                                                                                <div style='font-family:sans-serif;font-size:16px;line-height:150%;text-align:center;color:#000'>
                                                                                                    Abraços,
                                                                                                    <br>Equipe <span class='il'>Kinkee</span>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </tbody>
                                                                                </table>
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>


                                    <div style='Margin:0px auto;max-width:600px'>


                                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                            <tbody>
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:20px 0;padding-bottom:0;padding-left:40px;padding-right:40px;padding-top:0;text-align:center;vertical-align:top'>
                                                        <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                <tbody>

                                                                    <tr>
                                                                        <td align='center' style='font-size:0px;padding:10px 40px 20px 40px;word-break:break-word'>
                                                                            <div style='font-family:sans-serif;font-size:13px;line-height:150%;text-align:center;color:#777777'>
                                                                                Em caso de qualquer dúvida,
                                                                                fique à vontade para nos contatar no <a href='http://contato@kinkeesugar.com' style='color:#8c43c5;font-weight:bold' target='_blank'>contato@<span class='il'>kinkee</span>.co</a>.

                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>


                                    <div style='Margin:0px auto;max-width:600px'>
                                        <table align='center' border='0' cellpadding='0' cellspacing='0' role='presentation' style='width:100%'>
                                            <tbody>
                                                <tr>
                                                    <td style='direction:ltr;font-size:0px;padding:20px 0;padding-left:40px;padding-right:40px;padding-top:0;text-align:center;vertical-align:top'>
                                                        <div class='m_-6575732997404725114mj-column-per-100 m_-6575732997404725114outlook-group-fix' style='font-size:13px;text-align:left;direction:ltr;display:inline-block;vertical-align:top;width:100%'>
                                                            <table border='0' cellpadding='0' cellspacing='0' role='presentation' style='vertical-align:top' width='100%'>
                                                                <tbody>
                                                                    <tr>
                                                                        <td align='center' style='font-size:0px;word-break:break-word'>
                                                                            <div style='font-family:sans-serif;font-size:13px;line-height:150%;text-align:center;color:#777777'>
                                                                                <span class='il'>Kinkee</span> 2021
                                                                            </div>
                                                                        </td>
                                                                    </tr>
                                                                </tbody>
                                                            </table>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>

                                </div>
                            </div>";


            //string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Começe a Pulsar {0}!<BR/>Você acabou de receber uma visita em seu perfil de " + MensagemDE + " <br/><a href=\"{1}\" title=\"User Email Confirm\">Retribua a visita clicando aqui</a></div>", userName, urlAction);

            string body = string.Format(query, MensagemDE, urlAction);

            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }

        public static bool SendMailVisitaPerfil(List<string> emailsRecipient, int? numeroVisitas)
        {
            string from = "Ola@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = "Você recebeu " + numeroVisitas + " visitas";
            string body = "";

            bool envia = false;

            if (numeroVisitas == 10)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 20)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 30)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 40)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 50)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 100)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 110)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 120)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 130)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 140)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 150)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 160)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 170)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 180)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }
            if (numeroVisitas == 190)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");

                envia = true;
            }

            if (numeroVisitas == 200)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 300)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }
            if (numeroVisitas == 400)
            {
                body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Você recebeu " + numeroVisitas + " visitas no seu perfil.<br/>Mantenha-o atualizado e faça cada vez mais sucesso.<br/><a href=\"https://kinkeesugar.com\" title=\"Acesse seu Perfil.\">Acesse seu Perfil.</a></div>");
                envia = true;
            }


            if (envia)
            {
                if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                    return true;

                else
                    return false;
            }
            else
                return false;


        }

        public static bool SendMailAll(List<string> emailsRecipient)
        {
            string from = "Ola@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = "Diga olá a Kinkee";
            string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Olá,<BR/><BR/>A Kinkee iniciou suas atividades hoje precisamente as 00:01, e já obtivemos um grande sucesso. O pré cadastro teve mais 800 pessoas e a cada hora mais e mais pessoas entram fazem parte da plataforma.<BR/><BR/>Você foi uma das pessoas que realizou o pré cadastro e já pode acessar o seu perfil na Kinkee.<BR/><BR/><a href==\"https://kinkeesugar.com\">Clique aqui e Acesse!<a/><BR/><BR/>Abraços,<BR/><BR/>-Equipe Kinkee.</div>");

            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }


        public static bool SendMailRegistration(List<string> emailsRecipient, string userName)
        {
            string from = "Ola@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = "Você fez nosso coração Pulsar";
            string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Olá {0},<BR/><BR/>A Kinkee iniciará em 3 etapas, a primeira é essa com a pré inscrição, onde coletamos e organizamos todas as pessoas no aconchego do nosso lar; então nós liberaremos para que seja incluídas outras informações como interesses, fotos e descrição, nesse momento você já poderá buscar seu relacionamento Sugar perfeito; na terceira etapa nós iremos liberar diversas funcionalidades importantes, como o nosso botão Kinkee que esperamos ser muito divertido usar.<BR/><BR/>Essa abordagem garante maiores chances de que tudo ocorra bem.Então fique a vontade a casa também é sua.<BR/><BR/>Abraços,<BR/><BR/>-Equipe Kinkee.</div>", userName);

            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))            
                return true;
            
            else
                return false;
        }

        public static bool SendMailSenhaAlterada(List<string> emailsRecipient, string userName)
        {
            string from = "Ola@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = "Senha Alterada!";
            string body = string.Format("<div style='font - family:Helvetica; font - size:16px; line - height:150 %; text - align:left'>Olá {0},<BR/><BR/>Sua senha foi alterada com sucesso.<BR/>Caso não tenha sido você, por favor, <a href='Kinkee.me/Account/ForgotPassword/'>clique aqui</a>.<BR/><BR/>-Equipe Kinkee.</div>", userName);

            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }

        public static bool SendMailForgot(List<string> emailsRecipient, string userName, string urlAction)
        {

            string from = "Ola@Kinkee.me";
            string password = "R330p908";
            string displayName = "Kinkee";
            string subject = "Redefinir Senha";

            string body = string.Format("Olá {0}<BR/>Por favor, clique no link a seguir para redefinir sua senha com segurança: <br/><a href=\"{1}\" title=\"User Email Confirm\">{1}</a><br/><br/> Se não foi você que solicitou, desconsidere esse email.", "", urlAction);

            if (SendMail(from, displayName, password, emailsRecipient, subject, body, true))
                return true;

            else
                return false;
        }

        public static bool SendMailNewslatter(List<string> emailsRecipient, string userName, string urlAction)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();

                // To
                mailMsg.To.Add(new MailAddress("raphael.esanto@gmail.com", "Raphael Santo"));

                // From
                mailMsg.From = new MailAddress("rsanto@gmail.com", "Raphael");

                // Subject and multipart/alternative Body
                mailMsg.Subject = "subject";
                string text = "text body";
                string html = @"<p>html body</p>";
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
                mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

                // Init SmtpClient and send
                SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("raphaelsanto", "r330p908");
                smtpClient.Credentials = credentials;

                smtpClient.Send(mailMsg);

                return true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return false;
            }
        }
    }
}