using System;
using System.Collections.Generic;
using System.Linq;
using Mongo.Conn;
using Mongo.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo.Infrastruture.Helper
{
    public static class UsuarioHelper
    {
        private const string UsersBanners = "Users.Banners";
        private const string UsersBooking = "Users.Booking";
        private const string UsersBasicData = "Users.BasicData";
        private const string ConfigBasicData = "Config.BasicData";

        #region Gets Simples

        public static UserModel GetUsuario(string usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var result = collection.AsQueryable()
                                   .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper()
                                                     && u.isActive == true);
            return result;
        }

        public static UserModel GetUsuarioAtivoENaoAtivo(string usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var result = collection.AsQueryable()
                                   .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper());
            return result;
        }

        public static UserModel GetUsuarioByString(string usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var result = collection.AsQueryable()
                                   .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper());
            return result;
        }

        public static string GetFotoPerfil(string usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper());
            return user?.imagemPerfil;
        }

        public static DateTime? PerfilSugeridoAtivo(string usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper());
            return user?.DateLimitSugerido;
        }

        public static string GetUsuarioID(string usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Usuario.ToUpper() == usuario.ToUpper());
            return user?.Id.ToString();
        }

        public static UserModel GetUsuarioByID(string id)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Id.ToString().ToUpper() == id.ToUpper());
            return user;
        }

        public static UserModel GetUsuarioByObjetcID(ObjectId id)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Id == id);
            return user;
        }

        public static UserModel GetUsuarioAtivoByObjetcID(ObjectId id)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Id == id && u.isActive == true);
            return user;
        }

        #endregion

        #region Banner

        public static BannerModel GetBannerByUsuarioID(string userID, string genero)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<BannerModel>(UsersBanners);

            // Pega banners ativos cujo genero seja '0' (qualquer) ou igual ao gênero do user
            var query = collection.AsQueryable()
                                  .Where(b => b.isActive == true && (b.Genero == "0" || b.Genero == genero))
                                  .ToList();

            // Pega o primeiro banner que o usuário ainda não viu
            var banner = query.Where(i => !i.vistoPor.Contains(userID)).FirstOrDefault();
            return banner;
        }

        public static BannerModel GetBannerByID(string id)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<BannerModel>(UsersBanners);

            var banner = collection.AsQueryable()
                                   .FirstOrDefault(b => b.Id == ObjectId.Parse(id));
            return banner;
        }

        public static bool AddNewViewerBanner(BannerModel banner)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<BannerModel>(UsersBanners);
            bool retorno;

            try
            {
                // Em vez de Save<BannerModel>(...), vamos usar ReplaceOne (upsert) se a key do banner for o Id
                var filter = Builders<BannerModel>.Filter.Eq(x => x.Id, banner.Id);
                collection.ReplaceOne(filter, banner, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool AddBanner(BannerModel banner)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<BannerModel>(UsersBanners);

            bool retorno;
            try
            {
                // Substitui Insert<BannerModel>(banner)
                collection.InsertOne(banner);
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        #endregion

        #region User Favorite / Visits

        public static void RemoveUserFavorite(string idUsuarioFavorito, UserModel usuario)
        {
            usuario.Favoritos.Remove(idUsuarioFavorito);
            AddAndRemoveUserFavorite(usuario);
        }

        public static void AddUserFavorite(string idUsuarioFavorito, UserModel usuario)
        {
            usuario.Favoritos.Add(idUsuarioFavorito);
            AddAndRemoveUserFavorite(usuario);
        }

        public static void AddVisita(UserModel usuario, int? num)
        {
            if (usuario.visitasPerfil == null)
            {
                usuario.visitasPerfil = 0;
            }
            usuario.visitasPerfil += num;
            AddVisita(usuario);
        }

        public static void AddPerfilVisita(UserModel usuario, UserModel usuarioVisita)
        {
            if (usuario.visitadoPor == null)
            {
                usuario.visitadoPor = new List<string>();
                usuario.visitadoPor.Add(usuarioVisita.Id.ToString());
            }
            else
            {
                if (!usuario.visitadoPor.Contains(usuarioVisita.Id.ToString()))
                {
                    usuario.visitadoPor.Add(usuarioVisita.Id.ToString());
                }
            }

            AddVisita(usuario);
        }

        #endregion

        #region Block / Report

        public static void AddUserBlocked(string currentUser, string blockedUser)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBlockModel>("UserBlock");

            var userBlock = collection.AsQueryable()
                                      .FirstOrDefault(u => u.UserIdBlock == currentUser
                                                        && u.UserBlocked == blockedUser);

            if (userBlock == null)
            {
                userBlock = new UserBlockModel
                {
                    DateCreate = DateTime.Now,
                    DateLastInteraction = DateTime.Now,
                    isActive = true,
                    UserIdBlock = currentUser,
                    UserBlocked = blockedUser
                };
            }
            else
            {
                userBlock.isActive = true;
                userBlock.DateLastInteraction = DateTime.Now;
                userBlock.UserIdBlock = currentUser;
                userBlock.UserBlocked = blockedUser;
            }

            try
            {
                // Em vez de Save<UserBlockModel>(...), ReplaceOne
                var filter = Builders<UserBlockModel>.Filter.Eq(x => x.Id, userBlock.Id);
                collection.ReplaceOne(filter, userBlock, new ReplaceOptions { IsUpsert = true });
            }
            catch
            {
                // ignored
            }
        }

        public static void RemoveBlocked(string currentUser, string blockedUser)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBlockModel>("UserBlock");

            var userBlock = collection.AsQueryable()
                                      .FirstOrDefault(u => u.UserIdBlock == currentUser
                                                        && u.UserBlocked == blockedUser);
            if (userBlock == null) return;

            userBlock.isActive = false;
            userBlock.DateLastInteraction = DateTime.Now;

            try
            {
                var filter = Builders<UserBlockModel>.Filter.Eq(x => x.Id, userBlock.Id);
                collection.ReplaceOne(filter, userBlock, new ReplaceOptions { IsUpsert = true });
            }
            catch
            {
                // ignored
            }
        }

        public static void AddUserReport(string currentUser, string reportedUser, string reportedType, string reportDetails)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserReportModel>("UserReport");

            var userReport = new UserReportModel
            {
                DateCreate = DateTime.Now,
                DateLastInteraction = DateTime.Now,
                UserIdReport = currentUser,
                UserIdReported = reportedUser,
                ReportedType = reportedType,
                ReportDetails = reportDetails
            };

            try
            {
                // Substitui Save<UserReportModel>(userReport) => ReplaceOne ou InsertOne
                // Supondo que userReport.Id controla a PK:
                collection.InsertOne(userReport);
            }
            catch
            {
                // ignored
            }
        }

        #endregion

        #region Salvar / Alterar / Remover Lógicas

        public static bool AddLastInteraction(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool AddVisita(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool AddAndRemoveUserFavorite(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool EditarUsuario(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool SavePromotionalCode(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool AtivarDesativarContaUsuario(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool SalvarNumeroTelefone(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool SalvarUsuarioDivulgacao(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool SalvarUserID(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool DesativarContaGold(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static bool SaveUser(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        #endregion

        #region Dados Bancários

        public static bool SaveDadosBancarios(DadosBancariosModel dadosBancario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<DadosBancariosModel>(ConfigBasicData);
            bool retorno;

            try
            {
                var filter = Builders<DadosBancariosModel>.Filter.Eq(x => x.Id, dadosBancario.Id);
                collection.ReplaceOne(filter, dadosBancario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static DadosBancariosModel GetDadosBancarios(ObjectId usuarioId)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<DadosBancariosModel>(ConfigBasicData);

            var result = collection.AsQueryable()
                                   .FirstOrDefault(u => u.UserId == usuarioId);
            return result;
        }

        #endregion

        #region UserBasicData

        public static UserBasicData GetUserBasicByCodigoConvite(string codigoConvite)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBasicData>(UsersBasicData);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.CodigoConvite == codigoConvite.Replace(" ", "").ToUpper()
                                                   && (u.ConviteInvalido == null || u.ConviteInvalido == false));
            return user;
        }

        public static bool AlterarUserBasic(UserBasicData user)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBasicData>(UsersBasicData);
            bool retorno;

            try
            {
                var filter = Builders<UserBasicData>.Filter.Eq(x => x.Id, user.Id);
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

        #region Contagem

        public static int GetCountUsuariosOnline()
        {
            // Lógica original: "var num = random.Next(80, 100);" e "count = (Int32)collection.AsQueryable<UserModel>().Where(u => u.PerfilVerificado).Count()*11;"
            // Mantivemos a mesma
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            int count;

            try
            {
                var rnd = new Random();
                var num = rnd.Next(80, 100);

                // Driver 2.x => .AsQueryable().Where(...).Count()
                count = collection.AsQueryable()
                                  .Where(u => u.PerfilVerificado)
                                  .Count() * 11;
            }
            catch
            {
                count = 0;
            }

            return count;
        }

        public static int GetCountUsuariosAtivos()
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            int count;

            try
            {
                // Substitui FindAll().Count() => CountDocuments ou .AsQueryable().Count()
                // Se queremos todos:
                count = collection.AsQueryable().Count();
            }
            catch
            {
                count = 0;
            }

            return count;
        }

        #endregion

        #region Convidar / Celular

        public static bool SaveUserSendInvite(UserModel usuario)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);
            bool retorno;

            try
            {
                var filter = Builders<UserModel>.Filter.Eq(x => x.Id, usuario.Id);
                collection.ReplaceOne(filter, usuario, new ReplaceOptions { IsUpsert = true });
                retorno = true;
            }
            catch
            {
                retorno = false;
            }

            return retorno;
        }

        public static List<UserBasicData> GetUserBasicToSendInvite()
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBasicData>(UsersBasicData);

            try
            {
                var list = collection.AsQueryable()
                                     .Where(u => u.ConviteInvalido == null)
                                     .Take(100)
                                     .ToList();

                if (list == null || list.Count == 0)
                {
                    list = collection.AsQueryable()
                                     .Where(u => (u.Mobile == null || u.Mobile == "")
                                               && u.ConviteInvalido == null)
                                     .Take(100)
                                     .ToList();
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public static int GetCountBasicToSendInvite()
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBasicData>(UsersBasicData);

            try
            {
                var quantidade = collection.AsQueryable()
                                           .Count(u => u.ConviteInvalido == false);
                return quantidade;
            }
            catch
            {
                return 0;
            }
        }

        public static UserBasicData GetUserBasicToSendInvite(string email)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBasicData>(UsersBasicData);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper()
                                                   && (u.ConviteInvalido == null || u.ConviteInvalido == false));
            return user;
        }

        public static List<UserModel> GetListUserActive(ObjectId id)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserModel>(UsersBooking);

            var list = collection.AsQueryable()
                                 .Where(u => u.Genero == "Homem"
                                          && u.isActive == true
                                          && u.Id != id)
                                 .ToList();
            return list;
        }

        public static bool ValidarNovoConvite(string codigoConvite)
        {
            var database = new Connection().ConnectServer();
            var collection = database.GetCollection<UserBasicData>(UsersBasicData);

            var user = collection.AsQueryable()
                                 .FirstOrDefault(u => u.CodigoConvite == codigoConvite);

            return (user == null);
        }

        #endregion

        #region Utilidades

        public static string GeneratePromotionalCode()
        {
            var random = new Random();
            var source = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdfghijkmnoqrtvwxyz0123456789";
            var length = 15;

            var builder = new System.Text.StringBuilder(length);
            while (length-- > 0)
                builder.Append(source[random.Next(source.Length)]);

            return builder.ToString();
        }

        #endregion
    }
}
