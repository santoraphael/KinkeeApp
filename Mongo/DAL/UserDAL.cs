using Mongo.Conn;
using Mongo.Models;
using Mongo.Models.Compra;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mongo.DAL
{
    public class UserDAL
    {
        private readonly Connection db = new Connection();

        private const string UsersBooking = "Users.Booking";
        private const string UsersInformation = "Users.Information";

        #region Insert / Update

        public bool InsertUser(UserModel user)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            bool retorno;
            try
            {
                collection.InsertOne(user);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool AlterarUser(UserModel user)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            bool retorno;
            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, user.Id);
                collection.ReplaceOne(filter, user, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        #endregion

        #region Gets Simples

        public UserModel GetUserByEmail(string email)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            return user;
        }

        public UserModel GetUserByUsuario(string usuario)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper());
            return user;
        }

        public UserModel GetUserByCodigoConvite(string codigoConvite)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.CodigoConvite.ToUpper() == codigoConvite.ToUpper());
            return user;
        }

        public UserModel GetUsuarioByUserId(ObjectId id)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Id == id);
            return user;
        }

        public List<UserModel> GetListUserByInvited(ObjectId invitedBy)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var list = collection.AsQueryable()
                                 .Where(u => u.InvitedBy == invitedBy)
                                 .OrderByDescending(u => u.DateCreate)
                                 .ToList();
            return list;
        }

        #endregion

        #region Diversos (GetListUserByTipo, GetListTopUserByTipo, etc.)

        public List<UserModel> GetListUserByTipo(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && !string.IsNullOrEmpty(u.imagemPerfil)
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListTopUserByTipo(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.isActive == true
                                          && u.PerfilTop)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListTopUserByTipo(string meuGenero, string meuTipoSugar, string tenhoInteresseEm)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            var list = new List<UserModel>();
            var rnd = new Random();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse == meuGenero || u.TenhoInteresse == "3")
                                     && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper() || u.TenhoInteresse.ToUpper() == "3")
                                     && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2").OrderBy(_ => rnd.Next()).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3").OrderBy(_ => rnd.Next()).ToList();
            }

            return list;
        }

        public List<UserModel> GetListUserBySearch(string tipo, string tipoBusca, string busca)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            var list = new List<UserModel>();

            if (tipoBusca == "2")
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.isActive == true
                                          && u.Endereco.Cidade == busca)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.isActive == true
                                          && u.Endereco.Estado == busca)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            return list;
        }

        public List<UserModel> GetListAllUserByTipo(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            var list = new List<UserModel>();

            try
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.PerfilTop == false
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            catch
            {
                // ignored
            }

            return list;
        }

        public List<UserModel> GetListAllUser()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            var list = new List<UserModel>();

            try
            {
                list = collection.AsQueryable()
                                 .Where(u => u.isActive == true && u.CodigoConvite != "")
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            catch
            {
                // ignored
            }

            return list;
        }

        public List<UserModel> GetListAllUserByTipo(string meuGenero, string meuTipoSugar, string tenhoInteresseEm)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            DateTime menorData = DateTime.Now.AddDays(-90);
            var list = new List<UserModel>();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.DateLastInteraction >= menorData
                                     && u.ComplementoCadastrado
                                     && u.isActive == true
                                     && u.PerfilTop == false)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.DateLastInteraction >= menorData
                                     && u.ComplementoCadastrado
                                     && u.isActive == true
                                     && u.PerfilTop == false)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }

            return list;
        }

        public List<UserModel> GetSearchAllUserByTipo(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            var list = new List<UserModel>();

            try
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            catch
            {
                // ignored
            }

            return list;
        }

        public List<UserModel> GetListNewUserByTipo(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            DateTime menorData = DateTime.Now.AddDays(-7);

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.PerfilTop == false
                                          && u.isActive == true
                                          && u.DateCreate >= menorData)
                                 .OrderByDescending(u => u.DateCreate)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListNewUserByTipo(string meuGenero, string meuTipoSugar, string tenhoInteresseEm)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            DateTime menorData = DateTime.Now.AddDays(-7);
            var list = new List<UserModel>();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.PerfilTop == false
                                     && u.DateCreate >= menorData)
                                 .OrderByDescending(u => u.DateCreate)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.PerfilTop == false
                                     && u.DateCreate >= menorData)
                                 .OrderByDescending(u => u.DateCreate)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }

            return list;
        }

        public List<UserModel> GetListVeficidados(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.PerfilVerificado
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListVeficidados(string meuGenero, string meuTipoSugar, string tenhoInteresseEm)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            var list = new List<UserModel>();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.PerfilVerificado)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.PerfilVerificado)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }

            return list;
        }

        public List<UserModel> GetListGold(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.ContaGold
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListGold(string meuGenero, string meuTipoSugar, string tenhoInteresseEm)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            var list = new List<UserModel>();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.ContaGold)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.ContaGold)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }

            return list;
        }

        public List<UserModel> GetListBlack(string meuGenero, string meuTipoSugar, string tenhoInteresseEm)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            var list = new List<UserModel>();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.ContaSelectBlack)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.isActive == true
                                     && u.ComplementoCadastrado
                                     && u.ContaSelectBlack)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).ToList();
            }

            return list;
        }

        #endregion

        #region Sugeridos / Destaques / Stories

        public List<UserModel> GetListSugeridos(string tipo, DateTime diaAtual)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            string img = " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png";

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.DateLimitSugerido >= diaAtual
                                          && u.imagemPerfil != img
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListSugeridos(string meuGenero, string meuTipoSugar, string tenhoInteresseEm, DateTime diaAtual)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            string img = " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png";
            var rnd = new Random();
            var list = new List<UserModel>();

            if (tenhoInteresseEm == "3")
            {
                list = collection.AsQueryable()
                                 .Where(u =>
                                     (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.PerfilVerificado
                                     && u.imagemPerfil != img
                                     && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }
            else
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                     && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                      || (u.TenhoInteresse.ToUpper() == "3"
                                      || string.IsNullOrEmpty(u.TenhoInteresse)))
                                     && u.PerfilVerificado
                                     && u.imagemPerfil != img
                                     && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).OrderBy(_ => rnd.Next()).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).OrderBy(_ => rnd.Next()).ToList();
            }

            return list;
        }

        public List<UserModel> GetListDestaques(string meuGenero, string meuTipoSugar, string tenhoInteresseEm, DateTime diaAtual)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");
            string img = " https://app.kinkeesugar.com/pulsar/Kinkee/assets/images/modules/sobre/profile.png";
            var rnd = new Random();
            var list = new List<UserModel>();

            try
            {
                if (tenhoInteresseEm == "3")
                {
                    list = collection.AsQueryable()
                                     .Where(u =>
                                         (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                          || (u.TenhoInteresse.ToUpper() == "3"
                                          || string.IsNullOrEmpty(u.TenhoInteresse)))
                                         && u.PerfilTop
                                         && u.imagemPerfil != img
                                         && u.visitasPerfil > 100
                                         && u.GaleriaFotos.Count > 0
                                         && !u.Adm
                                         && u.isActive == true)
                                     .OrderByDescending(u => u.DateLastInteraction)
                                     .ThenByDescending(u => u.visitasPerfil)
                                     .ToList();
                }
                else
                {
                    list = collection.AsQueryable()
                                     .Where(u => u.Genero.ToUpper() == tenhoInteresseEm.ToUpper()
                                         && (u.TenhoInteresse.ToUpper() == meuGenero.ToUpper()
                                          || (u.TenhoInteresse.ToUpper() == "3"
                                          || string.IsNullOrEmpty(u.TenhoInteresse)))
                                         && u.PerfilTop
                                         && u.imagemPerfil != img
                                         && u.visitasPerfil > 100
                                         && u.GaleriaFotos.Count > 0
                                         && !u.Adm
                                         && u.isActive == true)
                                     .OrderByDescending(u => u.DateLastInteraction)
                                     .ThenByDescending(u => u.visitasPerfil)
                                     .ToList();
                }
            }
            catch
            {
                // ignored
            }

            if (meuTipoSugar == "1" || meuTipoSugar == "3")
            {
                list = list.Where(u => u.TipoSugar == "2" || string.IsNullOrEmpty(u.TipoSugar)).OrderBy(_ => rnd.Next()).ToList();
            }
            else
            {
                list = list.Where(u => u.TipoSugar == "1" || u.TipoSugar == "3" || string.IsNullOrEmpty(u.TipoSugar)).OrderBy(_ => rnd.Next()).ToList();
            }

            return list;
        }

        public List<UserModel> GetListDestaquesSugar(string genero, DateTime diaAtual)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            var list = new List<UserModel>();

            try
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero == genero
                                          && !u.Adm
                                          && u.ProfileCreated
                                          && u.ApprovedProfile
                                          && u.imagemPerfil != null
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ThenByDescending(u => u.visitasPerfil)
                                 .ToList();
            }
            catch
            {
                // ignored
            }

            return list;
        }

        public List<UserModel> GetListStoriesSugar(string genero, DateTime diaAtual)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            var list = new List<UserModel>();

            try
            {
                list = collection.AsQueryable()
                                 .Where(u => u.Genero == genero
                                          && u.imagemPerfil != "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-01.png"
                                          && u.imagemPerfil != "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-02.png"
                                          && u.imagemPerfil != "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-03.png"
                                          && u.imagemPerfil != "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-04.png"
                                          && u.imagemPerfil != "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-05.png"
                                          && u.imagemPerfil != "/pulsar/Kinkee/assets/images/modules/sobre/monstrinho-06.png"
                                          && !u.Adm
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateCreate)
                                 .ToList();
            }
            catch
            {
                // ignored
            }

            return list;
        }

        #endregion

        #region Relacionamentos / Bloqueios

        public List<UserModel> GetListVisitadoPor(string tipo, List<string> listaVisitas)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");

            var query = collection.AsQueryable()
                                  .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                           && u.isActive == true);

            var listID = listaVisitas.ConvertAll(id => new ObjectId(id));
            var result = query.Where(q => listID.Contains(q.Id)).ToList();

            return result;
        }

        public List<UserBlockModel> GetListUsersBlocked(string currentUser)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserBlockModel>("UserBlock");

            var list = collection.AsQueryable()
                                 .Where(u => u.UserIdBlock == currentUser
                                          && u.isActive == true)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListAllActiveUsersNotFriendShip(List<ObjectId> objectIds)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var filter = Builders<UserModel>.Filter.Nin(u => u.Id, objectIds)
                       & Builders<UserModel>.Filter.Eq(u => u.isActive, true);

            var list = collection.Find(filter)
                                 .SortByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListAllActiveUsersNotFriendShip(List<ObjectId> objectIds, string genero)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var filter = Builders<UserModel>.Filter.Nin(u => u.Id, objectIds)
                       & Builders<UserModel>.Filter.Eq(u => u.isActive, true)
                       & Builders<UserModel>.Filter.Eq(u => u.Genero, genero);

            var list = collection.Find(filter)
                                 .SortByDescending(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListUserRangeScore(List<string> objectIds, string genero)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            var rnd = new Random();

            var listaConvertida = objectIds.ConvertAll(id => new ObjectId(id));

            var filter = Builders<UserModel>.Filter.In(u => u.Id, listaConvertida)
                       & Builders<UserModel>.Filter.Eq(u => u.isActive, true)
                       & Builders<UserModel>.Filter.Eq(u => u.ProfileCreated, true)
                       & Builders<UserModel>.Filter.Eq(u => u.ApprovedProfile, true)
                       & Builders<UserModel>.Filter.Ne(u => u.imagemPerfil, null)
                       & Builders<UserModel>.Filter.Eq(u => u.Genero, genero);

            var list = collection.Find(filter).ToList();
            return list.OrderBy(_ => rnd.Next()).ToList(); // embaralha em memória
        }

        public List<UserBlockModel> GetListUsersBlockMe(string currentUser)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserBlockModel>("UserBlock");

            var list = collection.AsQueryable()
                                 .Where(u => u.UserBlocked == currentUser
                                          && u.isActive == true)
                                 .ToList();
            return list;
        }

        #endregion

        #region Outras Consultas

        public List<UserModel> GetListUserByTipo()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var list = collection.AsQueryable().ToList();
            return list;
        }

        public List<UserModel> GetListUserByGenero(string genero)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            var rnd = new Random();

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == genero.ToUpper()
                                          && u.ProfileCreated
                                          && u.ApprovedProfile
                                          && u.imagemPerfil != null
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();

            list = list.OrderBy(_ => rnd.Next()).ToList();
            return list;
        }

        public List<UserModel> GetListUserByTipoWithLimit(string tipo)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>("usuarios");

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero.ToUpper() == tipo.ToUpper()
                                          && u.isActive == true)
                                 .OrderByDescending(u => u.DateLastInteraction)
                                 .ToList();
            // var queryLimit = list.Take(list.Count * 5 / 100).ToList(); // (comentado)

            return list;
        }

        #endregion

        #region Login / Informações / Remoção

        public UserModel GetLogarUsuario(string usuario, string passwordHash)
        {
            UserModel result = null;
            try
            {
                var database = db.ConnectServer();
                var collection = database.GetCollection<UserModel>(UsersBooking);
                result = collection.AsQueryable()
                                   .FirstOrDefault(u =>
                                       (u.Usuario.ToUpper() == usuario.ToUpper().Replace(" ", "")
                                        || u.Email.ToUpper() == usuario.ToUpper().Replace(" ", ""))
                                       && u.PasswordHash.ToUpper() == passwordHash.ToUpper());
            }
            catch
            {
                // ignored
            }

            return result;
        }

        public bool InsertUserInformation(UserInformationModel userInformation)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserInformationModel>(UsersInformation);

            bool retorno;
            try
            {
                collection.InsertOne(userInformation);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public bool AlterUserInformation(UserInformationModel userInformation)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserInformationModel>(UsersInformation);

            bool retorno;
            try
            {
                var filter = Builders<UserInformationModel>.Filter.Eq(x => x.UserInformationId, userInformation.UserInformationId);
                collection.ReplaceOne(filter, userInformation, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }
        public List<UserModel> GetListUserByCreatedProfile()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            // Retorna perfis criados e ainda não aprovados
            var list = collection.AsQueryable()
                                 .Where(u => u.ProfileCreated == true
                                          && u.ApprovedProfile == false)
                                 .OrderByDescending(u => u.DateCreate)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListUserPicToApprove()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            // Retorna perfis criados, aprovados e com imagemPerfilPrivado != null
            var list = collection.AsQueryable()
                                 .Where(u => u.ProfileCreated == true
                                          && u.ApprovedProfile == true
                                          && u.imagemPerfilPrivado != null)
                                 .OrderBy(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }

        public List<UserModel> GetListUserPicGaleryToApprove()
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            // Retorna perfis criados, aprovados e com qualquer foto na galeria não aprovada
            var list = collection.AsQueryable()
                                 .Where(u => u.ProfileCreated == true
                                          && u.ApprovedProfile == true
                                          && u.GaleriaFotos.Any(f => f.isApproved == false))
                                 .OrderBy(u => u.DateLastInteraction)
                                 .ToList();
            return list;
        }



        public UserInformationModel GetInformationByUserId(ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserInformationModel>(UsersInformation);

            var info = collection.AsQueryable()
                                 .FirstOrDefault(u => u.UserInformationId == userId);
            return info;
        }

        public List<string> GetInformationByScore(int? minScore, int? maxScore)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserInformationModel>(UsersInformation);

            var list = collection.AsQueryable()
                                 .Where(u => (u.sugar_score == null || u.sugar_score >= minScore)
                                          && u.sugar_score <= maxScore)
                                 .ToList();

            var usersId = new List<string>();
            foreach (var item in list)
            {
                usersId.Add(item.UserInformationId.ToString());
            }

            return usersId;
        }

        public bool RemoveInformationByUserId(ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserInformationModel>(UsersInformation);

            try
            {
                var filter = Builders<UserInformationModel>.Filter.Eq(x => x.UserInformationId, userId);
                collection.DeleteOne(filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveUserBookingByUserId(ObjectId userId)
        {
            var database = db.ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, userId);
                collection.DeleteOne(filter);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        public static implicit operator UserDAL(ConnectionsDAL v)
        {
            throw new NotImplementedException();
        }
    }
}
