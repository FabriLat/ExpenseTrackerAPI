using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class UserNotInTeamException : Exception
    {
        public UserNotInTeamException()
            : base($"El usuario no pertenece al grupo seleccionado.")
        {
        }

    }
}