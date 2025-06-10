using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;


namespace Application.services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        public UserDTO? AddUser(CreateUserDTO newUserData)
        {
            if (newUserData.Password != newUserData.ConfirmPassword)
            {
                return null;
            }

            var ExistingUserByEmail = _userRepository.GetByEmail(newUserData.Email);
            if (ExistingUserByEmail != null)
            {
                throw new DuplicateUserDataException("Email");
            }

            var existingUserByPhone = _userRepository.GetByPhoneNumber(newUserData.PhoneNumber);
            if (existingUserByPhone != null)
            {
                throw new DuplicateUserDataException("número de teléfono");
            }

            User newUser = new User();
            newUser.Name = newUserData.Name;
            newUser.LastName = newUserData.LastName;
            newUser.Email = newUserData.Email;
            newUser.Password = newUserData.Password;
            newUser.PhoneNumber = newUserData.PhoneNumber;
            _userRepository.Add(newUser);
            return UserDTO.Create(newUser);
        }

        public UserDTO? UpdateUser(int userId, UpdateUserDTO newUserData)
        {
            var IsEmailInUse = _userRepository.GetByEmail(newUserData.Email);
            var IsPhoneNumberInUse = _userRepository.GetByPhoneNumber(newUserData.PhoneNumber);


            if (IsEmailInUse != null || IsPhoneNumberInUse != null)
                return null;


            User? userToUpdate = _userRepository.GetById(userId);
            if (userToUpdate != null)
            {
                userToUpdate.Name = newUserData.Name;
                userToUpdate.LastName = newUserData.LastName;
                userToUpdate.Email = newUserData.Email;
                userToUpdate.PhoneNumber = newUserData.PhoneNumber;
                _userRepository.Update(userToUpdate);
                UserDTO userUpdated = UserDTO.Create(userToUpdate);
                return userUpdated;
            }
            return null;
        }


        public List<UserDTO>? GetAll()
        {
            List<User> users = _userRepository.GetAll();
            List<UserDTO> userDTOs = new List<UserDTO>();

            if (users.Count > 0)
            {
                foreach (var u in users)
                {
                    UserDTO dto = UserDTO.Create(u);
                    userDTOs.Add(dto);
                }
                return userDTOs;
            }
            return null;
        }

        public UserDTO? GetById(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                UserDTO userDTO = UserDTO.Create(user);
                return userDTO;
            }
            return null;
        }

        public User? GetByIdCompleteData(int id)
        {
            User? user = _userRepository.GetById(id);
            return user;
        }

        public bool Delete(int id)
        {
            var user = _userRepository.GetById(id);
            if (user != null)
            {
                _userRepository.Delete(user);
                return true;
            }
            return false;
        }
    }
}