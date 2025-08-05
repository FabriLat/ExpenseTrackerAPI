using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.dto.response
{
    public class ExpenseDTO
    {
        public int Id {  get; set; }

        public string ExpenseCategory {  get; set; }

        public string? Description { get; set; }

        public decimal Amount { get; set; }

        public DateOnly ExpenseDate { get; set; }

        public int? teamId { get; set; }

        public int userId { get; set; }



        public static ExpenseDTO Create(Expense expense)
        {
            var expenseDTO = new ExpenseDTO();
            expenseDTO.Id = expense.IdExpense;
            expenseDTO.ExpenseCategory = expense.ExpenseName;
            expenseDTO.Description = expense.Description;
            expenseDTO.Amount = expense.Amount;
            expenseDTO.ExpenseDate = expense.ExpenseDate;
            expenseDTO.teamId = expense.TeamId;
            expenseDTO.userId = expense.UserId;
            return expenseDTO;
        }

    }
}
