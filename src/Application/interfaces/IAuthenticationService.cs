using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;

namespace Application.interfaces
{
    public interface IAuthenticationService
    {
        string Authenticate(AuthenticationRequest authenticationRequest);
     
    }
}
