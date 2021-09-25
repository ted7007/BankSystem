using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankComponentsModel_Libraly.EventArgs;

namespace BankComponentsModel_Libraly.InterfaceObjects
{
    public interface IBankNotifyEvent
    {
        /// <summary>
        /// Событие для вызова оповещений
        /// </summary>
        event Action<NotifyEventArgs> BankNotifyEvent; 
    }
}
