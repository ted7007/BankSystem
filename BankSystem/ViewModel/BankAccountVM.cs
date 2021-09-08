using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BankSystem.Model;
using BankSystem.Model.Client;

namespace BankSystem.ViewModel
{
    class BankAccountVM:INotifyPropertyChanged
    {
        #region fields

        private BankAccount bankAccount;

        private ComboBoxItem selectedProfileControlToTransfer;

        private ButtonCommand transferCommand;

        private ButtonCommand fillCommand;

        private int idToTransfer;

        private decimal sumToTransfer;

        private decimal sumToFill;
        #endregion

        #region properties

        /// <summary>
        /// Банковский счёт
        /// </summary>
        public BankAccount BankAccount { get { return bankAccount; }  set { bankAccount = value; OnPropertyChanged("BankAccount"); } }
        
        /// <summary>
        /// Выбранный объект со счётом для перевода
        /// </summary>
        public ComboBoxItem SelectedProfileControlToTransfer{ get { return selectedProfileControlToTransfer; } set { selectedProfileControlToTransfer = value; OnPropertyChanged("SelectedProfileControlToTransfer"); } }

        /// <summary>
        /// Индетефикатор для перевода
        /// </summary>
        public int IdToTransfer { get { return idToTransfer; } set { idToTransfer = value; OnPropertyChanged("IdToTransfer"); } }

        /// <summary>
        /// Сумма для перевода
        /// </summary>
        public decimal SumToTransfer { get { return sumToTransfer; } set { sumToTransfer = value; OnPropertyChanged("SumToTransfer"); } }

        /// <summary>
        /// Сумма для пополнения
        /// </summary>
        public decimal SumToFill { get { return sumToFill; } set { sumToFill = value; OnPropertyChanged("SumToFill"); } }

        /// <summary>
        /// Команда для перевода
        /// </summary>
        public ButtonCommand TransferCommand { get { return transferCommand ?? (transferCommand = new ButtonCommand(a => Transfer())); } }

        /// <summary>
        /// Команда для пополнения
        /// </summary>
        public ButtonCommand FillCommand { get { return fillCommand ?? (fillCommand = new ButtonCommand(a => Fill())); } }
        #endregion

        public BankAccountVM(BankAccount bankAccount)
        {
            this.bankAccount = bankAccount;
        }

        #region methods
        
        /// <summary>
        /// Метод для перевода средств
        /// </summary>
        public void Transfer()
        {
            if (SelectedProfileControlToTransfer is null || SumToTransfer==0)
                return;
            switch(SelectedProfileControlToTransfer.Content as string)
            {
                case "Bank account":
                    if (BankSystem.Model.BankAccount.Find(IdToTransfer) is null)
                        return;
                    bankAccount.Transfer(BankSystem.Model.BankAccount.Find(IdToTransfer), SumToTransfer);
                    OnPropertyChanged("BankAccount.CurrentBalance");
                    break;
                case "Deposite":
                    if (BankSystem.Model.Deposite.Deposite.Find(IdToTransfer) is null)
                        return;
                    bankAccount.Transfer(BankSystem.Model.Deposite.Deposite.Find(IdToTransfer), SumToTransfer);
                    break;
                case "Loan":
                    if (BankSystem.Model.Loan.Find(IdToTransfer) is null)
                        return;
                    bankAccount.Transfer(BankSystem.Model.Loan.Find(IdToTransfer), SumToTransfer);
                    break;
            }
            BankClientProfile.Find(bankAccount.ProfileId).RemoveUnactiveControls();
            OnPropertyChanged("BankAccount");
        }

        /// <summary>
        /// Метод для пополнения средств
        /// </summary>
        public void Fill()
        {
            if (SumToFill == 0)
                return;
            bankAccount.Fill(SumToFill);
            OnPropertyChanged("BankAccount");
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
