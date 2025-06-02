using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.response;
using Domain.Entities;

namespace Application.interfaces
{
    public interface IUserService
    {
        UserDTO? AddUser(CreateUserDTO newUserData);

        UserDTO? GetById(int id);

        User? GetByIdCompleteData(int id);

        List<UserDTO>? GetAll();

        bool Delete(int id);
    }
}
