using Application.dto.request;
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

        UserDTO? UpdateUser(int userId, UpdateUserDTO newUserData);

        List<UserDTO> GetByName(string fullName);

        bool Delete(int id);
    }
}
