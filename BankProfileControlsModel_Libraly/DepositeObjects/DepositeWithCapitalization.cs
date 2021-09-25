using BankComponentsModel_Libraly.EventArgs;
using BankComponentsModel_Libraly.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankProfileControlsModel_Libraly.DepositeObjects
{
    /// <summary>
    /// Депозит с капитализацией
    /// </summary>
    public class DepositeWithCapitalization : Deposite
    {
        public DepositeWithCapitalization(Action<NotifyEventArgs> notifyRelease,int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod)
            : base(notifyRelease, rate, profileId, startBalance, startPeriod, finishPeriod) { }

        public override void GoNextMounth()
        {
                                                                                        //
            if (!IsAccrualsContinue)                                                    //     При переходе на следующий год начисляются проценты.
                return;                                                                 //
                                                                                        //
            CurrentPeriod = CurrentPeriod.AddMonths(1);                                 //
            CheckActive();
            decimal diff = CurrentBalance.GetAccrualOfInterest(Rate) - CurrentBalance;
            CallNotify(new AccountEventArgs(this, "Transaction succesful finished", diff, AccountNotifyType.AccrualOfInterest));
            CurrentBalance = CurrentBalance.GetAccrualOfInterest(Rate);                                                    //
        }                                                                               

    }
}
