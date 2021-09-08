using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model
{
    /// <summary>
    /// класс банковского счёта
    /// </summary>
    class BankAccount : INotifyPropertyChanged, IProfileControl
    {
        #region static
        static private List<BankAccount> BAccounts;

        static private int nowId;

        /// <summary>
        /// Свойство для выявления следующего индетефикатора
        /// </summary>
        static private int NextId { get { BankAccount.nowId++; return BankAccount.nowId; } }

        /// <summary>
        /// Метод поиска счёта
        /// </summary>
        /// <param name="id"> индетефикатор счёта</param>
        /// <returns></returns>
        static public BankAccount Find(int id)
        {
            foreach(var i in BAccounts)
            {
                if (i.Id == id)
                    return i;
            }
            return null;
        }
        static BankAccount()
        {
            BAccounts = new List<BankAccount>();
            nowId = -1;
        }

        /// <summary>
        /// Метод добавления счёта в список банковской системы
        /// </summary>
        /// <param name="bAccount">банковский аккаунт, который следует добавить</param>
        static void AddBAccount(BankAccount bAccount)
        {
            BAccounts.Add(bAccount);
        }

        #endregion

        #region fields

        private int id;

        private int profileId;

        private decimal currentBalance;

        private bool isActive;

        private AccountLogs logs;

        #endregion

        #region properties

        public int Id { get { return id; } }

        public int ProfileId { get { return profileId; } }

        public decimal CurrentBalance 
        {
            get { return currentBalance; }
            set
            {
                Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Изменение баланса на : {value - CurrentBalance}$");
                currentBalance = value;
                OnPropertyChanged("CurrentBalance"); 
            }
        }

        public AccountLogs Logs { get { return logs; } set { logs = value; OnPropertyChanged("Logs"); } }

        public bool IsActive { get { return isActive; } set { isActive = false; OnPropertyChanged("IsActive"); } }

        #endregion

        public BankAccount(int profileId, int currentBalance = 0)
        {
            this.profileId = id;
            this.currentBalance = currentBalance;
            this.logs = new AccountLogs();
            this.id = NextId;
            this.isActive = true;
            BankAccount.AddBAccount(this);
        }

        #region methods

        /// <summary>
        /// Метод перевода средств на другой счёт
        /// </summary>
        /// <param name="account"></param>
        /// <param name="sum"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool Transfer(IProfileControl account, decimal sum)
        {
            if (!IsActive||sum>CurrentBalance)
                return false;
            account.CurrentBalance += sum;
            CurrentBalance -= sum;
            Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Перевод на счёт №{account.Id} типа {account.GetType().Name} - {sum}$");
            return true;
        }

        /// <summary>
        /// Метод для снятия средств
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool Withdraw(decimal sum)
        {
            if (!IsActive||sum>CurrentBalance)
                return false;
            
            CurrentBalance -= sum;
            Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Снятие наличных - {sum}$");
            return true;
        }

        /// <summary>
        /// Метод для пополнения средств
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public bool Fill(decimal sum)
        {
            if (!isActive)
                return false;
            CurrentBalance += sum;
            Logs.AddLog($"[{Logs.CurrentDate.ToShortTimeString()}]: Пополнение на - {sum}$");
            return true;
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
