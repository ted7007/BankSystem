using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.Model.EventArgs
{
    /// <summary>
    /// класс, который содержит данные о транзакции
    /// </summary>
    class AccountEventArgs:NotifyEventArgs
    {
        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal Sum { get; }

        /// <summary>
        /// Тип транзакции
        /// </summary>
        public AccountNotifyType Type { get; }
        

        public AccountEventArgs(object sender, string message, decimal sum, AccountNotifyType type):base(sender,message)
        {
            Sum = sum;
            Type = type;
        }

    }

    /// <summary>
    /// Тип транзакции
    /// </summary>
    enum AccountNotifyType
    {
        /// <summary>
        /// Транзакция перевода
        /// </summary>
        Transfer,
        /// <summary>
        /// Транзакция снятия
        /// </summary>
        Take,
        /// <summary>
        /// Транзацкия пополнения
        /// </summary>
        Put,
        /// <summary>
        /// Начисление процентов
        /// </summary>
        AccrualOfInterest
    }
}
