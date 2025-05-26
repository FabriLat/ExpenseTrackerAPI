using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.response;

namespace Application.interfaces
{
    public interface IUserService
    {
        UserDTO? AddUser(CreateUserDTO newUserData);

        UserDTO? GetById(int id);

        List<UserDTO>? GetAll();

        bool Delete(int id);
    }
}
