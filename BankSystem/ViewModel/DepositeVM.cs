using BankSystem.Model.Client;
using BankSystem.Model.Deposite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public Deposite Deposite { get { return deposite; } set { deposite = value; OnPropertyChanged("Deposite"); } }

        public ComboBoxItem SelectedProfileControlToTransfer { get { return selectedProfileControlToTransfer; } set { selectedProfileControlToTransfer = value; OnPropertyChanged("SelectedProfileControlToTransfer"); } }

        public int IdToTransfer { get { return idToTransfer; } set { idToTransfer = value; OnPropertyChanged("IdToTransfer"); } }

        public decimal SumToTransfer { get { return sumToTransfer; } set { sumToTransfer = value; OnPropertyChanged("SumToTransfer"); } }

        public ButtonCommand TransferCommand { get { return transferCommand ?? (transferCommand = new ButtonCommand(a => Transfer())); } }
        #endregion

        public DepositeVM(Deposite deposite)
        {
            this.deposite = deposite;
        }

        #region methods

        public void Transfer()
        {
            if (SelectedProfileControlToTransfer is null || SumToTransfer == 0)
                return;
            switch (SelectedProfileControlToTransfer.Content as string)
            {
                case "Bank account":
                    if (BankSystem.Model.BankAccount.Find(IdToTransfer) is null)
                        return;
                    deposite.Transfer(BankSystem.Model.BankAccount.Find(IdToTransfer), SumToTransfer);
                    OnPropertyChanged("BankAccount.CurrentBalance");
                    break;
                case "Deposite":
                    if (BankSystem.Model.Deposite.Deposite.Find(IdToTransfer) is null)
                        return;
                    deposite.Transfer(BankSystem.Model.Deposite.Deposite.Find(IdToTransfer), SumToTransfer);
                    break;
                case "Loan":
                    if (BankSystem.Model.Loan.Find(IdToTransfer) is null)
                        return;
                    deposite.Transfer(BankSystem.Model.Loan.Find(IdToTransfer), SumToTransfer);
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
