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
        public DefaultDeposite(Action<EventArgs.NotifyEventArgs> notifyRelease, int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod)
            :base(notifyRelease, rate, profileId, startBalance, startPeriod, finishPeriod) { }

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
                decimal diff = CurrentBalance * ((decimal)Rate / 100);
                CallNotify(new EventArgs.AccountEventArgs(this, "Транзакция успешна", diff, EventArgs.AccountNotifyType.AccrualOfInterest));
                this.CurrentBalance += diff;         //
            }
        }

    }
}
