using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class NotUserInTeamException : Exception
    {
        public NotUserInTeamException()
            : base($"El usuario no pertenece al grupo seleccionado.")
        {
        }

    }
}