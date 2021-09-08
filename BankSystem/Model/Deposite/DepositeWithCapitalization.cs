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
        public DepositeWithCapitalization(int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod) : base(rate, profileId, startBalance, startPeriod, finishPeriod) { }

        public override void GoNextMounth()
        {
                                                                                        //
            if (!IsAccrualsContinue)                                                    //     При переходе на следующий год начисляются проценты.
                return;                                                                 //
                                                                                        //
            CurrentPeriod = CurrentPeriod.AddMonths(1);                                 //
            CheckActive();                                                              //
            decimal diff = CurrentBalance * ((decimal)Rate / 100);
            CurrentBalance += diff;                                                     //
            Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Начисление на - {diff}$");

        }

        public override void CheckActive()
        {
            if (CurrentBalance <= 0)
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
            Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Перевод на счёт №{account.Id} типа {account.GetType().Name} - {sum}$");
            return true;
        }
    }
}
