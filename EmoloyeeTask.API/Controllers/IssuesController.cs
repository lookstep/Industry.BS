using EmoloyeeTask.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTask.API.Controllers
{
    /// <summary>
    /// Контроллер задач для работников
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly IDbRepository<Issue> _taskForEmployeeRepository;
        /// <summary>
        /// Внедрение зависимости интерфейса репазитория
        /// </summary>
        /// <param name="taskForEmployeeRepository">интерфейс репазитория</param>
        public IssuesController(IDbRepository<Issue> taskForEmployeeRepository)
        {
            _taskForEmployeeRepository = taskForEmployeeRepository;
        }
        /// <summary>
        /// Получение всех задач для работников
        /// </summary>
        /// <returns>Список пзадач для работников</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Issue>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Issue>>> GetTasksForEmployees()
        {
            try
            {
                return Ok(await _taskForEmployeeRepository.GetAll());
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Получение конкретной задач для работника по id
        /// </summary>
        /// <param name="id">уникальный ключ</param>
        /// <returns>конкретная задача</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Issue), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Issue>> GetTaskForEmployeeById(int id)
        {
            try
            {
                var result = await _taskForEmployeeRepository.Get(id);

                if(result == null)
                    return NotFound("Задачи с данным id нет");

                return Ok(result);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Создание новой задачи
        /// </summary>
        /// <param name="taskForEmployee">Новая задача</param>
        /// <returns>Созданное задание</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Issue), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Issue>> CreateTasksForEmployees(Issue taskForEmployee)
        {
            try
            {
                if (taskForEmployee == null)
                    return BadRequest("Данные о задаче пустые или некорректно введены");

                var createdTask = await _taskForEmployeeRepository.Add(taskForEmployee);
                    return CreatedAtAction(nameof(GetTaskForEmployeeById), new { id = createdTask.Id }, createdTask);
            }
            catch 
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка создания задачи");
            }
        }
        /// <summary>
        /// Обновление задачи
        /// </summary>
        /// <param name="id">Уникальный ключ</param>
        /// <param name="taskForEmployee">Задача, данные о которой мы хотим изменить</param>
        /// <returns>Обновленные данные задачи</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Issue), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Issue>> UpdateTasksForEmployee(int id, Issue taskForEmployee)
        {
            try
            {
                if (id != taskForEmployee.Id)
                    return BadRequest("id у задачи и зпроса различны");

                var result = await _taskForEmployeeRepository.Get(id);

                if (result == null)
                    return NotFound("Задачи с данным id нет");

                return Ok(await _taskForEmployeeRepository.Update(taskForEmployee));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка обновления задачи");
            }
        }
        /// <summary>
        /// Удаление задач
        /// </summary>
        /// <param name="id">Уникальнеый ключ</param>
        /// <returns>Код 200 если удаление прошло успешно, код 404 если не найдена задача по id, код 500 если случилась ошибка сервера</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Division), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTasksForEmployee(int id)
        {
            try
            {
                var result = await _taskForEmployeeRepository.Get(id);
                if (result == null)
                    return NotFound("Задачи с данным id нет");

                await _taskForEmployeeRepository.Delete(id);

                return Ok($"Задача с id {id} была успешно удалена");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления задачи");
            }
        }
    }
}
