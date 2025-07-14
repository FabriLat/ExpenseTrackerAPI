using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.services
{
    public class ExpenseService : IExpenseService
    {

        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserService _userService;
        private readonly ITeamService _teamService;

        public ExpenseService(IExpenseRepository expenseRepository, IUserService userService, ITeamService teamRepository)
        {
            _expenseRepository = expenseRepository;
            _userService = userService;
            _teamService = teamRepository;
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
            newExpense.ExpenseName = ((CategoriesEnum)expense.CategoryId-1).ToString();
            newExpense.Description = expense.ExpenseDescription;
            newExpense.CategoryId = expense.CategoryId;

            if (expense.TeamId <= 0)
            {
                newExpense.TeamId = null;
            }
            else if (expense.TeamId > 0)
            {
                
                User? user = _userService.GetByIdCompleteData(userId);
                Team? team = _teamService.GetTeamAndUsers(expense.TeamId);
                if (user == null || team == null)
                    { return false; }

                if (team.Users.Contains(user))
                {
                    newExpense.TeamId = expense.TeamId;
                }
                else
                {
                    throw new NotUserInTeamException();
                }
            }
            newExpense.ExpenseDate = DateOnly.FromDateTime(DateTime.Today);
            _expenseRepository.Add(newExpense);
            return true;
        }



        public List<ExpenseDTO>? GetByTeamId(int userId, int teamId)
        {
            List<Expense> expensesFullData = _expenseRepository.GetByTeamId(teamId);
            User? user = _userService.GetByIdCompleteData(userId);

            if (user != null)
            {
                Team? team = _teamService.GetTeamAndUsers(teamId);

                if (team != null && team.Users.Contains(user))
                {
                    List<ExpenseDTO> dtos = new List<ExpenseDTO>();
                    foreach (var expense in expensesFullData)
                    {
                        ExpenseDTO dto = ExpenseDTO.Create(expense);
                        dtos.Add(dto);
                    }
                    return dtos;
                }
                throw new NotUserInTeamException();
            }  
            return null;
        }


        public decimal GetTotalLastMonthExpenses(int userId)
        {
            List<ExpenseDTO> expenses = GetCurrentMonthExpenses(userId);

            decimal total = expenses.Sum(e => e.Amount);
            return total;
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


        public List<ExpenseDTO>? GetUserExpensesForTeam(int userId, int teamId)
        {
            List<Expense> expenses = _expenseRepository.GetUserExpensesForTeam(userId, teamId);
            User? user = _userService.GetByIdCompleteData(userId);
            Team? team = _teamService.GetTeamAndUsers(teamId);

            if (team == null || user == null || !team.Users.Contains(user))
                return null;

            if (expenses.Count() > 0)
            {
                List<ExpenseDTO> expensesDto = new List<ExpenseDTO>();
                foreach (var e in expenses)
                {
                    var dto = ExpenseDTO.Create(e);
                    expensesDto.Add(dto);
                }
                return expensesDto;
            }
            return [];
        }

        public bool DeleteExpense(int expenseId, int userId)
        {
            var expense = _expenseRepository.GetById(expenseId);
            if (expense != null)
            {
                if (expense.UserId == userId)
                {
                    _expenseRepository.Delete(expense);
                    return true;
                }
            }
            return false;
        }


        public decimal GetTotalExpensesByUser(int userId, int? teamId)
        {
            if(teamId != null && teamId > 0)
            {

                var team = _teamService.GetTeamAndUsers(teamId.Value);

                var user = _userService.GetByIdCompleteData(userId);

                if(user != null && team != null)
                {
                    if (team.Users.Contains(user))
                    {
                        decimal totalExpenseTeam = _expenseRepository.GetTotalExpensesByUserIdAndTeamId(userId, teamId.Value);
                        return totalExpenseTeam;
                    }
                    else
                    {
                        throw new NotUserInTeamException();
                    }
                } 
            }
            decimal totalExpense = _expenseRepository.GetTotalExpensesByUserId(userId);
            return totalExpense;
        }

        public List<ExpenseDTO> GetCurrentMonthExpenses(int userId)
        {
            int currentMonth = DateTime.UtcNow.Month;
            int currentYear = DateTime.UtcNow.Year;

            var expenses =  _expenseRepository.GetCurrentMonthExpenses(userId, currentMonth, currentYear);

            List<ExpenseDTO> expensesDtos = new List<ExpenseDTO>();

            foreach (var e in expenses)
            {
                ExpenseDTO dto = ExpenseDTO.Create(e);
                expensesDtos.Add(dto);
            }
            return expensesDtos;

        }
    }
}
