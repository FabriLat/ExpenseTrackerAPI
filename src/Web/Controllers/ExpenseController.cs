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


        /// <summary>
        /// Crea un nuevo gasto.
        /// </summary>
        /// <param name="newExpense">Datos del nuevo gasto.</param>
        /// <returns>Respuesta exitosa si el gasto se crea correctamente.</returns>
        /// <response code="200">Gasto creado exitosamente.</response>
        /// <response code="400">Datos inválidos o monto no válido para el gasto.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT.
        /// </remarks>
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
                
                bool created = _expenseService.AddExpense(newExpense, userId);
                if (created == true)
                {
                    return Ok();
                }
                return StatusCode(400);
            } catch (InvalidAmountException e)
            {
                return BadRequest(e.Message);
            }

        }


        /// <summary>
        /// Obtiene todos los gastos de un usuario autenticado.
        /// </summary>
        /// <returns>Una lista de gastos del usuario.</returns>
        /// <response code="200">Devuelve la lista de gastos.</response>
        /// <response code="400">Error al obtener los gastos o ID de usuario inválido.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT.
        /// </remarks>
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


        /// <summary>
        /// Obtiene los gastos de un usuario autenticado para una fecha específica.
        /// </summary>
        /// <param name="date">Fecha en formato MM-dd-yyyy (ej. 05-19-2025).</param>
        /// <returns>Una lista de gastos para la fecha especificada.</returns>
        /// <response code="200">Devuelve la lista de gastos.</response>
        /// <response code="400">Formato de fecha inválido, ID de usuario inválido o error al obtener los gastos.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT.
        /// La fecha debe estar en formato MM-dd-yyyy.
        /// </remarks>
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




        /// <summary>
        /// Elimina un gasto por su ID.
        /// </summary>
        /// <param name="expenseId">El ID del gasto a eliminar.</param>
        /// <returns>Ok si se elimina correctamente.</returns>
        /// <response code="200">Gasto eliminado exitosamente.</response>
        /// <response code="400">Error al eliminar el gasto.</response>
         /// <remarks>
        /// Este endpoint requiere autenticación JWT.
        /// </remarks>
        [HttpDelete]
        [Authorize]
        public ActionResult Delete(int expenseId)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            bool deleted = _expenseService.DeleteExpense(expenseId, userId);

            if (deleted)
            {
                return Ok();
            }
            return BadRequest();

        }

    }

}
