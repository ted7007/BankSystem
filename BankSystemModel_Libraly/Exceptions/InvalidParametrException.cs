using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemModel_Libraly.Exceptions
{
    /// <summary>
    /// Исключение вызывается в случае недействительного параметра
    /// </summary>
    class InvalidParametrException:Exception
    {
        public InvalidParametrException():base()
        {

        }

        public InvalidParametrException(string msg) : base(msg)
        {

        }
    }
}
