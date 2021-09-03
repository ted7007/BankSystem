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
        static private int NextId { get { Deposite.nowId++; return Deposite.nowId; } }

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
        #endregion

        #region properties
        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        public int ProfileId { get { return profileId; } }

        public decimal CurrentBalance { get { return currentBalance; } set { currentBalance = value; OnPropertyChanged("CurrentBalance"); } }
        public bool IsActive { get { return isActive; } set { isActive = value; OnPropertyChanged("IsActive"); } }

        public DateTime StartPeriod { get { return startPeriod; } set { startPeriod = value; OnPropertyChanged("StartPeriod"); } }

        public DateTime CurrentPeriod { get { return currentPeriod; } set { currentPeriod = value; OnPropertyChanged("CurrentPeriod"); } }


        public DateTime FinishPeriod { get { return finishPeriod; } set { finishPeriod = value; OnPropertyChanged("FinishPeriod"); } }

        public int Rate { get { return rate; } set { rate = value; OnPropertyChanged("Rate"); } }

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
            this.id = Deposite.NextId;
            Deposite.AddDeposite(this);
        }

        #region methods

        /// <summary>
        /// Сдвиг текущей даты на месяц вперёд
        /// </summary>
        abstract public void GoNextMounth();

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

