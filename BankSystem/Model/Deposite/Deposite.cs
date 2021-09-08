using BankSystem.Model.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.Deposite
{
    abstract class Deposite:IProfileControl, INotifyPropertyChanged
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

       private int id;

       private int profileId;

       private int rate;

       private DateTime startPeriod;

       private DateTime currentPeriod;

       private DateTime finishPeriod;

       private decimal currentBalance;

       private bool isActive;

       private bool isAccrualsContinue;

        AccountLogs logs;
        #endregion

        #region properties

        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        public int ProfileId { get { return profileId; } }

        public decimal CurrentBalance
        {
            get { return currentBalance; } 
            set
            {
                Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Изменение баланса на: {value - CurrentBalance}$");
                currentBalance = value;
                OnPropertyChanged("CurrentBalance"); 
            } 
        }

        public bool IsActive { get { return isActive; } set { isActive = value; OnPropertyChanged("IsActive"); } }

        public AccountLogs Logs { get { return logs; } set { logs = value; OnPropertyChanged("Logs"); } }

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
        public Deposite(int rate, int profileId, decimal startBalance, DateTime startPeriod, DateTime finishPeriod)
        {
            this.rate = rate;
            this.finishPeriod = finishPeriod;
            this.startPeriod = startPeriod;
            this.currentPeriod = startPeriod;
            this.currentBalance = startBalance;
            this.profileId = profileId;
            this.isActive = true;
            this.isAccrualsContinue = true;
            this.id = Deposite.NextId;
            this.logs = new AccountLogs();
            Deposite.AddDeposite(this);
        }

        #region methods

        /// <summary>
        /// Сдвиг текущей даты на месяц вперёд
        /// </summary>
        abstract public void GoNextMounth();

        /// <summary>
        /// Метод проверки активности депозита.
        /// </summary>
        abstract public void CheckActive();

        /// <summary>
        /// Метод перевода денежек на другой счёт.
        /// </summary>
        /// <param name="account">счёт, на который следует перевести денежки</param>
        /// <param name="sum">сумма, которую следует перевести</param>
        abstract public bool Transfer(IProfileControl account, decimal sum);

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

