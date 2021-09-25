using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankComponentsModel_Libraly.EventArgs;
using BankComponentsModel_Libraly.Extensions;


namespace BankProfileControlsModel_Libraly.DepositeObjects
{
    /// <summary>
    /// Обычный депозит
    /// </summary>
    public class DefaultDeposite : Deposite
    {
        public DefaultDeposite(Action<NotifyEventArgs> notifyRelease, int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod)
            :base(notifyRelease, rate, profileId, startBalance, startPeriod, finishPeriod) { }

        public override void GoNextMounth()
        {
                                                                                     //
            if (!IsAccrualsContinue)                                                 //     При переходе на следующий год начисляются проценты.
                return;                                                              //
            CurrentPeriod = CurrentPeriod.AddMonths(1);                              //
            CheckActive();                                                           //
            if((CurrentPeriod-StartPeriod).GetYears()>=1)                         //
            {                                                                        //
                StartPeriod = CurrentPeriod;                                         //
                decimal diff = CurrentBalance.GetAccrualOfInterest(Rate);
                CallNotify(new AccountEventArgs(this, "Transaction succesful finished", diff, AccountNotifyType.AccrualOfInterest));
                this.CurrentBalance =CurrentBalance.GetAccrualOfInterest(Rate);         //
            }
        }

    }
}
