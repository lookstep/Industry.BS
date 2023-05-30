using EmoloyeeTask.Data.Interfaces;
using EmployeeTask.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTask.API.Controllers
{
    /// <summary>
    /// Контроллер API и операций CRUD для данных о трудозатратах
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class LaborCostsController : ControllerBase
    {
        private readonly IDbRepository<LaborCost> _laborCostsRepository;
        /// <summary>
        /// Внедрение зависимости интерфейса для работы с данными о трудозатратах
        /// </summary>
        /// <param name="laborCostsRepository">зависимость</param>
        public LaborCostsController(IDbRepository<LaborCost> laborCostsRepository)
        {
            _laborCostsRepository = laborCostsRepository;
        }
        /// <summary>
        /// Получение всех данных о трудозатратах
        /// </summary>
        /// <returns>список данных о трудозатратах</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LaborCost>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<LaborCost>>> GetLaborCosts()
        {
            try
            {
                return Ok(await _laborCostsRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Получение конкретных данных о трудозатратах по id
        /// </summary>
        /// <param name="id">уникальный ключ</param>
        /// <returns>сотрудника с данными о трудозатратах по id</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(LaborCost), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LaborCost>> GetLaborCostById(int id)
        {
            try
            {
                var result = await _laborCostsRepository.Get(id);
                if (result == null)
                    return NotFound("Не удалось найти трудозатраты по данному id");
                return Ok(result);

            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Добавление данных о трудозатратах
        /// </summary>
        /// <param name="laborCosts">Новые данные о трудозатратах</param>
        /// <returns>Создание новых данных о трудозатратах и последующая переадресация по нужному URI</returns>
        [HttpPost]
        [ProducesResponseType(typeof(LaborCost), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LaborCost>> CreateLaborCosts(LaborCost laborCosts)
        {
            try
            {
                if (laborCosts == null)
                    return BadRequest("Данные о трудозатратах пусты или некорректно добавлены");

                var createdLaborCosts = await _laborCostsRepository.Add(laborCosts);
                if (createdLaborCosts == null)
                    return NotFound("Ошибка");
                return CreatedAtAction(nameof(GetLaborCostById), new { id = createdLaborCosts.Id }, createdLaborCosts);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка создания данных о трудозатратах");
            }
        }
        /// <summary>
        /// Обновление данных о трудозатратах
        /// </summary>
        /// <param name="id">Уникальный ключ (берётся из URI)</param>
        /// <param name="laborCosts">Видоизменённые данные о трудозатратах</param>
        /// <returns>Обновленные данные о трудозатратах</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult<LaborCost>> UpdateLaborCosts(int id, LaborCost laborCosts)
        {
            try
            {
                if (id != laborCosts.Id)
                    return BadRequest("id у трудозатрат и зпроса различны");

                var result = await _laborCostsRepository.Get(id);
                if (result == null)
                    return NotFound("Не удалось найти трудозатраты по данному id");

                result.Date = laborCosts.Date;
                result.HourCount = laborCosts.HourCount;

                var saveManager = _laborCostsRepository as LaborCostRepository;

                await saveManager.Save();

                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка обновления данных о трудозатратах");
            }
        }
        /// <summary>
        /// Удаление данных о трудозатратах по уникальному клчючу 
        /// </summary>
        /// <param name="id">уникальный ключ</param>
        /// <returns>Статус http 200 если данные о трудозатратах найден и удалены, 
        /// 404 если данных о трудозатратах с нужным id нет, 
        /// 500 - ошибка сервера при удалении данных о трудозатратах</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(LaborCost), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LaborCost>> DeleteLaborCosts(int id)
        {
            try
            {
                var result = await _laborCostsRepository.Get(id);
                if (result == null)
                    return NotFound($"Не удалось найти трудозатраты по id: {id}");

                await _laborCostsRepository.Delete(id);

                return Ok($"Данные о трудозатратах с id {id} были удалены");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления данных о трудозатратах");
            }

        }

    }
}
