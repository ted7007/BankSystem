using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model
{
    /// <summary>
    /// Интерфейс объекта профиля
    /// </summary>
    interface IProfileControl
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
    }
}
