using BankSystem.Model.EventArgs;
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
    class NotificationLogs:INotifyPropertyChanged
    {
        #region fields

        ObservableCollection<string> logs;


        #endregion

        #region properties

        public ObservableCollection<string> Logs { get { return logs; } }

        public DateTime CurrentDate { get { return BankSystem.CurrentDate; } }

        #endregion

        public NotificationLogs()
        {
            logs = new ObservableCollection<string>();
        }

        public void AddLog(NotifyEventArgs e)
        {
            if (e is null)
                return;
            switch(e.GetType().Name)
            {
                case "NotifyEventArgs":
                    logs.Add($"[{BankSystem.CurrentDate.ToString()}|{(e.Sender as Client.BankClientProfile).Name}]: {e.Message}");
                    break;
                case "AccountEventArgs":
                    AccountEventArgs accountE = e as AccountEventArgs;
                    logs.Add($"[{BankSystem.CurrentDate.ToString()}|{(accountE.Sender as IProfileControl).GetType().Name} №{(accountE.Sender as IProfileControl).Id}] - " +
                             $"Тип транзакции: {accountE.Type}; Сумма транзакции: {accountE.Sum}$ \n" +
                             $"Сообщение: {accountE.Message}");
                    break;
            }
            
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
