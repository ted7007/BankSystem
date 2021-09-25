using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankComponentsModel_Libraly.EventArgs;
using BankComponentsModel_Libraly.Exceptions;
using BankComponentsModel_Libraly.InterfaceObjects;

namespace BankProfileControlsModel_Libraly.BankAccountObjects
{
    /// <summary>
    /// класс банковского счёта
    /// </summary>
    public class BankAccount : INotifyPropertyChanged, IProfileControl
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

        /// <summary>
        /// событие оповещений
        /// </summary>
        public event Action<NotifyEventArgs> BankNotifyEvent;

        #endregion

        #region properties

        public int Id { get { return id; } }

        public int ProfileId { get { return profileId; } }

        public decimal CurrentBalance { get { return currentBalance; }  set { currentBalance = value; } }

        public bool IsActive { get { return isActive; } set { isActive = false; } }

        #endregion


        public BankAccount( Action<NotifyEventArgs> notifyRelease, int profileId, int currentBalance = 0)
        {
            if (currentBalance < 0)
                throw new InvalidParametrException("Start balance can't be less than 0");
            this.profileId = id;
            this.currentBalance = currentBalance;
            this.id = NextId;
            this.isActive = true;
            BankAccount.AddBAccount(this);
            this.BankNotifyEvent += notifyRelease;

            BankNotifyEvent?.Invoke(new NotifyEventArgs(this, $"new {this.GetType().Name} №{this.Id} claimed."));
        }

        #region methods

        /// <summary>
        /// Метод перевода средств на другой счёт
        /// </summary>
        /// <param name="account"></param>
        /// <param name="sum"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public void Transfer(IProfileControl account, decimal sum)
        {
            if (!IsActive || sum > CurrentBalance || (account is BankAccount&&account.Id==Id))
            {
                BankNotifyEvent?.Invoke(new AccountEventArgs(account, "Transaction canceled", sum, AccountNotifyType.Transfer));
                return;
            }
            account.Put(this, sum);
            this.CurrentBalance -= sum;
            BankNotifyEvent?.Invoke(new AccountEventArgs(account, "Transaction succesful finished", sum, AccountNotifyType.Transfer));
        }

        /// <summary>
        /// Метод для снятия средств
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public void Take(IProfileControl sender, decimal sum)
        {
            if (!IsActive||sum>CurrentBalance)
            {
                BankNotifyEvent?.Invoke(new AccountEventArgs(sender, "Transaction canceled", sum, AccountNotifyType.Take));
                return ;
            }
           
            
            CurrentBalance -= sum;
            BankNotifyEvent?.Invoke(new AccountEventArgs(sender, "Transaction succesful finished", sum, AccountNotifyType.Take));

        }

        /// <summary>
        /// Метод для пополнения средств
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public void Put(IProfileControl sender, decimal sum)
        {
            if (!isActive)
            {
                BankNotifyEvent?.Invoke(new AccountEventArgs(sender, "Transaction canceled", sum, AccountNotifyType.Put));

                return;
            }
            CurrentBalance += sum;
            BankNotifyEvent?.Invoke(new AccountEventArgs(sender, "Transaction succesful finished", sum, AccountNotifyType.Put));
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
