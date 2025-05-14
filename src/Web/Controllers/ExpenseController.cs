using System.Security.Claims;
using Application.dto.request;
using Application.dto.response;
using Application.interfaces;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    [Route("api/[controller]s")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService _expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            _expenseService = expenseService;
        }


        [HttpPost]
        [Authorize]
        public ActionResult AddExpense(CreateExpenseDTO newExpense)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                _expenseService.AddExpense(newExpense, userId);
                return Ok();
            } catch (InvalidAmountException e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<ExpenseDTO>> Get()
        {
            try
            {
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
                var userExpenses = _expenseService.GetExpenses(userId);
                return Ok(userExpenses);
            } catch (Exception e)
            { return BadRequest(e.Message); }

        }


        [HttpGet("Date")]
        [Authorize]
        public ActionResult<List<ExpenseDTO>> GetByDate([FromQuery] string date)
        {
            if (!DateOnly.TryParseExact(date, "MM-dd-yyyy", out DateOnly parsedDate))
            {
                return BadRequest("Formato de fecha inválido. Use MM-dd-yyyy.");
            }
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            var expenses = _expenseService.GetByDate(parsedDate, userId);
            return Ok(expenses);
        }
    } 
}
