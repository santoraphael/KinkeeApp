using System;
using System.Web;
using System.Web.Security;
using System.Linq;
using System.Web.Services;

namespace Plataforma.Helper
{
    public static class CommonHelper
    {
        public static string CalculateRelativeTime(DateTime myDateTime)
        {
            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - myDateTime.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            string textTime = "";

            if (delta < 1 * MINUTE)
            {
                textTime = ts.Seconds == 1 ? "um segundo atrás" : ts.Seconds + " segundos atrás";
            }
            else if (delta < 2 * MINUTE)
            {
                textTime = "um minuto atrás";
            }
            else if (delta < 45 * MINUTE)
            {
                textTime = ts.Minutes + " minutos atrás";
            }
            else if (delta < 90 * MINUTE)
            {
                textTime = "uma hora atrás";
            }
            else if (delta < 24 * HOUR)
            {
                textTime = ts.Hours + " horas atrás";
            }
            else if (delta < 48 * HOUR)
            {
                textTime = "hoje";
            }
            else if (delta < 30 * DAY)
            {
                textTime = ts.Days + " dias Atrás";
            }
            else if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                textTime = months <= 1 ? "um mês atrás" : months + " meses atrás";
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                textTime = years <= 1 ? "um ano atrás" : years + " anos atrás";
            }

            return textTime;
        }
    }
}