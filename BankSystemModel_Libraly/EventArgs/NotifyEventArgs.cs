using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemModel_Libraly.EventArgs
{
    /// <summary>
    /// Класс данных, которые следует передать в событие
    /// </summary>
    public class NotifyEventArgs
    {
        /// <summary>
        /// Сообщение, которое необходимо передать
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Объект, который вызывает оповещение
        /// </summary>
        public object Sender { get; }

        public NotifyEventArgs(object sender, string message)
        {
            Message = message;
            Sender = sender;
        }




    }
}
