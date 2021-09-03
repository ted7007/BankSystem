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
        static private int NextId { get { BankAccount.nowId++; return BankAccount.nowId; } }

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

        static void AddBAccount(BankAccount bAccount)
        {
            BAccounts.Add(bAccount);
        }

        #endregion

        #region fields

        private int id;

        private int profileId;

        private decimal currentBalance;

        private ObservableCollection<string> logs;

        private bool isActive;

        #endregion

        #region properties

        public int Id { get { return id; } }

        public int ProfileId { get { return profileId; } }

        public decimal CurrentBalance { get { return currentBalance; }  set { currentBalance = value; } }

        public ObservableCollection<string> Logs { get { return logs; } private set { logs = value; } }

        public bool IsActive { get { return isActive; } set { isActive = false; } }

        #endregion

        public BankAccount(int profileId, int currentBalance = 0)
        {
            this.profileId = id;
            this.currentBalance = currentBalance;
            this.logs = new ObservableCollection<string>();
            this.id = NextId;
            this.isActive = true;
            logs.Add($"[{DateTime.Now}] : Account created");
            BankAccount.AddBAccount(this);
        }

        #region methods

        public bool Transfer(IProfileControl account, decimal sum, string log = "")
        {
            if (!IsActive||sum>CurrentBalance)
                return false;
            account.CurrentBalance += sum;
            CurrentBalance -= sum;
            Logs.Add(log);
            return true;
        }

        public bool Withdraw(decimal sum, string log = "")
        {
            if (!IsActive||sum>CurrentBalance)
                return false;
            
            CurrentBalance -= sum;
            Logs.Add(log);
            return true;
        }

        public bool Fill(decimal sum, string log = "")
        {
            if (!isActive)
                return false;
            CurrentBalance += sum;
            Logs.Add(log);
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
