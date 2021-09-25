using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BankComponentsModel_Libraly.EventArgs;
using BankComponentsModel_Libraly.InterfaceObjects;

namespace BankClientProfileModel_Libraly.LogObjects
{
    public class NotificationLogs:INotifyPropertyChanged
    {
        #region fields

        ObservableCollection<string> logs;


        #endregion

        #region properties

        public ObservableCollection<string> Logs { get { return logs; } }

        public object BankSystem { get; private set; }

        #endregion

        public NotificationLogs()
        {
            logs = new ObservableCollection<string>();
        }

        /// <summary>
        /// Метод добавления записи в журнал
        /// </summary>
        /// <param name="e"> данные для добавления записи</param>
        public void AddLog(NotifyEventArgs e)
        {
            if (e is null)
                return;
            switch(e.GetType().Name)
            {
                case "NotifyEventArgs":
                    logs.Add($"\n ---Notification---\n[{e.Sender.GetType().Name}] - Message: {e.Message}");
                    break;
                case "AccountEventArgs":
                    AccountEventArgs accountE = e as AccountEventArgs;
                    logs.Add($"---Transaction---\n[{(accountE.Sender as IProfileControl).GetType().Name} №{(accountE.Sender as IProfileControl).Id}] - " +
                             $"Type of transaction: {accountE.Type}; Sum of transaction: {accountE.Sum}$ \n" +
                             $"Message: {accountE.Message}");
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
