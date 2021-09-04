using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Client
{
    /// <summary>
    /// Обычный пользователь как банковский профиль
    /// </summary>
    class RegularUser:BankClientProfile
    {
        public RegularUser(string name, int depositeRate, int loanRate):base(name, depositeRate, loanRate) { }

        /// <summary>
        /// Получить платную поддержку
        /// </summary>
        /// <param name="account">счёт, с которого поддержка будет оплачена</param>
        /// <returns></returns>
        public string GetPaidSupport(BankAccount account)
        {

            if (account.CurrentBalance >= 50)
            {
                account.Withdraw(50, "PaidSupport: -50 $");
                OnPropertyChanged("BankAccounts");
                return "U can get help in FAQ";
            }
            else
            {
                return "U havent 50 $ for paid support";
            }
        }
    }
}
