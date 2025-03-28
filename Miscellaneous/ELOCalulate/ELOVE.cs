using System;

namespace Miscellaneous.ELOCalulate
{
    public class ELOVE
    {
        public static int CalculateELO(ref int EloUsuario, bool usuario_premium, ref int eloBase,TipoAcao tipoAcao, Acao outcome)
        {
            //O zero é sempre que o calculo for considerar como base a plataforma kinkee e não a interação direta
            // com um outro usuário. Como por exemplo. O cadastro
            if (eloBase == 0)
            {
                eloBase = EloUsuario;
            }


            //Esse é o valor chave para o calculo do delta, quanto maior o valor, maior será a pontuação ganha ou perdida.
            double eloK = KConstatCalculate(EloUsuario, 0, tipoAcao);

            //Se o usuário for premium é adicionado mais 10 no valor chave para o calculo do delta
            if (usuario_premium && Acao.Positiva == outcome)
            {
                eloK += 10;
            }

            double delta = (double)(eloK * ((double)outcome - ExpectationToLike(EloUsuario, eloBase)));

            EloUsuario += Convert.ToInt32(delta);

            //if (outcome == Acao.Positiva)
            //{
            //    EloUsuario += Convert.ToInt32(delta);
            //}
            //else if(outcome == Acao.Negativa)
            //{
            //    EloUsuario -= Convert.ToInt32(delta);
            //}


            return EloUsuario;
        }

        public static int SugarScorePreview(ref int EloUsuario, bool usuario_premium, ref int eloBase, TipoAcao tipoAcao, Acao outcome)
        {
            //O zero é sempre que o calculo for considerar como base a platagorma kinkee e não a interação direta
            // com um outro usuário. Como por exemplo. O cadastro
            if (eloBase == 0)
            {
                eloBase = EloUsuario;
            }


            //Esse é o valor chave para o calculo do delta, quanto maior o valor, maior será a pontuação ganha ou perdida.
            double eloK = KConstatCalculate(EloUsuario, 0, tipoAcao);

            //Se o usuário for premium é adicionado mais 10 no valor chave para o calculo do delta
            if (usuario_premium && Acao.Positiva == outcome)
            {
                eloK += 10;
            }

            double delta = (double)(eloK * ((double)outcome - ExpectationToLike(EloUsuario, eloBase)));

            return Convert.ToInt32(delta);
        }

        static double ExpectationToLike(double EloUsuario, double eloBase)
        {
            return (1 / (1 + Math.Pow(10, (eloBase - EloUsuario) / 400.0)));
        }
        

        public static int KConstatCalculate(double Points, int nrAcoesNoElo, TipoAcao tipoAcao)
        {
            int kConstant = 0;
            Elo cartao = Elo.Nacional;

            if (Points >= 1 && Points <= 1499)
            {
                cartao = Elo.Nacional;
            }
            else if (Points >= 1500 && Points <= 1999)
            {
                cartao = Elo.Internacional;
            }
            else if (Points >= 2000 && Points <= 2499)
            {
                cartao = Elo.Gold;
            }
            else if (Points >= 2500 && Points <= 2999)
            {
                cartao = Elo.Platinum;
            }
            else if (Points >= 3000 && Points <= 3499)
            {
                cartao = Elo.Black;
            }
            else if (Points >= 3500 && Points <= 3999)
            {
                cartao = Elo.Infinity;
            }
            else if (Points >= 4000 && Points <= 5000)
            {
                cartao = Elo.Unlimited;
            }

            int pesoCalculo = 0;

            //Ajuste do Peso
            if(TipoAcao.Basica == tipoAcao)
            {
                pesoCalculo = 0;
            }
            else if (TipoAcao.Media == tipoAcao)
            {
                pesoCalculo = 10;
            }
            else if (TipoAcao.Essencial == tipoAcao)
            {
                pesoCalculo = 20;
            }
            else if (TipoAcao.Importante == tipoAcao)
            {
                pesoCalculo = 30;
            }


            switch (cartao)
            {
                case Elo.Nacional:
                    kConstant = 85 + pesoCalculo;
                    break;

                case Elo.Internacional:
                    kConstant = 70 + pesoCalculo;
                    break;

                case Elo.Gold:
                    kConstant = 60 + pesoCalculo;
                    break;

                case Elo.Platinum:
                    kConstant = 50 + pesoCalculo;
                    break;

                case Elo.Black:
                    kConstant = 40 + pesoCalculo;
                    break;

                case Elo.Infinity:
                    kConstant = 30 + pesoCalculo;
                    break;

                case Elo.Unlimited:
                    kConstant = 20 + pesoCalculo;
                    break;
            }

            return kConstant;
        }

        public enum Elo
        {
            Nacional = 1,
            Internacional = 2,
            Gold = 3,
            Platinum = 4,
            Black = 5,
            Infinity = 6,
            Unlimited = 7
        }

        public enum Acao
        {
            Positiva = 1,
            Negativa = 0
        }

        public enum TipoAcao
        {
            Basica = 1,
            Media = 2,
            Essencial = 3,
            Importante = 4
        }
    }
}
