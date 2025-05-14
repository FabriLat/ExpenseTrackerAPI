using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;

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


    }
}
