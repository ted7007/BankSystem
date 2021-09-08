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
    class AccountLogs:INotifyPropertyChanged
    {

        #region fields

        ObservableCollection<string> logs;


        #endregion

        #region properties

        public ObservableCollection<string> Logs { get { return logs; } }

        public DateTime CurrentDate { get { return BankSystem.CurrentDate; } }

        #endregion

        public AccountLogs()
        {
            logs = new ObservableCollection<string>();
        }

        public void AddLog(string msg)
        {
            logs.Add(msg);
            OnPropertyChanged("Logs");
        }

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
