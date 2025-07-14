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
        public int CategoryId { get; set; }

        [StringLength(155)]
        public string? ExpenseDescription { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public int TeamId { get; set; } = 0;
    }
}
