using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Client
{
    class RegularUser:BankClientProfile
    {
        public RegularUser(string name, int depositeRate, int loanRate):base(name, depositeRate, loanRate) { }

        public string GetPaidSupport(ref BankAccount account)
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
