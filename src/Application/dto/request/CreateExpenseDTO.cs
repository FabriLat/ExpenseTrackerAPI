using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application.dto.request
{
    public class CreateExpenseDTO
    {

        [Required]
        public decimal Amount { get; set; }

        [StringLength(45)]
        [Required]
        public string ExpenseName { get; set; } = null!;

        [StringLength(155)]
        public string? Description { get; set; }

        public int? GroupId { get; set; }
    }
}
