using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemModel_Libraly
{
    /// <summary>
    /// Интерфейс объекта профиля
    /// </summary>
    public interface IProfileControl:IBankNotifyEvent
    {
        /// <summary>
        /// ID объекта
        /// </summary>
        int Id { get; }


        /// <summary>
        /// ID аккаунта владельца объекта
        /// </summary>
        int ProfileId { get; }

        /// <summary>
        /// Текущий баланс
        /// </summary>
        decimal CurrentBalance { get; set; }

        /// <summary>
        /// Состояние объекта
        /// </summary>
        bool IsActive { get; set; }

        /// <summary>
        /// Метод принятия средств
        /// </summary>
        /// <param name="sender">объект вызывающий метод</param>
        /// <param name="sum">сумма транзакции</param>
        void Put(IProfileControl sender, decimal sum);

        /// <summary>
        /// Метод отчисления средств
        /// </summary>
        /// <param name="sender">объект вызывающий метод</param>
        /// <param name="sum">сумма транзакции</param>
        void Take(IProfileControl sender, decimal sum);
    }
}
