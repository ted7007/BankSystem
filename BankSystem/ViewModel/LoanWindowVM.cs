using BankSystem.Model;
using BankSystem.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.ViewModel
{
    class LoanWindowVM : INotifyPropertyChanged
    {

        #region fields

        private Loan loan;

        private ButtonCommand checkLogsCommand;


        #endregion

        #region properties

        public Loan Loan { get { return loan; } set { loan = value; OnPropertyChanged("Loan"); } }

        /// <summary>
        /// Команда для открытия журнала действий
        /// </summary>
        public ButtonCommand CheckLogsCommand { get { return checkLogsCommand ?? (checkLogsCommand = new ButtonCommand(a => CheckLogs())); } }
        #endregion

        public LoanWindowVM(Loan loan)
        {
            this.loan = loan;
        }

        #region methods

        /// <summary>
        /// Метод для открытия окна с журналом действий
        /// </summary>
        public void CheckLogs()
        {
            AccountLogsWindow w = new AccountLogsWindow();
            w.DataContext = Loan.Logs;
            w.ShowDialog();
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
