using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Deposite
{
    /// <summary>
    /// Обычный депозит
    /// </summary>
    class DefaultDeposite : Deposite
    {
        public DefaultDeposite(int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod):base(rate, profileId, startBalance, startPeriod, finishPeriod) { }

        public override void GoNextMounth()
        {
                                                                                     //
            if (!IsAccrualsContinue)                                                 //     При переходе на следующий год начисляются проценты.
                return;                                                              //
            CurrentPeriod = CurrentPeriod.AddMonths(1);                              //
            CheckActive();                                                           //
            if((CurrentPeriod-StartPeriod).TotalDays/365>=1)                         //
            {                                                                        //
                StartPeriod = CurrentPeriod;                                         //
                CurrentBalance = CurrentBalance * ((decimal)Rate / 100 + 1);         //
            }
        }

        public override void CheckActive()
        {
            if (CurrentBalance<=0)
                IsActive = false;
            if (CurrentPeriod >= FinishPeriod)
                IsAccrualsContinue = false;
        }


        public override bool Transfer(IProfileControl account, decimal sum)
        {
            if (!IsActive || sum > CurrentBalance)
                return false;
            account.CurrentBalance += sum;
            CurrentBalance -= sum;
            return true;
        }
    }
}
