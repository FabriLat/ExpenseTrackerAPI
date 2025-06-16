using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;


namespace Domain.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
       List<Expense> GetByUserId(int userId);

        List<Expense> GetByTeamId(int teamId);

        List<Expense> GetByDate(DateOnly date, int userId);

        List<Expense> GetUserExpensesForTeam(int userId, int teamId);

        public decimal GetTotalExpensesByUserIdAndTeamId(int userId, int teamId);

        public decimal GetTotalExpensesByUserId(int userId);


    }
}
