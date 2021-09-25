using BankComponentsModel_Libraly.EventArgs;
using BankComponentsModel_Libraly.Exceptions;
using BankComponentsModel_Libraly.InterfaceObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankProfileControlsModel_Libraly.DepositeObjects
{
    public abstract class Deposite:IProfileControl, INotifyPropertyChanged
    {
        #region static
        static private List<Deposite> Deposites;

        static private int nowId;

        /// <summary>
        /// Свойство для выявления следующего индетефикатора
        /// </summary>
        static private int NextId { get { Deposite.nowId++; return Deposite.nowId; } }

        /// <summary>
        /// Метод поиска депозита
        /// </summary>
        /// <param name="id"> индетефикатор депозита</param>
        /// <returns></returns>
        static public Deposite Find(int id)
        {
            foreach (var i in Deposites)
            {
                if (i.Id == id)
                    return i;
            }
            return null;
        }
        static Deposite()
        {
            Deposites = new List<Deposite>();
            nowId = -1;
        }

        /// <summary>
        /// вычисление конечной суммы вклада
        /// </summary>
        /// <param name="type">тип вклада</param>
        /// <param name="startBalance">начальная сумма вклада</param>
        /// <param name="months">период, на который открывается вклад (в месяцах)</param>
        /// <param name="rate">ставка вклада</param>
        /// <returns></returns>
        static public decimal CalculateFinishSum(DepositeType type, decimal startBalance, int months, int rate)
        {
            decimal finishBalance = startBalance;
            switch(type)                                                                //     Для каждого типа депозита вычисления конечной суммы отличаются.
            {                                                                           //     
                case DepositeType.Default:                                              //     
                    while((float)months/12>=1)                                          //     У обычного депозита проценты начисляются раз в год.
                    {                                                                   //     
                        months /= 12;                                                   //     
                        finishBalance += finishBalance * (decimal)rate / 100;           //     
                    }                                                                   //     
                    break;                                                              //     
                case DepositeType.WithCapitalization:                                   //     
                    for(int i = 0; i < months; i++)                                     //     У депозита с капитализацией процентры начисляются каждый месяц.
                    {                                                                   //     
                        finishBalance += finishBalance * (decimal)rate / 100;           //     
                    }                                                                   //     
                    break;                                                              //     
            }                                                                           //     
            return finishBalance;
        }

        /// <summary>
        /// Метод добавления депозита в список банковской системы
        /// </summary>
        /// <param name="deposite">депозит, который следует добавить</param>
        static void AddDeposite(Deposite deposite)
        {
            Deposites.Add(deposite);
        }

        #endregion

        #region fields

        int id;

        int profileId;

        int rate;

        DateTime startPeriod;

        DateTime currentPeriod;

        DateTime finishPeriod;

        decimal currentBalance;

        bool isActive;

        bool isAccrualsContinue;

        public event Action<NotifyEventArgs> BankNotifyEvent;
        #endregion

        #region properties

        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        public int ProfileId { get { return profileId; } }

        public decimal CurrentBalance { get { return currentBalance; } set { currentBalance = value; OnPropertyChanged("CurrentBalance"); } }

        public bool IsActive { get { return isActive; } set { isActive = value; OnPropertyChanged("IsActive"); } }

        /// <summary>
        /// Начальная дата вклада
        /// </summary>
        public DateTime StartPeriod { get { return startPeriod; } set { startPeriod = value; OnPropertyChanged("StartPeriod"); } }

        /// <summary>
        /// Текущая дата вклада
        /// </summary>
        public DateTime CurrentPeriod { get { return currentPeriod; } set { currentPeriod = value; OnPropertyChanged("CurrentPeriod"); } }

        /// <summary>
        /// Конечная дата вклада
        /// </summary>
        public DateTime FinishPeriod { get { return finishPeriod; } set { finishPeriod = value; OnPropertyChanged("FinishPeriod"); } }

        /// <summary>
        /// Ставка по вкладу
        /// </summary>
        public int Rate { get { return rate; } set { rate = value; OnPropertyChanged("Rate"); } }

        /// <summary>
        /// Состояние начислений
        /// </summary>
        public bool IsAccrualsContinue { get { return isAccrualsContinue; } set { isAccrualsContinue = value; OnPropertyChanged("IsAccrualsContinue"); } }

        #endregion

        public Deposite(Action<NotifyEventArgs> notifyRelease, int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod)
        {
            if (rate < 0 || startBalance < 0 || startPeriod > finishPeriod)
                throw new InvalidParametrException("Invalid parametr. rate, startBalance can't be less than 0; startPeriod can't be more than finishPeriod");
            this.rate = rate;
            this.finishPeriod = finishPeriod;
            this.startPeriod = startPeriod;
            this.currentPeriod = startPeriod;
            this.currentBalance = startBalance;
            this.profileId = profileId;
            this.isActive = true;
            this.isAccrualsContinue = true;
            this.id = Deposite.NextId;
            this.BankNotifyEvent += notifyRelease;

            Deposite.AddDeposite(this);
            BankNotifyEvent?.Invoke(new NotifyEventArgs(this, $"new {this.GetType().Name} №{this.Id} claimed."));
        }

        #region methods

        /// <summary>
        /// Сдвиг текущей даты на месяц вперёд
        /// </summary>
        abstract public void GoNextMounth();

        /// <summary>
        /// Метод проверки активности депозита.
        /// </summary>
        public void CheckActive()
        {
            if (CurrentBalance <= 0)
                IsActive = false;
            if (CurrentPeriod >= FinishPeriod)
                IsAccrualsContinue = false;
        }

        /// <summary>
        /// Метод перевода денежек на другой счёт.
        /// </summary>
        /// <param name="account">счёт, на который следует перевести денежки</param>
        /// <param name="sum">сумма, которую следует перевести</param>
        public void Transfer(IProfileControl account, decimal sum)
        {
            if (!IsActive || sum > CurrentBalance)
            {
                CallNotify(new AccountEventArgs(account, "Transaction canceled", sum, AccountNotifyType.Transfer));
                return;
            }
            account.Put(this, sum);
            this.CurrentBalance -= sum;
            CallNotify(new AccountEventArgs(account, "Transaction succesful finished", sum, AccountNotifyType.Transfer));

        }

        public void Put(IProfileControl sender, decimal sum)
        {
            if (!IsActive)
            {
                CallNotify(new AccountEventArgs(sender, "Transaction canceled", sum, AccountNotifyType.Take));
                return;
            }
            this.CurrentBalance += sum;
            CallNotify(new AccountEventArgs(sender, "Transaction succesful finished", sum, AccountNotifyType.Put));

        }

        public void Take(IProfileControl sender, decimal sum)
        {
            if (!IsActive | this.CurrentBalance < sum)
            {
                CallNotify(new AccountEventArgs(sender, "Transaction canceled", sum, AccountNotifyType.Take));
                return;
            }
            this.CurrentBalance -= sum;
            CallNotify(new AccountEventArgs(sender, "Transaction succesful finished", sum, AccountNotifyType.Take));
        }

        internal void CallNotify(NotifyEventArgs e)
        {
            BankNotifyEvent?.Invoke(e);
        }

        #endregion

        #region INPC
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        #endregion
    }
}

