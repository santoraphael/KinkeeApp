using MongoDB.Bson;
using MongoDB.Driver;
using Mongo.DAL;
using Mongo.Models;
using Mongo.INFRA;
using System;
using System.Collections.Generic;
using System.Web.Security;
using System.Linq;
using System.Collections;

namespace Mongo.BSN
{
    public class UserBSN
    {
        UserDAL userDAL = new UserDAL();
        NotificationBSN _notificationBSN = new NotificationBSN();

        public bool InsertUser(UserModel user)
        {
            bool retorno = false;
            try
            {
                //var passwordHash = SecurityHash.EncryptHashMD5(user.PasswordHash);

                user.DateCreate = DateTime.Now;

                if (!String.IsNullOrEmpty(user.Email) && !String.IsNullOrEmpty(user.Usuario) && !String.IsNullOrEmpty(user.PasswordHash))
                {
                    //user.PasswordHash = passwordHash;
                    user.Usuario = user.Usuario.ToUpper();
                    user.Email = user.Email.ToUpper();

                    Random random = new Random();
                    int randomNumber = random.Next(1, 6);

                    switch(randomNumber)
                    {
                        case 1:
                            user.imagemPerfil = "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-01.png";
                            break;

                        case 2:
                            user.imagemPerfil = "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-02.png";
                            break;

                        case 3:
                            user.imagemPerfil = "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-03.png";
                            break;

                        case 4:
                            user.imagemPerfil = "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-04.png";
                            break;

                        case 5:
                            user.imagemPerfil = "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-05.png";
                            break;

                        case 6:
                            user.imagemPerfil = "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-06.png";
                            break;
                    }


                    List<ConnectionModel> Connections = new List<ConnectionModel>();
                    List<UserInboxModel> Inboxes = new List<UserInboxModel>();
                    List<GaleriaFoto> GaleriaFotos = new List<GaleriaFoto>();
                    List<String> Favoritos = new List<String>();

                    user.Connections = Connections;
                    user.Inboxes = Inboxes;
                    user.GaleriaFotos = GaleriaFotos;
                    user.Favoritos = Favoritos;
                    user.isActive = true;


                    userDAL.InsertUser(user);

                    ProcessSwitcher processSwitcher = new ProcessSwitcher();
                    processSwitcher.CreditarKoins(user.Id, "Kinkee", 10, TransactionType.NewContaCredit);

                    //GerarNotificacaoNovoUsuario(user.Id, "Kinkee", user.Id, 10, TransactionType.NewContaCredit);

                }
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool AtivaConta(UserModel user)
        {
            bool retorno = false;
            try
            {
                var passwordHash = SecurityHash.EncryptHashMD5(user.PasswordHash);

                user.DateCreate = DateTime.Now;
                List<string> emailUsuario = new List<string>();
                emailUsuario.Add(user.Email);

                Email.SendMailRegistration(emailUsuario, user.Usuario);
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool AlterarSenhaUser(UserModel user)
        {
            bool retorno = false;
            try
            {
                var passwordHash = SecurityHash.EncryptHashMD5(user.PasswordHash);
                user.PasswordHash = passwordHash;
                user.DateLastInteraction = DateTime.Now;
                user.isActive = true;
                if (userDAL.AlterarUser(user))
                {
                    List<string> emailUsuario = new List<string>();
                    emailUsuario.Add(user.Email);

                    Email.SendMailSenhaAlterada(emailUsuario, user.Usuario);
                }
                else
                {
                    retorno = false;
                }
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool ConfirmarCadastro(UserModel user)
        {
            bool retorno = false;
            try
            {
                var passwordHash = SecurityHash.EncryptHashMD5(user.PasswordHash);

                user.DateLastInteraction = DateTime.Now;
                user.isActive = true;
                if (userDAL.AlterarUser(user))
                {
                    List<string> emailUsuario = new List<string>();
                    emailUsuario.Add(user.Email);

                    Email.SendMailRegistration(emailUsuario, user.Usuario);
                }
                else
                {
                    retorno = false;
                }
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool EditarUsuario(UserModel user)
        {
            bool retorno = false;
            try
            {
                user.DateLastInteraction = DateTime.Now;

                userDAL.AlterarUser(user);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public UserModel GetUserByUsuario(UserModel user)
        {
            UserModel usuario = new UserModel();
            try
            {
                usuario = userDAL.GetUserByUsuario(user.Usuario);
            }
            catch (Exception ex)
            {

            }

            return usuario;
        }

        public UserModel GetUserByUsuario(string user)
        {
            UserModel usuario = new UserModel();
            usuario.Usuario = user;
            try
            {
                usuario = userDAL.GetUserByUsuario(usuario.Usuario);
            }
            catch (Exception ex)
            {

            }

            return usuario;
        }

        public UserModel GetUserByCodigoConvite(string codigoConvite)
        {
            UserModel usuario = new UserModel();

            try
            {
                return userDAL.GetUserByCodigoConvite(codigoConvite);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<UserModel> GetListUserByInvited(ObjectId InvitedBy)
        {
            UserModel usuario = new UserModel();

            try
            {
                return userDAL.GetListUserByInvited(InvitedBy);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserModel GetUsuarioByUserId(ObjectId _userId)
        {
            var user = new UserModel();

            try
            {
                user = userDAL.GetUsuarioByUserId(_userId);
            }
            catch
            {

            }

            return user;
        }


        public bool NomeUsuarioValido(String NomeUsuario)
        {
            try
            {
                var usuario = userDAL.GetUserByUsuario(NomeUsuario);

                if (usuario != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public ICollection<UserInboxModel> GetUserInboxByID(string _usuario)
        {
            UserModel usuario = new UserModel();
            try
            {
                usuario = userDAL.GetUserByUsuario(_usuario);
            }
            catch
            {

            }

            return usuario.Inboxes;
        }

        public UserModel GetUserByEmail(string Email)
        {
            UserModel usuario = new UserModel();
            try
            {
                usuario = userDAL.GetUserByEmail(Email);
            }
            catch
            {

            }

            return usuario;
        }

        public List<UserModel> GetListUserByTipo(string _tipo)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserByTipo(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListTopUserByTipo(string _tipo)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListTopUserByTipo(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListTopUserByTipo(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListTopUserByTipo(_meuGenero, _meuTipoSugar, _tenhoInteresseEm);
            }
            catch (Exception ex)
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListUserBySearch(string _tipo, string _tipoBusca, string _busca)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserBySearch(_tipo, String.IsNullOrEmpty(_tipoBusca) ? null : _tipoBusca, String.IsNullOrEmpty(_busca) ? null : _busca);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListAllUserByTipo(string _tipo)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListAllUserByTipo(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListAllUser()
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                usuarios = userDAL.GetListAllUser();
            }
            catch
            {

            }

            return usuarios;
        }

        public List<UserModel> GetListAllUserByTipo(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListAllUserByTipo(_meuGenero, _meuTipoSugar, _tenhoInteresseEm);
            }
            catch (Exception ex)
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetSearchAllUserByTipo(string _tipo)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetSearchAllUserByTipo(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListNewUserByTipo(string _tipo)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                usuarios = userDAL.GetListNewUserByTipo(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListNewUserByTipo(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListNewUserByTipo(_meuGenero, _meuTipoSugar, _tenhoInteresseEm);
            }
            catch (Exception ex)
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListVeficidados(string _tipo)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                usuarios = userDAL.GetListVeficidados(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListVeficidados(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListVeficidados(_meuGenero, _meuTipoSugar, _tenhoInteresseEm);
            }
            catch (Exception ex)
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListGold(string _tipo)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                usuarios = userDAL.GetListGold(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListGold(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListGold(_meuGenero, _meuTipoSugar, _tenhoInteresseEm);
            }
            catch (Exception ex)
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListBlack(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListBlack(_meuGenero, _meuTipoSugar, _tenhoInteresseEm);
            }
            catch (Exception ex)
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListSugeridos(string _tipo)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                DateTime diaAtual = DateTime.Now;

                usuarios = userDAL.GetListSugeridos(_tipo, diaAtual);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListSugeridos(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                DateTime diaAtual = DateTime.Now;

                usuarios = userDAL.GetListSugeridos(_meuGenero, _meuTipoSugar, _tenhoInteresseEm, diaAtual);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListDestaques(string _meuGenero, string _meuTipoSugar, string _tenhoInteresseEm)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                DateTime diaAtual = DateTime.Now;

                usuarios = userDAL.GetListDestaques(_meuGenero, _meuTipoSugar, _tenhoInteresseEm, diaAtual);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListDestaquesSugar(string Genero)
        {
            List<UserModel> usuarios = new List<UserModel>();

            try
            {
                DateTime diaAtual = DateTime.Now;

                usuarios = userDAL.GetListDestaquesSugar(Genero, diaAtual);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListStoriesSugar(string Genero)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                DateTime diaAtual = DateTime.Now;

                usuarios = userDAL.GetListStoriesSugar(Genero, diaAtual);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> GetListVisitadoPor(string _tipo, List<string> listaVisitas)
        {
            List<UserModel> usuarios = new List<UserModel>();
            try
            {
                usuarios = userDAL.GetListVisitadoPor(_tipo, listaVisitas).OrderByDescending(u => u.DateLastInteraction).ToList();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return RemoveBlockedUsers(usuarios);
        }

        public List<UserModel> RemoveBlockedUsers(List<UserModel> usuarios)
        {
            var usuariosBloqueados = GetListUsersFilterUsers();

            var usuariosFiltrados = new List<UserModel>();

            if (usuariosBloqueados.Count > 0)
            {
                foreach (var item in usuariosBloqueados)
                {
                    usuariosFiltrados = usuarios.Where(u => u.Id.ToString() != item).ToList();
                }
            }
            else
            {
                usuariosFiltrados = usuarios;
            }

            return usuariosFiltrados;
        }

        public List<string> GetListUsersFilterUsers()
        {
            string de = System.Web.HttpContext.Current.User.Identity.Name;
            UserModel u = new UserModel();
            u.Usuario = de;
            var usuarioLogado = GetUserByUsuario(u);

            List<UserBlockModel> BlockedUsers = new List<UserBlockModel>();
            List<UserBlockModel> UsersBlockedMe = new List<UserBlockModel>();
            List<string> rangeFilter = new List<string>();

            try
            {
                BlockedUsers = userDAL.GetListUsersBlocked(usuarioLogado.Id.ToString());
                UsersBlockedMe = userDAL.GetListUsersBlockMe(usuarioLogado.Id.ToString());

                BlockedUsers.AddRange(UsersBlockedMe);

                foreach (var item in BlockedUsers)
                {
                    rangeFilter.Add(item.UserBlocked);
                }

                foreach (var item in UsersBlockedMe)
                {
                    rangeFilter.Add(item.UserIdBlock);
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return rangeFilter;
        }

        public List<UserBlockModel> GetListUsersBlocked()
        {
            string de = System.Web.HttpContext.Current.User.Identity.Name;
            UserModel u = new UserModel();
            u.Usuario = de;
            var usuarioLogado = GetUserByUsuario(u);

            List<UserBlockModel> BlockedUsers = new List<UserBlockModel>();

            try
            {
                BlockedUsers = userDAL.GetListUsersBlocked(usuarioLogado.Id.ToString());
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return BlockedUsers;
        }

        public List<UserModel> GetListAllActiveUsersNotFriendShip(List<ObjectId> objectIds, string genero = null)
        {
            var UsersList = new List<UserModel>();
            string generoBuscado = "";

            try
            {
                if(!String.IsNullOrEmpty(genero))
                {
                    if(genero == "Homem")
                    {
                        generoBuscado = "Mulher";
                    }
                    else
                    {
                        generoBuscado = "Homem";
                    }

                    UsersList = userDAL.GetListAllActiveUsersNotFriendShip(objectIds, generoBuscado);
                }
                else
                {
                    UsersList = userDAL.GetListAllActiveUsersNotFriendShip(objectIds);
                }

                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return UsersList;
        }

        public List<UserModel> GetListUserByTipoWithLimit(string _tipo)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserByTipoWithLimit(_tipo);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListUserByTipo()
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserByTipo();
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListUserByGenero(string Genero)
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserByGenero(Genero);
            }
            catch
            {

            }

            return RemoveBlockedUsers(usuario);
        }

        public List<UserModel> GetListUserRangeScore(List<string> objectIds, string Genero)
        {
            var UsersList = new List<UserModel>();
            try
            {
                UsersList = userDAL.GetListUserRangeScore(objectIds, Genero);
                UsersList = RemoveBlockedUsers(UsersList);                
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            

            return UsersList;
        }

        public UserModel GetLogarUsuario(string _usuario, string _passwordHash)
        {
            UserModel usuario = userDAL.GetLogarUsuario(_usuario, SecurityHash.EncryptHashMD5(_passwordHash));

            if (usuario != null)
            {
                usuario.DateLastLogin = DateTime.Now;
                userDAL.AlterarUser(usuario);
            }

            return usuario;
        }

        public BuscarModel GetSearch(string searchText)
        {
            BuscarModel search = new BuscarModel();

            var usuarioEmail = GetUserByEmail(searchText);
            var usuarioName = userDAL.GetUserByUsuario(searchText);

            if(usuarioEmail != null)
            {
                search.ListaUsuarios.Add(usuarioEmail);
            }

            if (usuarioName != null)
            {
                search.ListaUsuarios.Add(usuarioName);
            }

            return search;
        }

        public bool InsertUserInformation(UserInformationModel userInformation)
        {
            try
            {
                return userDAL.InsertUserInformation(userInformation);
            }
            catch
            {
                return false;
            }
        }

        public bool AlterUserInformation(UserInformationModel userInformation)
        {
            try
            {
                return userDAL.AlterUserInformation(userInformation);
            }
            catch
            {
                return false;
            }
        }

        public bool InsertUserInformationCreateProfile(UserInformationModel userInformation)
        {
            try
            {
                var user = GetUserByUsuario(userInformation.NomeUsuario);
                user.ProfileCreated = true;
                EditarUsuario(user);
                InsertUserInformation(userInformation);
                

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool GerarNotificacaoNovoUsuario(ObjectId From, String WalletNameFrom, ObjectId To, Int64 AmountTransaction, TransactionType TransactionType)
        {
            Dictionary<string, string> valores = new Dictionary<string, string>();
            valores.Add("URL_DESTINO", "/ContaDigital/");
            valores.Add("ALT_IMAGEM", "Crédito Koins");
            valores.Add("URL_IMAGEM", "../../modules/img/credit-koins.png");
            valores.Add("TITULO", "Transação Bancária");
            valores.Add("SUBTITULO", TransactionType.GetDescription());

            if (TransactionType == TransactionType.LoginCredit)
            {
                string texto = "";
                if (WalletNameFrom == "Kinkee")
                {
                    texto = String.Format("Você recebeu <b>{0} Koin(s)</b> em crédito(s) da <b>Kinkee</b> como bonificação.", AmountTransaction);
                }
                else
                {
                    texto = String.Format("Você recebeu <b>{0} Koin(s)</b> em crédito(s) do usuário <b>{1}</b> como bonificação.", AmountTransaction, WalletNameFrom);
                }

                valores.Add("DESCRICAO", texto);
            }


            dynamic dados = valores;
            List<NotificationSendTo> notificationSends = new List<NotificationSendTo>();

            notificationSends.Add(NotificationSendTo.Site);
            return _notificationBSN.GerarNovaNotificacao(dados, Notificationtype.NotificacaoGeral, To, From, notificationSends);
        }

        public List<UserModel> GetListUserByCreatedProfile()
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserByCreatedProfile();
            }
            catch
            {

            }

            return usuario;
        }

        public List<UserModel> GetListUserPicToApprove()
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserPicToApprove();
            }
            catch
            {

            }

            return usuario;
        }

        public List<UserModel> GetListUserPicGaleryToApprove()
        {
            List<UserModel> usuario = new List<UserModel>();
            try
            {
                usuario = userDAL.GetListUserPicGaleryToApprove();
            }
            catch
            {

            }

            return usuario;
        }

        public UserInformationModel GetInformationByUserId(ObjectId _userId)
        {
            var userInformation = new UserInformationModel();

            try
            {
                userInformation = userDAL.GetInformationByUserId(_userId);
            }
            catch
            {

            }

            return userInformation;
        }

        public List<string> GetInformationByScore(int? minScore, int? maxScore)
        {
            var userInformation = new List<string>();

            try
            {
                userInformation = userDAL.GetInformationByScore(minScore, maxScore).ToList();
            }
            catch
            {

            }

            return userInformation;
        }


        public bool RemoveInformationByUserId(ObjectId _userId)
        {
            var userInformation = new UserInformationModel();

            try
            {
                
                return userDAL.RemoveInformationByUserId(_userId);
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveUserBookingByUserId(ObjectId _userId)
        {
            var userInformation = new UserInformationModel();

            try
            {
                return userDAL.RemoveUserBookingByUserId(_userId);
            }
            catch
            {
                return false;
            }
        }
    }
}
