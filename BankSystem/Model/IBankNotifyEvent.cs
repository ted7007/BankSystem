using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystem.Model.EventArgs;

namespace BankSystem.Model
{
    interface IBankNotifyEvent
    {
        /// <summary>
        /// Событие для вызова оповещений
        /// </summary>
        event Action<NotifyEventArgs> BankNotifyEvent; 
    }
}
