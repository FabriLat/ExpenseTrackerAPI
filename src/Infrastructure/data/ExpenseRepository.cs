using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure.data
{
    public class ExpenseRepository : BaseRepository<Expense>, IExpenseRepository
    {

        private readonly GestionGastosContext _context;
        public ExpenseRepository(GestionGastosContext context) : base(context)
        {
            _context = context;
        }

        public List<Expense> GetByUserId(int userId)
        {
            List<Expense> expenses = _context.Expenses.Where(e =>  e.UserId == userId).ToList();

            return expenses;
        }

        public List<Expense> GetByDate(DateOnly date, int userId)
        {
            List<Expense> expenses = _context.Expenses.Where(e => e.ExpenseDate == date && e.UserId == userId).ToList();
            return expenses;
        }

        public List<Expense> GetByTeamId(int teamId)
        {
            List<Expense> expenses = _context.Expenses.Where(e => e.TeamId == teamId).ToList();
            return expenses;
        }


        public List<Expense> GetUserExpensesForTeam(int userId, int teamId)
        {
            List<Expense> expenses = _context.Expenses.Where(e => e.UserId == userId && e.TeamId == teamId).ToList();
            return expenses;
        }


        public decimal GetTotalExpensesByUserId(int userId)
        {
            decimal totalExpenses = _context.Expenses.Where(u => u.UserId == userId && u.TeamId == null).Sum(e => e.Amount);

            return totalExpenses;
        }

        public decimal GetTotalExpensesByUserIdAndTeamId(int userId, int teamId)
        {
            decimal totalExpenses = _context.Expenses.Where(u => u.UserId == userId && u.TeamId == teamId).Sum(e => e.Amount);

            return totalExpenses;
        }

        public List<Expense> GetCurrentMonthExpenses(int userId, int month, int year)
        {
             return  _context.Expenses
            .Where(e => e.UserId == userId && e.ExpenseDate.Year == year && e.ExpenseDate.Month == month).ToList();
        }



    }
}
