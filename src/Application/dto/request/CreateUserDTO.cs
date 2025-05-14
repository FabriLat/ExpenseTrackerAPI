using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.dto.response
{
    public class CreateUserDTO
    {
        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(20)]
        [Phone]
        [Required]
        public string PhoneNumber { get; set; }

        [StringLength(20)]
        [Required]
        public string Password { get; set; }

        [StringLength(20)]
        [Required]
        public string ConfirmPassword { get; set; }


        public static CreateUserDTO Create(User user)
        {
            var userDTO = new CreateUserDTO();
            userDTO.Name = user.Name;
            userDTO.LastName = user.LastName;
            userDTO.Email = user.Email;
            userDTO.PhoneNumber = user.PhoneNumber;
            return userDTO;
        }
    }
}

