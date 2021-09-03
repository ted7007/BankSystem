using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Deposite
{
    class DepositeWithCapitalization : Deposite
    {
        public DepositeWithCapitalization(int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod) : base(rate, profileId, startBalance, startPeriod, finishPeriod) { }

        public override void GoNextMounth()
        {
            if (!IsActive)
                return;
            CurrentPeriod = CurrentPeriod.AddMonths(1);
            if (CurrentPeriod > FinishPeriod)
                IsActive = false;
            CurrentBalance = CurrentBalance * ((decimal)Rate / 100 + 1);
        }
    }
}
