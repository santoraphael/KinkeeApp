using Mongo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Messaging;

namespace Mongo.INFRA
{
    public static class ProcessaEmails
    {
        public static bool SendMailSeuConviteChegou(SendEmailAddress to, string codigoConvite)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Seu_Convite_Chegou");

            string body = string.Format(modeloEmail, to.Nome, codigoConvite);

            if (Email.SendGridEmail("Seu convite chegou!", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendMailBemVindo(SendEmailAddress to)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Boas_Vindas_Novo_Usuario");

            string body = string.Format(modeloEmail, to.Nome);

            if (Email.SendGridEmail("Bem vindo(a) à Kinkee", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendMailAVISORAPHAELNOVOUSUARIO(SendEmailAddress to, string nomeUsuario, string emailUsuario, string Genero)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            //string modeloEmail = PegaModeloEmail("Boas_Vindas_Novo_Usuario");

            //string body = string.Format(modeloEmail, to.Nome);

            if (Email.SendGridEmail("Novo usuario "+ nomeUsuario, "Usuario:"+ nomeUsuario+"<br/> Email: "+ emailUsuario + "<br/> Genero: " + Genero, from, to))
                return true;

            else
                return false;
        }
        
        public static bool SendMailSemTempalte(SendEmailAddress to, string subject, string textoEmail)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            //string modeloEmail = PegaModeloEmail("Boas_Vindas_Novo_Usuario");

            //string body = string.Format(modeloEmail, to.Nome);

            if (Email.SendGridEmail(subject, textoEmail, from, to))
                return true;

            else
                return false;
        }
        
        public static bool SendMailComentarioPublicacao(SendEmailAddress to, string NomeUsuario, string linkComentario)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Novo_Comentario_Publicacao");

            string body = string.Format(modeloEmail, to.Nome, linkComentario);

            if (Email.SendGridEmail(NomeUsuario+ " Comentou sua Publicação", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendMailNovaSolicitacaoAmizade(SendEmailAddress to, string NomeUsuario, string LinkImagemPerfil)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Nova_Solicitacao_Amizade");

            string body = string.Format(modeloEmail, to.Nome, NomeUsuario, LinkImagemPerfil);

            if (Email.SendGridEmail(NomeUsuario+ " quer se seu amigo(a)", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendMailRespostaSolicitacaoAmizade(SendEmailAddress to, string NomeUsuario, string LinkImagemPerfil)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Resposta_Solicitacao_Amizade");

            string body = string.Format(modeloEmail, to.Nome, NomeUsuario, LinkImagemPerfil);

            if (Email.SendGridEmail(NomeUsuario + " aceitou sua solicitacao", body, from, to))
                return true;

            else
                return false;
        }

        public static async void SendMailVisitaRecebida(SendEmailAddress to, string NomeUsuario, string LinkImagemPerfil)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Nova_Visita_Recebida").Replace("##NOME##", NomeUsuario).Replace("##USUARIO##", NomeUsuario).Replace("##USUARIO##", LinkImagemPerfil);

            //string body = string.Format(modeloEmail, to.Nome, NomeUsuario, LinkImagemPerfil);

            await Email.SendGridEmailAsync(NomeUsuario + " visitou seu perfil", modeloEmail, from, to);
        }

        public static void SendMailMensagemRecebida(SendEmailAddress to, string NomeUsuario, string LinkImagemPerfil)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Nova_Mensagem_Recebida");

            string body = string.Format(modeloEmail, to.Nome, "https://app.kinkeesugar.com/Dating/Inbox");

            Email.SendGridEmail("Você recebeu uma mensagem de " + NomeUsuario, body, from, to);
        }

        public static bool SendMailVisitaPrivadaRecebida(SendEmailAddress to, string NomeUsuario, string LinkImagemPerfil)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Nova_Visita_Privada_Recebida");

            string body = string.Format(modeloEmail, to.Nome, NomeUsuario, LinkImagemPerfil);

            if (Email.SendGridEmail(NomeUsuario + " tem alguém interessado em seu perfil privado", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendMailForgot(List<string> emailsRecipient, string userName, string urlAction)
        {
            string subject = "Redefinir Senha";

            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            SendEmailAddress to = new SendEmailAddress();
            foreach (var item in emailsRecipient)
            {

                to.Nome = userName;
                to.Email = item;
            }
            


            string body = string.Format("Olá {0}<BR/>Por favor, clique no link a seguir para redefinir sua senha com segurança: <br/><a href=\"{1}\" title=\"User Email Confirm\">{1}</a><br/><br/> Se não foi você que solicitou, desconsidere esse email.", "", urlAction);

            if (Email.SendGridEmail(subject, body, from, to))
                return true;

            else
                return false;
        }


        public static bool SendEmailConviteCadastro(SendEmailAddress to)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Convite_Enviado_Cadastro");

            string body = string.Format(modeloEmail, to.Nome);

            if (Email.SendGridEmail("Alguém que te acha incrível acredita que você deve conhecer a kinkee", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendEmailConfirmacaoCriacaoPerfil(SendEmailAddress to)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string modeloEmail = PegaModeloEmail("Confirmacao_Criacao_Perfil");

            string body = string.Format(modeloEmail, to.Nome);

            if (Email.SendGridEmail("Agora falta pouco", body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendEmailRetornoValidacaoPerfil(SendEmailAddress to, bool isApproved, string coments)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string tituloEmail, comentarioEmail;

            if(isApproved)
            {
                tituloEmail = "Parabéns! Seu perfil foi aprovado!";
                comentarioEmail = coments;
            }
            else
            {
                tituloEmail = "Seu perfil não foi aprovado! Veja o que aconteceu.";
                comentarioEmail = coments;
            }


            string modeloEmail = PegaModeloEmail("Retorno_Validacao_Perfil");

            string body = modeloEmail.Replace("##TITULOEMAIL##", tituloEmail).Replace("##COMENTARIOSEMAIL##", comentarioEmail);    //string.Format(modeloEmail, tituloEmail, comentarioEmail);

            if (Email.SendGridEmail(tituloEmail, body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendEmailAprovarFoto(SendEmailAddress to, int tipoFoto, string pontuacao)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string tituloEmail = "";

            if (tipoFoto == 1)// tipo foto == 1 Foto de perfil
            {
                tituloEmail = to.Nome+" Aprovamos sua nova foto de Perfil";
            }
            else if(tipoFoto == 2) // tipo foto == 2 Foto de galeria
            {
                tituloEmail = to.Nome + " agora você tem uma nova foto na sua galeria";
            }


            string modeloEmail = PegaModeloEmail("Foto_Aprovada");

            string body = modeloEmail.Replace("##PONTUACAO##", pontuacao);    //string.Format(modeloEmail, tituloEmail, comentarioEmail);

            if (Email.SendGridEmail(tituloEmail, body, from, to))
                return true;

            else
                return false;
        }

        public static bool SendEmailRetornoErroValidacaoPerfil(SendEmailAddress to, bool isApproved, string coments)
        {
            SendEmailAddress from = new SendEmailAddress();
            from.Email = "no-replay@kinkeesugar.com";
            from.Nome = "Kinkee";

            string tituloEmail, comentarioEmail;

            tituloEmail = "Ops! Acho que cometemos um erro!";
            comentarioEmail = coments;


            string modeloEmail = PegaModeloEmail("Retorno_Validacao_Perfil");

            string body = modeloEmail.Replace("##TITULOEMAIL##", tituloEmail).Replace("##COMENTARIOSEMAIL##", comentarioEmail);    //string.Format(modeloEmail, tituloEmail, comentarioEmail);

            if (Email.SendGridEmail(tituloEmail, body, from, to))
                return true;

            else
                return false;
        }


        private static string PegaModeloEmail(string nomeModelo)
        {
            using (WebClient client = new WebClient())
            {
                string htmlCode = client.DownloadString(string.Format("https://app.kinkeesugar.com/Templates_emails/{0}.html", nomeModelo));

                return htmlCode;
            }
        }
    }
}
