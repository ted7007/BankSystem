using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemModel_Libraly.Extensions
{
    public static class TimeSpanExtensions
    {
        /// <summary>
        /// Метод возвращает количество лет в периоде времени
        /// </summary>
        /// <param name="timeSpan">Период времени, в котором необходимо посчитать кол-во лет</param>
        /// <returns></returns>
        public static double GetYears(this TimeSpan timeSpan)
        {
            return timeSpan.TotalDays / 365;
        }
    }
}
