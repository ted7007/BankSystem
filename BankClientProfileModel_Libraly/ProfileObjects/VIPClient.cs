using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClientProfileModel_Libraly.PorfileObjects
{
    /// <summary>
    /// ВИП клиент как банковский профиль
    /// </summary>
    public class VIPClient:BankClientProfile
    {
        public VIPClient(string name, int depositeRate, int loanRate):base(name, depositeRate, loanRate) { }

        /// <summary>
        /// получение бесплатной поддержки
        /// </summary>
        /// <returns></returns>
        public string GetFreeSupport()
        {
            return "We are very sorry that you are experiencing inconvenience when using our service. The problem will be solved soon..";
        }
    }
}
