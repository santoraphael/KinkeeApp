using System;
using static Miscellaneous.SugarScore.Scores;

namespace Miscellaneous.SugarScore
{
    public class Scores
    {
        public static PontuacaoNivelGenero PegarEloGenero(int? Points, string genero)
        {
            var pedracartao = PegarPedraCartao(Points);
            PontuacaoNivelGenero pontuacaoNivelGenero = new PontuacaoNivelGenero();

            //if(string.IsNullOrEmpty(genero))
            //{
            //    pontuacaoNivelGenero.idEloUsuario = pedracartao.idEloUsuario;
            //    pontuacaoNivelGenero.nomeElo = Enum.GetName(typeof(EloCartao), pedracartao.nivelCartao);
            //    pontuacaoNivelGenero.urlImagemElo = pedracartao.urlImagemCartao;
            //    pontuacaoNivelGenero.rangeMinimo = pedracartao.rangeMinimo;
            //    pontuacaoNivelGenero.rangeMaximo = pedracartao.rangeMaximo;
            //}

            //if (genero.ToUpper() == "HOMEM")
            //{
            //    pontuacaoNivelGenero.idEloUsuario = pedracartao.idEloUsuario;
            //    pontuacaoNivelGenero.nomeElo = Enum.GetName(typeof(EloCartao),pedracartao.nivelCartao);
            //    pontuacaoNivelGenero.urlImagemElo = pedracartao.urlImagemCartao;
            //    pontuacaoNivelGenero.rangeMinimo = pedracartao.rangeMinimo;
            //    pontuacaoNivelGenero.rangeMaximo = pedracartao.rangeMaximo;
            //}
            //else if(genero.ToUpper() == "MULHER")
            //{
            //    pontuacaoNivelGenero.idEloUsuario = pedracartao.idEloUsuario;
            //    pontuacaoNivelGenero.nomeElo = Enum.GetName(typeof(EloPedra), pedracartao.nivelPedra);
            //    pontuacaoNivelGenero.urlImagemElo = pedracartao.urlImagemPedra;
            //    pontuacaoNivelGenero.rangeMinimo = pedracartao.rangeMinimo;
            //    pontuacaoNivelGenero.rangeMaximo = pedracartao.rangeMaximo;
            //}


            pontuacaoNivelGenero.idEloUsuario = pedracartao.idEloUsuario;
            pontuacaoNivelGenero.nomeElo = Enum.GetName(typeof(EloCartao), pedracartao.nivelCartao);
            pontuacaoNivelGenero.pontuacaoAtual = Points;
            pontuacaoNivelGenero.urlImagemElo = pedracartao.urlImagemCartao;
            pontuacaoNivelGenero.rangeMinimo = pedracartao.rangeMinimo;
            pontuacaoNivelGenero.rangeMaximo = pedracartao.rangeMaximo;


            return pontuacaoNivelGenero;
        }

        public static PontuacaoNivel PegarPedraCartao(int? Points)
        {
            if (Points == null)
            {
                Points = 0;
            }

            return DeterminarNivel(Points);
        }

        public static PontuacaoNivel DeterminarNivel(int? Points)
        {
            PontuacaoNivel nivel = new PontuacaoNivel();

            for (int i = 1; i < 8; i++)
            {
                var pontuacao = DeterminarPontuacaoPorNivel(i);

                if (Points >= pontuacao.rangeMinimo && Points <= pontuacao.rangeMaximo)
                {
                    nivel = pontuacao;

                    break;
                }
            }

            return nivel;
        }

        public static PontuacaoNivel DeterminarPontuacaoPorNivel(int idNivel)
        {
            PontuacaoNivel pontuacaoNivel = new PontuacaoNivel();

            switch (idNivel)
            {
                case 1:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.NACIONAL;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.NACIONAL);

                    pontuacaoNivel.nivelPedra = EloPedra.OPALA;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.OPALA);

                    pontuacaoNivel.rangeMinimo = 0;
                    pontuacaoNivel.rangeMaximo = 1499;

                    break;
                
                case 2:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.INTERNACIONAL;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.INTERNACIONAL);

                    pontuacaoNivel.nivelPedra = EloPedra.ESMERALDA;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.ESMERALDA);

                    pontuacaoNivel.rangeMinimo = 1500;
                    pontuacaoNivel.rangeMaximo = 1999;

                    break;

                case 3:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.GOLD;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.GOLD);

                    pontuacaoNivel.nivelPedra = EloPedra.ESPINELA;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.ESPINELA);

                    pontuacaoNivel.rangeMinimo = 2000;
                    pontuacaoNivel.rangeMaximo = 2499;

                    break;

                case 4:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.PLATINUM;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.PLATINUM);

                    pontuacaoNivel.nivelPedra = EloPedra.SAFIRA;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.SAFIRA);

                    pontuacaoNivel.rangeMinimo = 2500;
                    pontuacaoNivel.rangeMaximo = 2999;

                    break;

                case 5:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.BLACK;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.BLACK);

                    pontuacaoNivel.nivelPedra = EloPedra.RUBI;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.RUBI);

                    pontuacaoNivel.rangeMinimo = 3000;
                    pontuacaoNivel.rangeMaximo = 3499;

                    break;

                case 6:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.INFINITY;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.INFINITY);

                    pontuacaoNivel.nivelPedra = EloPedra.TURMALINA;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.TURMALINA);

                    pontuacaoNivel.rangeMinimo = 3500;
                    pontuacaoNivel.rangeMaximo = 3999;

                    break;

                case 7:
                    pontuacaoNivel.idEloUsuario = idNivel;
                    pontuacaoNivel.nivelCartao = EloCartao.UNLIMITED;
                    pontuacaoNivel.urlImagemCartao = PegaUrlCartao(EloCartao.UNLIMITED);

                    pontuacaoNivel.nivelPedra = EloPedra.DIAMANTE;
                    pontuacaoNivel.urlImagemPedra = PegaUrlPedra(EloPedra.DIAMANTE);

                    pontuacaoNivel.rangeMinimo = 4000;
                    pontuacaoNivel.rangeMaximo = 99999999;

                    break;
            }

            return pontuacaoNivel;
        }


        public static string PegaUrlCartao(EloCartao nivel)
        {
            string url = "";

            switch(nivel)
            {
                case EloCartao.NACIONAL:
                    url = "1_cartao_nacional.svg";
                    break;
                case EloCartao.INTERNACIONAL:
                    url = "2_cartao_internacional.svg";
                    break;
                case EloCartao.GOLD:
                    url = "3_cartao_gold.svg";
                    break;
                case EloCartao.PLATINUM:
                    url = "4_cartao_platinum.svg";
                    break;
                case EloCartao.BLACK:
                    url = "5_cartao_black.svg";
                    break;
                case EloCartao.INFINITY:
                    url = "6_cartao_infinity.svg";
                    break;
                case EloCartao.UNLIMITED:
                    url = "7_cartao_unlimited.svg";
                    break;

            }

            return url;
        }

        public static string PegaUrlPedra(EloPedra nivel)
        {
            string url = "";

            switch (nivel)
            {
                case EloPedra.OPALA:
                    url = "1_gema_opala.svg";
                    break;
                case EloPedra.ESMERALDA:
                    url = "2_gema_esmeralda.svg";
                    break;
                case EloPedra.ESPINELA:
                    url = "3_gema_espinela.svg";
                    break;
                case EloPedra.SAFIRA:
                    url = "4_gema_safira.svg";
                    break;
                case EloPedra.RUBI:
                    url = "5_gema_rubi.svg";
                    break;
                case EloPedra.TURMALINA:
                    url = "6_gema_turmalina.svg";
                    break;
                case EloPedra.DIAMANTE:
                    url = "7_gema_diamante.svg";
                    break;

            }

            return url;
        }

        public enum EloCartao
        {
            NACIONAL = 1,
            INTERNACIONAL = 2,
            GOLD = 3,
            PLATINUM = 4,
            BLACK = 5,
            INFINITY = 6,
            UNLIMITED = 7
        }

        public enum EloPedra
        {
            OPALA = 1,
            ESMERALDA = 2,
            ESPINELA = 3,
            SAFIRA = 4,
            RUBI = 5,
            TURMALINA = 6,
            DIAMANTE = 7
        }
    }

    public class PontuacaoNivel
    {
        public int idEloUsuario { get; set; }
        public EloCartao nivelCartao { get; set; }
        public string urlImagemCartao { get; set; }
        public EloPedra nivelPedra { get; set; }
        public string urlImagemPedra { get; set; }
        public int? rangeMinimo { get; set; }
        public int? rangeMaximo { get; set; }
    }

    public class PontuacaoNivelGenero
    {
        public int idEloUsuario { get; set; }
        public string urlImagemElo { get; set; }
        public string nomeElo { get; set; }
        public int? rangeMinimo { get; set; }
        public int? rangeMaximo { get; set; }
        public int? pontuacaoAtual { get; set; }
    }
}
