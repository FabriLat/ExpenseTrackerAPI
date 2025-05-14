using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;
using Application.dto.response;

namespace Application.interfaces
{
    public interface IExpenseService
    {
        bool AddExpense(CreateExpenseDTO expense, int userId);

        List<ExpenseDTO> GetExpenses(int userId);

        List<ExpenseDTO> GetByDate(DateOnly date, int userId);
    }
}
