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
            }catch (NotUserInTeamException e)
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
        /// Obtiene todos los gastos de un grupo específico.
        /// </summary>
        /// <param name="teamId">El ID del grupo del cual se desean consultar los gastos.</param>
        /// <returns>Lista de gastos del grupo.</returns>
        /// <response code="200">Gastos del grupo obtenidos exitosamente.</response>
        /// <response code="400">No se encontraron gastos o el grupo no es válido.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo los usuarios que pertenecen al grupo pueden consultar los gastos.
        /// </remarks>
        [HttpGet("GetAll/{teamId}")]
        [Authorize]
        public ActionResult<List<ExpenseDTO>> GetByTeamId(int teamId)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            List<ExpenseDTO>? expenses = _expenseService.GetByTeamId(userId,teamId);
            if(expenses != null)
            {
                return Ok(expenses);
            }
            return BadRequest();
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
        [HttpDelete("{expenseId}")]
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


        /// <summary>
        /// Obtiene todos los gastos realizados por un usuario en un grupo específico.
        /// </summary>
        /// <param name="teamId">El ID del grupo del cual se desean consultar los gastos.</param>
        /// <returns>Lista de gastos realizados por el usuario en el grupo.</returns>
        /// <response code="200">Gastos obtenidos exitosamente.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Solo el usuario autenticado puede consultar sus propios gastos en el grupo especificado.
        /// </remarks>
        [HttpGet("{teamId}")]
        [Authorize]
        public ActionResult<List<ExpenseDTO>> GetTeamUserExpeses(int teamId)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
            List<ExpenseDTO>? expenses = _expenseService.GetUserExpensesForTeam(userId, teamId);
            if (expenses != null)
            {
                return Ok(expenses);
            }
            return BadRequest();
        }




        /// <summary>
        /// Obtiene el total de gastos de un usuario, ya sea para un equipo específico o para sus gastos individuales.
        /// </summary>
        /// <param name="teamId">El ID del equipo para sumar los gastos del usuario en ese equipo. Si se ingresa 0 o un valor nulo, se devuelve el total de los gastos propios del usuario que no pertenecen a ningún equipo.</param>
        /// <returns>Total de gastos del usuario en el equipo especificado o de sus gastos individuales.</returns>
        /// <response code="200">Total de gastos obtenido exitosamente.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <response code="400">El equipo no existe o el usuario no es miembro del equipo.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. El usuario debe ser miembro del equipo especificado para consultar los gastos del equipo. Si teamId es 0, se devuelven los gastos individuales del usuario que no están asociados a ningún equipo.
        /// </remarks>
        [HttpGet("[action]/{teamId}")]
        [Authorize]
        public ActionResult<decimal> GetTotalExpensesByUser(int? teamId)
        {
            try
            {
                int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");

                decimal totalExpenses = _expenseService.GetTotalExpensesByUser(userId, teamId);

                return Ok(totalExpenses);
            }catch(NotUserInTeamException e)
            {
                return BadRequest(e.Message);
            }
           
        }



        /// <summary>
        /// Obtiene todos los gastos realizados por el usuario autenticado en el mes actual.
        /// </summary>
        /// <returns>Lista de gastos del usuario en el mes actual.</returns>
        /// <response code="200">Gastos del mes actual obtenidos exitosamente.</response>
        /// <response code="401">No autorizado: se requiere un token JWT válido.</response>
        /// <remarks>
        /// Este endpoint requiere autenticación JWT. Devuelve los gastos del usuario autenticado filtrados por el mes y año actuales, basados en la fecha del sistema (UTC).
        /// </remarks>
        [HttpGet("[action]")]
        [Authorize]
        public ActionResult<List<ExpenseDTO>> GetCurrentMonthExpenses()
        {
           int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "");
           var expenses = _expenseService.GetCurrentMonthExpenses(userId);
            return Ok(expenses);
        }
    }   

}
