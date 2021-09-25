using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemModel_Libraly;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using BankSystem.View;
using System.Windows;
using BankClientProfileModel_Libraly.PorfileObjects;
using BankProfileControlsModel_Libraly.BankAccountObjects;
using BankProfileControlsModel_Libraly.LoanObjects;
using BankProfileControlsModel_Libraly.DepositeObjects;

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

        /// <summary>
        /// Выбранный объект профиля
        /// </summary>
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

        /// <summary>
        /// Команда для добавления объекта
        /// </summary>
        public ButtonCommand AddCommand { get { return addCommand ?? (addCommand = new ButtonCommand(a => AddProfileControl())); } }

        /// <summary>
        /// Команда для редактирования объекта
        /// </summary>
        public ButtonCommand EditCommand { get { return editCommand ?? (editCommand = new ButtonCommand(a => EditProfileControl())); } }

        /// <summary>
        /// Команда для закрытия объекта
        /// </summary>
        public ButtonCommand CloseCommand { get { return closeCommand ?? (closeCommand = new ButtonCommand(a => CloseProfileControl())); } }

        /// <summary>
        /// Начальный баланс нового вклада
        /// </summary>
        public decimal NewDepositeStartBalance { get { return newDepositeStartBalance; } set { newDepositeStartBalance = value; OnPropertyChanged("NewDepositeStartBalance"); OnPropertyChanged("NewDepositeFinishSum"); } }

        /// <summary>
        /// Сумма кредита нового кредита
        /// </summary>
        public decimal NewLoanLoanAmount { get { return newLoanLoanAmount; } set { newLoanLoanAmount = value; OnPropertyChanged("NewLoanLoanAmount"); } }

        /// <summary>
        /// Тип нового вклада
        /// </summary>
        public ComboBoxItem NewDepositeTypeString { get { return newDepositeTypeString; } set { newDepositeTypeString = value; OnPropertyChanged("NewDepositeTypeString"); OnPropertyChanged("NewDepositeFinishSum"); } }

        /// <summary>
        /// Количество месяцев для нового вклада
        /// </summary>
        public int NewDepositeMonths { get { return newDepositeMonths; } set { newDepositeMonths = value; OnPropertyChanged("NewDepositeMonths"); OnPropertyChanged("NewDepositeFinishSum"); } }

        /// <summary>
        /// Конечная сумма нового вклада
        /// </summary>
        public decimal NewDepositeFinishSum 
        {
            get
            {
                if (NewDepositeMonths <= 0 || NewDepositeStartBalance <= 0 || NewDepositeTypeString is null)
                    return 0;
                DepositeType dt = DepositeType.Default;
                switch(NewDepositeTypeString.Content)
                {
                    case "Default":
                        dt = DepositeType.Default;
                        break;
                    case "WithCapitalization":
                        dt = DepositeType.WithCapitalization;
                        break;
                }
                return Deposite.CalculateFinishSum(dt, NewDepositeStartBalance, NewDepositeMonths, clientProfile.DepositeRate);
            }
        }

        #endregion

        public ClientProfileVM(T clientProfile)
        {
            this.clientProfile = clientProfile;
        }

        #region methods

        /// <summary>
        /// Метод для добавления объекта профиля
        /// </summary>
        public void AddProfileControl()
        {
            switch (SelectedProfileControl.Header)
            {
                case "Accounts":
                    ClientProfile.AddBankAccount(0);
                    break;

                case "Deposites":
                    if (NewDepositeTypeString is null || NewDepositeMonths == 0 || NewDepositeStartBalance == 0)
                    {
                        MessageBox.Show("There is not enough information to open a deposit.");
                        return;
                    }

                    DepositeType dt;
                    switch (NewDepositeTy﻿peString.Content)
                    {
                        case "Default":
                            dt = DepositeType.Default;
                            clientProfile.AddDeposite(dt, new DateTime().AddMonths(NewDepositeMonths), BankSystemModel_Libraly.BankSystemComponents.BankSystem.CurrentDate, newDepositeStartBalance);
                            break;
                        case "WithCapitalization":
                            dt = DepositeType.WithCapitalization;
                            clientProfile.AddDeposite(dt, new DateTime().AddMonths(NewDepositeMonths), BankSystemModel_Libraly.BankSystemComponents.BankSystem.CurrentDate, newDepositeStartBalance);
                            break;
                    }
                    break;
                case "Loans":
                    if(newLoanLoanAmount<=0)
                    {
                        MessageBox.Show("The amount of the loan payment cannot be less than / equal to 0");
                        return;
                    }
                    clientProfile.AddLoan(NewLoanLoanAmount, BankSystemModel_Libraly.BankSystemComponents.BankSystem.CurrentDate);
                    break;
            }
        }

        /// <summary>
        /// Метод для редактирования объекта профиля
        /// </summary>
        public void EditProfileControl()
        {
            switch (SelectedProfileControl.Header)
            {
                case "Accounts":
                    if (SelectedAccount is null)
                    {
                        MessageBox.Show("The invoice is not selected for editing.");
                        return;
                    }
                    AccountWindow aw = new AccountWindow();
                    aw.DataContext = new BankAccountVM(SelectedAccount);
                    aw.ShowDialog();
                    break;

                case "Deposites":
                    if (SelectedDeposite is null)
                    {
                        MessageBox.Show("The contribution for editing is not selected.");
                        return;
                    }
                    DepositeWindow dw = new DepositeWindow();
                    dw.DataContext = new DepositeVM(SelectedDeposite);
                    dw.ShowDialog();
                    break;
                case "Loans":
                    if (SelectedLoan is null)
                    {
                        MessageBox.Show("No credit selected for editing.");

                        return;
                    }
                    LoanWindow lw = new LoanWindow();
                    lw.DataContext = selectedLoan;
                    lw.ShowDialog();
                    break;
            }
        }

        /// <summary>
        /// Метод для закрытия объекта профиля
        /// </summary>
        public void CloseProfileControl()
        {
            switch(SelectedProfileControl.Header)
            {
                case "Accounts":
                    if (SelectedAccount is null)
                    {
                        MessageBox.Show("The account to close is not selected.");

                        return;
                    }
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