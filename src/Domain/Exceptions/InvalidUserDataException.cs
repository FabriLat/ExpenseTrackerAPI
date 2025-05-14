using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class DuplicateUserDataException : Exception
    {
        public DuplicateUserDataException(string field)
            : base($"El {field} ya está en uso.")
        {
        }

        public DuplicateUserDataException(string field, string message)
            : base($"El {field} ya está en uso: {message}")
        {
        }
    }
}
