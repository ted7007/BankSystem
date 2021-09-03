using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Model;
using BankSystem.Model.Deposite;
using BankSystem.Model.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using BankSystem.View;

namespace BankSystem.ViewModel
{
    class ClientProfileVM<T> : INotifyPropertyChanged
        where T : BankClientProfile
    {
        #region fields

        private T clientProfile;

        private TabItem selectedProfileControl;

        private BankAccount selectedAccount;

        private Loan selectedLoan;

        private Deposite selectedDeposite;

        private ButtonCommand addCommand;

        private ButtonCommand editCommand;

        private ButtonCommand closeCommand;

        private decimal newDepositeStartBalance;

        private ComboBoxItem newDepositeTypeString;

        private decimal newLoanLoanAmount;

        private int newDepositeMonths;
        #endregion

        #region properties
        /// <summary>
        /// Профиль, который следует отредактировать
        /// </summary>
        public T ClientProfile { get { return clientProfile; } set { clientProfile = value; OnPropertyChanged("ClientProfile"); } }

        public TabItem SelectedProfileControl { get { return selectedProfileControl; } set { selectedProfileControl = value; OnPropertyChanged("SelectedProfileControl"); } }

        /// <summary>
        /// Выбранный банковский счёт
        /// </summary>
        public BankAccount SelectedAccount { get { return selectedAccount; } set { selectedAccount = value; OnPropertyChanged("SelectedAccount"); } }

        /// <summary>
        /// Выбранный кредит
        /// </summary>
        public Loan SelectedLoan { get { return selectedLoan; } set { selectedLoan = value; OnPropertyChanged("SelectedLoan"); } }

        /// <summary>
        /// Выбранный вклад
        /// </summary>
        public Deposite SelectedDeposite { get { return selectedDeposite; } set { selectedDeposite = value; OnPropertyChanged("SelectedDeposite"); } }

        public ButtonCommand AddCommand { get { return addCommand ?? (addCommand = new ButtonCommand(a => AddProfileControl())); } }

        public ButtonCommand EditCommand { get { return editCommand ?? (editCommand = new ButtonCommand(a => EditProfileControl())); } }

        public ButtonCommand CloseCommand { get { return closeCommand ?? (closeCommand = new ButtonCommand(a => CloseProfileControl())); } }

        public decimal NewDepositeStartBalance { get { return newDepositeStartBalance; } set { newDepositeStartBalance = value; OnPropertyChanged("NewDepositeStartBalance"); } }

        public decimal NewLoanLoanAmount { get { return newLoanLoanAmount; } set { newLoanLoanAmount = value; OnPropertyChanged("NewLoanLoanAmount"); } }

        public ComboBoxItem NewDepositeTypeString { get { return newDepositeTypeString; } set { newDepositeTypeString = value; OnPropertyChanged("NewDepositeTypeString"); } }

        public int NewDepositeMonths { get { return newDepositeMonths; } set { newDepositeMonths = value; OnPropertyChanged("NewDepositeMonths"); } }

        #endregion

        public ClientProfileVM(T clientProfile)
        {
            this.clientProfile = clientProfile;
        }

        #region methods

        public void AddProfileControl()
        {
            switch (SelectedProfileControl.Header)
            {
                case "Accounts":
                    ClientProfile.AddBankAccount(0);
                    break;

                case "Deposites":
                    if (NewDepositeTypeString is null|| NewDepositeMonths == 0 || NewDepositeStartBalance == 0)
                        return;

                    DepositeType dt;
                    switch (NewDepositeTy﻿peString.Content)
                    {
                        case "Default":
                            dt = DepositeType.Default;
                            clientProfile.AddDeposite(dt, new DateTime().AddMonths(NewDepositeMonths), BankSystem.Model.BankSystem.CurrentDate, newDepositeStartBalance);
                            break;
                        case "WithCapitalization":
                            dt = DepositeType.WithCapitalization;
                            clientProfile.AddDeposite(dt, new DateTime().AddMonths(NewDepositeMonths), BankSystem.Model.BankSystem.CurrentDate, newDepositeStartBalance);
                            break;
                    }
                    break;
                case "Loans":
                    clientProfile.AddLoan(NewLoanLoanAmount, BankSystem.Model.BankSystem.CurrentDate);
                    break;
            }
        }

        public void EditProfileControl()
        {
            switch (SelectedProfileControl.Header)
            {
                case "Accounts":
                    if (SelectedAccount is null)
                        return;
                    AccountWindow aw = new AccountWindow();
                    aw.DataContext = new BankAccountVM(SelectedAccount);
                    aw.ShowDialog();
                    break;

                case "Deposites":
                    if (SelectedDeposite is null)
                        return;
                    DepositeWindow dw = new DepositeWindow();
                    dw.DataContext = new DepositeVM(SelectedDeposite);
                    dw.ShowDialog();
                    break;
                case "Loans":
                    if (SelectedLoan is null)
                        return;
                    LoanWindow lw = new LoanWindow();
                    lw.DataContext = selectedLoan;
                    lw.ShowDialog();
                    break;
            }
        }

        public void CloseProfileControl()
        {
            switch(SelectedProfileControl.Header)
            {
                case "Accounts":
                    if (SelectedAccount is null)
                        return;
                    clientProfile.RemoveBankAccount(SelectedAccount);
                    break;
            }
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