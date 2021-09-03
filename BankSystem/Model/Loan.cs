﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model
{
    class Loan:INotifyPropertyChanged,IProfileControl
    {
        #region static
        static private List<Loan> Loans;

        static private int nowId;
        static private int NextId { get { Loan.nowId++; return Loan.nowId; } }

       

        static public Loan Find(int id)
        {
            foreach (var i in Loans)
            {
                if (i.Id == id)
                    return i;
            }
            return null;
        }
        static Loan()
        {
            Loans = new List<Loan>();
            nowId = -1;
        }

        static void AddDeposite(Loan loan)
        {
            Loans.Add(loan);
        }

        #endregion

        #region fields

        private int id;

        private int profileId;

        private bool isActive;

        private decimal loanAmount;

        private decimal currentBalance;

        private int rate;

        private DateTime startPeriod;

        private DateTime currentPeriod;

        #endregion

        #region properties
        public int Id { get { return id; } set { id = value; OnPropertyChanged("Id"); } }

        public int ProfileId { get { return profileId; } set { profileId = value; OnPropertyChanged("ProfileId"); } }

        public decimal CurrentBalance { get { return currentBalance; } set { currentBalance = value; OnPropertyChanged("CurrentBalance"); CheckActive(); OnPropertyChanged("IsActive"); } }

        public bool IsActive { get { return isActive; } set { isActive = value; OnPropertyChanged("IsActive"); } }

        public decimal LoanAmount { get { return loanAmount-currentBalance; } set { loanAmount = value; OnPropertyChanged("LoanAmount"); } }

        public int Rate { get { return rate; } set { rate = value; OnPropertyChanged("Rate"); } }

        public DateTime StartPeriod { get { return startPeriod; } set { startPeriod = value; OnPropertyChanged("StartPeriod"); } }

        public DateTime CurrentPeriod { get { return currentPeriod; } set { currentPeriod = value; OnPropertyChanged("CurrentPeriod"); } }


        #endregion

        public Loan(decimal startLoanAmount, int loanRate, DateTime startPeriod, int profileId)
        {
            this.loanAmount = startLoanAmount;
            this.rate = loanRate;
            this.profileId = profileId;
            this.id = Loan.NextId;
            this.startPeriod = startPeriod;
            this.currentPeriod = startPeriod;
            this.isActive = true;

            Loan.Loans.Add(this);
        }

        #region methods

        public void GoNextMonth()
        {
            if (!isActive)
                return;
            CurrentPeriod = CurrentPeriod.AddMonths(1);

            CheckActive();
            if ((CurrentPeriod - StartPeriod).TotalDays / 365 >= 1)
            {
                StartPeriod = CurrentPeriod;
                LoanAmount += LoanAmount * ((decimal)rate / 100);
            }

            
        }

        public void CheckActive()
        {
            if (loanAmount <= 0)
                isActive = false;
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