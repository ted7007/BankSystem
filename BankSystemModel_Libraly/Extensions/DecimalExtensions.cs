using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemModel_Libraly.Extensions
{
    public static class DecimalExtensions
    {
        /// <summary>
        /// Метод возвращает сумму с начисленными процентами
        /// </summary>
        /// <param name="sum">Сумма, на которые необходимо начислить проценты</param>
        /// <param name="percents">Проценты, которые необходимо начислить проценты</param>
        /// <returns></returns>
        public static decimal GetAccrualOfInterest(this decimal sum, int percents)
        {
            return sum * (1 + (decimal)percents / 100);
        }
    }
}
