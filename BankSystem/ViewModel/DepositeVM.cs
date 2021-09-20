using BankSystemModel_Libraly.Client;
using BankSystemModel_Libraly.Deposite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using BankSystemModel_Libraly;
using System.Windows;

namespace BankSystem.ViewModel
{
    class DepositeVM : INotifyPropertyChanged
    {
        #region fields

        private Deposite deposite;

        private ComboBoxItem selectedProfileControlToTransfer;

        private ButtonCommand transferCommand;

        private int idToTransfer;

        private decimal sumToTransfer;
        #endregion

        #region properties

        /// <summary>
        /// Депозит
        /// </summary>
        public Deposite Deposite { get { return deposite; } set { deposite = value; OnPropertyChanged("Deposite"); } }

        /// <summary>
        /// Выбранный объект профиля для перевода
        /// </summary>
        public ComboBoxItem SelectedProfileControlToTransfer { get { return selectedProfileControlToTransfer; } set { selectedProfileControlToTransfer = value; OnPropertyChanged("SelectedProfileControlToTransfer"); } }

        /// <summary>
        /// Индетефикатор для перевода
        /// </summary>
        public int IdToTransfer { get { return idToTransfer; } set { idToTransfer = value; OnPropertyChanged("IdToTransfer"); } }

        /// <summary>
        /// Сумма для перевода
        /// </summary>
        public decimal SumToTransfer { get { return sumToTransfer; } set { sumToTransfer = value; OnPropertyChanged("SumToTransfer"); } }

        /// <summary>
        /// Команда для перевода
        /// </summary>
        public ButtonCommand TransferCommand { get { return transferCommand ?? (transferCommand = new ButtonCommand(a => Transfer())); } }

        /// <summary>
        /// Тип депозита
        /// </summary>
        public DepositeType DepositeType 
        { 
            get 
            {
                if (Deposite is DefaultDeposite)
                    return DepositeType.Default;
                else
                    return DepositeType.WithCapitalization;
                    
            } 
        }
        #endregion

        public DepositeVM(Deposite deposite)
        {
            this.deposite = deposite;
        }

        #region methods

        /// <summary>
        /// Метод для перевода
        /// </summary>
        public void Transfer()
        {
            if (SelectedProfileControlToTransfer is null || SumToTransfer == 0)
            {
                MessageBox.Show("There is not enough information for translation.");
                return;
            }
            switch (SelectedProfileControlToTransfer.Content as string)
            {
                case "Bank account":
                    if (BankAccount.Find(IdToTransfer) is null)
                        return;
                    deposite.Transfer(BankAccount.Find(IdToTransfer), SumToTransfer);
                    OnPropertyChanged("BankAccount.CurrentBalance");
                    break;
                case "Deposite":
                    if (Deposite.Find(IdToTransfer) is null)
                        return;
                    deposite.Transfer(Deposite.Find(IdToTransfer), SumToTransfer);
                    break;
                case "Loan":
                    if (Loan.Find(IdToTransfer) is null)
                        return;
                    deposite.Transfer(Loan.Find(IdToTransfer), SumToTransfer);
                    break;
            }
            BankClientProfile.Find(deposite.ProfileId).RemoveUnactiveControls();
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
