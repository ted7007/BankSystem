using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemModel_Libraly.EventArgs;

namespace BankSystemModel_Libraly
{
    public interface IBankNotifyEvent
    {
        /// <summary>
        /// Событие для вызова оповещений
        /// </summary>
        event Action<NotifyEventArgs> BankNotifyEvent; 
    }
}
