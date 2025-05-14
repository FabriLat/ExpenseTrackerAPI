using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.services
{
    public class ExpenseService : IExpenseService
    {

        private readonly IExpenseRepository _expenseRepository;

        public ExpenseService(IExpenseRepository expenseRepository)
        {
            _expenseRepository = expenseRepository;
        }

        public bool AddExpense(CreateExpenseDTO expense, int userId)
        {
            if(expense.Amount <= 0)
            {
                throw new InvalidAmountException();
            }
            Expense newExpense = new Expense();
            newExpense.UserId = userId;
            newExpense.Amount = expense.Amount;
            newExpense.ExpenseName = expense.ExpenseName;
            newExpense.Description = expense.Description;

            if(expense.GroupId == 0 || expense.GroupId == null)
            {
                newExpense.GroupId = null;
            }
            else
            {
                newExpense.GroupId = expense.GroupId;
            }
            newExpense.ExpenseDate = DateOnly.FromDateTime(DateTime.Today);
            _expenseRepository.Add(newExpense);
            return true;
        }

        public List<ExpenseDTO> GetExpenses(int userId)
        {
            List<Expense> expenses = _expenseRepository.GetByUserId(userId);


            if(expenses.Count > 0)
            {
                List<ExpenseDTO> dtos = new List<ExpenseDTO>();
                foreach (var e in expenses)
                {
                   ExpenseDTO dto = ExpenseDTO.Create(e);
                    dtos.Add(dto);
                }
                return dtos;
            }
            return [];
        }

        public List<ExpenseDTO> GetByDate(DateOnly date, int userId)
        {
            var expenses = _expenseRepository.GetByDate(date, userId);

            if (expenses.Count > 0)
            {
                List<ExpenseDTO> dtos = new List<ExpenseDTO>();
                foreach (var e in expenses)
                {
                    ExpenseDTO dto = ExpenseDTO.Create(e);
                    dtos.Add(dto);
                }
                return dtos;
            }
            return [];
        }
    }
}
