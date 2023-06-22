using EmoloyeeTask.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmoloyeeTask.API.Controllers
{
    public class EmployeeStatsController : Controller
    {
        private readonly IEmployeeStatsRepository _statsRepo;

        public EmployeeStatsController(IEmployeeStatsRepository statsRepo)
        {
            _statsRepo = statsRepo;
        }

        [HttpGet("weekly/{employeeId}")]
        public async Task<ActionResult<List<float>>> GetWeeklyStats(int employeeId)
        {
            try
            {
                var stats = _statsRepo.GetWeeklyStats(employeeId);
                return Ok(stats);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {e.Message}");
            }
        }

        [HttpGet("monthly/{employeeId}")]
        public async Task<ActionResult<IEnumerable<float>>> GetMonthlyStats(int employeeId)
        {
            try
            {
                var stats = _statsRepo.GetMonthlyStats(employeeId);
                return Ok(stats);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {e.Message}");
            }
        }

        [HttpGet("yearly/{employeeId}")]
        public async Task<ActionResult<List<float>>> GetYearlyStats(int employeeId)
        {
            try
            {
                var stats = await _statsRepo.GetYearlyStats(employeeId);
                return Ok(stats);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving data from the database: {e.Message}");
            }
        }


    }
}
