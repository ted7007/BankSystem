﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemModel_Libraly.Extensions;

namespace BankSystemModel_Libraly.Deposite
{
    /// <summary>
    /// Депозит с капитализацией
    /// </summary>
    public class DepositeWithCapitalization : Deposite
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
            decimal diff = CurrentBalance.GetAccrualOfInterest(Rate) - CurrentBalance;
            CallNotify(new EventArgs.AccountEventArgs(this, "Transaction succesful finished", diff, EventArgs.AccountNotifyType.AccrualOfInterest));
            CurrentBalance = CurrentBalance.GetAccrualOfInterest(Rate);                                                    //
        }                                                                               

    }
}
