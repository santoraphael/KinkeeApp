using System;
using System.Text;
using static Miscellaneous.SugarScore.Scores;

namespace Miscellaneous.SugarScore
{
    public class Geral
    {
        public string GerarCodigoConvite()
        {
            Random random = new Random();
            String source = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            Int32 length = 9;

            StringBuilder builder = new StringBuilder(length);

            while (length-- > 0)
                builder.Append(source[random.Next(source.Length)]);

            return builder.ToString();
        }
    }
}
