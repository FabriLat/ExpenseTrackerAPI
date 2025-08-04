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

        List<ExpenseDTO>? GetByTeamId(int userId, int teamId);

        bool DeleteExpense(int expenseId, int userId);

        List<ExpenseDTO> ?GetUserExpensesForTeam(int userId, int teamId);

       decimal GetTotalExpensesByUser(int userId, int? teamId);

        List<ExpenseDTO> GetCurrentMonthExpenses(int userId);

        decimal GetTotalLastMonthExpenses(int userId);

        List<ExpenseDTO> GetByCategory(int userId, int teamId, string category);
    }
}
