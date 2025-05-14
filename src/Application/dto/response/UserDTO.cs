using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.dto.response
{
    public class UserDTO
    {
       public int Id { get; set; }

       public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }


        public static UserDTO Create(User user)
        {
            var userDTO = new UserDTO();
            userDTO.Id = user.IdUser;
            userDTO.Name = user.Name;
            userDTO.LastName = user.LastName;
            userDTO.Email = user.Email;
            userDTO.PhoneNumber = user.PhoneNumber;
            return userDTO;
        }
    }
}
