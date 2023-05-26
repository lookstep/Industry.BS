using EmoloyeeTask.API.Auth;
using EmoloyeeTask.Data.Interfaces;
using EmployeeTask.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTask.API.Controllers
{
    /// <summary>
    /// Контроллер подразделений
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DivisionsController : ControllerBase
    {
        private readonly IDbRepository<Division> _divisionRepository;
        /// <summary>
        /// Внедрение зависимости интерфейса репазитория
        /// </summary>
        /// <param name="divisionRepository">интерфейс репазитория</param>
        public DivisionsController(IDbRepository<Division> divisionRepository)
        {
            _divisionRepository = divisionRepository;
        }
        /// <summary>
        /// Получение всех подразделений
        /// </summary>
        /// <returns>Список подразделений</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Division>>> Divisions()
        {
            try
            {
                return Ok(await _divisionRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Получение конкретного подразделение по id
        /// </summary>
        /// <param name="id">уникальный ключ</param>
        /// <returns>конкретное подразделение</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Division), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Division>> GetDivision(int id)
        {
            try
            {
                var result = await _divisionRepository.Get(id);

                if (result == null)
                    return NotFound("Не удалось найти подразделение по данному id");
                else
                    return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }

        }
        /// <summary>
        /// Создание нового подразделение
        /// </summary>
        /// <param name="division">Новое подразделение</param>
        /// <returns>Созданное подразделение</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Division), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Division>> CreateDivision(Division division)
        {
            try
            {
                if (division == null)
                    return BadRequest("Подразделение пустое или некорректно добавлены данные");

                var createdDivision = await _divisionRepository.Add(division);

                return CreatedAtAction(nameof(GetDivision), new { id = createdDivision.Id }, createdDivision);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка добавления данных");
            }
        }
        /// <summary>
        /// Обновление подразделение
        /// </summary>
        /// <param name="id">Уникальный ключ</param>
        /// <param name="division">Подразделение, данные о котором мы хотим изменить</param>
        /// <returns>Обновленное подразделение</returns>
        [HttpPut("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(Division), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Division>> UpdateDivision(int id, Division division)
        {
            try
            {
                if (division.Id != id)
                    return BadRequest("id у подразделения и зпроса различны");

                var result = await _divisionRepository.Get(id);
                if (result == null)
                    return NotFound($"Не удалось найти подразделение по id: {id}");
                return Ok(await _divisionRepository.Update(division));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка обновления данных");
            }        
        }
        /// <summary>
        /// Удаление информации о подразделении
        /// </summary>
        /// <param name="id">Уникальнеый ключ</param>
        /// <returns>Код 200 если удаление прошло успешно, код 404 если не найдено подразделение по id, код 500 если случилась ошибка сервера</returns>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(Division), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDivision(int id)
        {
            try
            {
                var result = await _divisionRepository.Get(id);
                if (result == null)
                    return NotFound($"Не удалось найти подразделение по id: {id}");

                await _divisionRepository.Delete(id);
                return Ok($"Отдел {id} бы успешно удалён");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления подразделения");
            }
        }
    }
}
