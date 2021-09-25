using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankComponentsModel_Libraly.Exceptions
{
    /// <summary>
    /// Исключение вызывается в случае недействительного параметра
    /// </summary>
    public class InvalidParametrException:Exception
    {
        public InvalidParametrException():base()
        {

        }

        public InvalidParametrException(string msg) : base(msg)
        {

        }
    }
}
