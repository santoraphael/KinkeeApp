using System;
using System.Linq;
using System.Web;
using Miscellaneous.ELOCalulate;
using Mongo.BSN;
using MongoDB.Bson;
using static Miscellaneous.ELOCalulate.ELOVE;

namespace Plataforma.Helper
{
    public static class EloHelper
    {
        /// <summary>Calculoa o MMR (Sugar Score) do usuário baseado no tipo de ação, peso da ação se é positiva ou negativa.</summary>
        /// <param name="id_usuario_calculado">ObjectId do usuário a ser calculado.</param>
        /// <param name="usuario_premium">Se o usuário tem conta Premium.</param>
        /// <param name="scoreBase">Qual é o ScoreBase.</param>
        /// <param name="TipoAcao">Ação basicam, média, essencial e importante.</param>
        /// <param name="acao">Se é uma ação Positiva ou Negativa.</param>
        public static bool AtualizaMMR(ObjectId id_usuario_calculado, bool usuario_premium, int scoreBase, TipoAcao TipoAcao, Acao acao)
        {
            try
            {
                UserBSN Usuario = new UserBSN();

                var ScoreUsuarioCalculado = Usuario.GetInformationByUserId(id_usuario_calculado);
                if (ScoreUsuarioCalculado == null)
                {
                    ScoreUsuarioCalculado = new Mongo.Models.UserInformationModel();
                    ScoreUsuarioCalculado.UserInformationId = id_usuario_calculado;
                }

                var novoMMR = CalculaMMR(ScoreUsuarioCalculado.sugar_score.GetValueOrDefault(), usuario_premium, scoreBase, TipoAcao, acao);
                ScoreUsuarioCalculado.sugar_score = novoMMR;

                return Usuario.AlterUserInformation(ScoreUsuarioCalculado);
            }
            catch
            {
                return false;
            }
            
        }

        public static int SugarScorePreview(ObjectId id_usuario_calculado, bool usuario_premium, int scoreBase, TipoAcao TipoAcao, Acao acao)
        {
            UserBSN Usuario = new UserBSN();

            var ScoreUsuarioCalculado = Usuario.GetInformationByUserId(id_usuario_calculado);
            if (ScoreUsuarioCalculado == null)
            {
                ScoreUsuarioCalculado = new Mongo.Models.UserInformationModel();
                ScoreUsuarioCalculado.UserInformationId = id_usuario_calculado;
            }


            var sugarscore = Convert.ToInt32(ScoreUsuarioCalculado.sugar_score.GetValueOrDefault());

            return ELOVE.SugarScorePreview(ref sugarscore, usuario_premium, ref scoreBase, TipoAcao, acao);
        }

        private static int CalculaMMR(int EloUsuarioASerCalculado, bool usuario_premium, int EloUsadoDeBase, TipoAcao TipoAcao, Acao acao)
        {
            int EloCalculado = ELOVE.CalculateELO(ref EloUsuarioASerCalculado, usuario_premium, ref EloUsadoDeBase, TipoAcao, acao);


            if(EloCalculado < 0)
            {
                EloCalculado = 0;
            }
            else if(EloCalculado > 5000)
            {
                EloCalculado = 5000;
            }

            return EloCalculado;
        }
    }
}