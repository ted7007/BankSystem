using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Client
{
    class EntityClient:BankClientProfile
    {
        public EntityClient(string name, int depositeRate, int loanRate) : base(name, depositeRate, loanRate) { }

        public string GetFreeSupport()
        {
            return "We are very sorry that you are experiencing inconvenience when using our service. The problem will be solved soon..";
        }
    }
}
