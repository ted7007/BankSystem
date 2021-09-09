using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Deposite
{
    /// <summary>
    /// Депозит с капитализацией
    /// </summary>
    class DepositeWithCapitalization : Deposite
    {
        public DepositeWithCapitalization(Action<EventArgs.NotifyEventArgs> notifyRelease,int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod)
            : base(notifyRelease, rate, profileId, startBalance, startPeriod, finishPeriod) { }

        public override void GoNextMounth()
        {
                                                                                        //
            if (!IsAccrualsContinue)                                                    //     При переходе на следующий год начисляются проценты.
                return;                                                                 //
                                                                                        //
            CurrentPeriod = CurrentPeriod.AddMonths(1);                                 //
            CheckActive();
            decimal diff = CurrentBalance * ((decimal)Rate / 100);
            CallNotify(new EventArgs.AccountEventArgs(this, "Транзакция успешна", diff, EventArgs.AccountNotifyType.AccrualOfInterest));
            CurrentBalance += diff;                                                    //
        }                                                                               

    }
}
