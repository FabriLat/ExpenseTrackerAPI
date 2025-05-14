using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class InvalidAmountException : Exception
    {

            public InvalidAmountException()
                : base($"Se debe ingresar una cantidad de dinero mayor que 0.")
            {
            }

    }
}
