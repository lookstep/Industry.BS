using EmoloyeeTask.Data.Interfaces;
using EmployeeTask.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTask.API.Controllers
{

    /// <summary>
    /// Контроллер API и операций CRUD для сотрудников
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IDbRepository<Employee> _employeeRepository;
        /// <summary>
        /// Внедрение зависимости интерфейса для работе с сотдрудником
        /// </summary>
        /// <param name="employeeRepository">зависимость</param>
        public EmployeesController(IDbRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        /// <summary>
        /// Получение всех сотрудников
        /// </summary>
        /// <returns>список сотрудников</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Employee>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            try
            {
                return Ok(await _employeeRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось получить информацию с сервера");
            }

        }
        /// <summary>
        /// Получение конкретного сотрудника по id
        /// </summary>
        /// <param name="id">уникальный ключ</param>
        /// <returns>сотрудника с конкретным id</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.Get(id);
                if (result == null)
                    return NotFound("Не удалось найти сотрудника по данному id");
                else return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось получить информацию с сервера");
            }

        }
        /// <summary>
        /// Добавление сотрудника
        /// </summary>
        /// <param name="employeeDTO">Дто сотрудника</param>
        /// <returns>Создание нового сотрудника и последующая переадресация по нужному URI</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Employee>> AddEmployee([FromForm]EmployeeDto employeeDTO)
        {
            try
            {
                if (employeeDTO == null)
                    return BadRequest("Данные о сотруднике не заполненны пустое или некорректно введены");

                var employee = new Employee()
                {
                    Id = employeeDTO.Id,
                    FirstName = employeeDTO.FirstName,
                    LastName = employeeDTO.LastName,
                    SecondName = employeeDTO.SecondName,
                    ServiceNumber = employeeDTO.ServiceNumber,
                    Email = employeeDTO.Email,
                    OneCPass = employeeDTO.OneCPass,
                    Post = employeeDTO.Post,
                    Password = employeeDTO.Password,
                    Role = employeeDTO.Role,
                    DivisionId = employeeDTO.DivisionId
                };

                var hasher = new PasswordHasher<EmployeeDto>();

                employee.Password = hasher.HashPassword(employeeDTO, employeeDTO.Password);

                var createEmployee = await _employeeRepository.AddWithFile(employee, employeeDTO.File);
                return CreatedAtAction(nameof(GetEmployee), new { id = createEmployee.Id }, createEmployee);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось добавить нового отрудника");
            }
        }
        /// <summary>
        /// Обновление данных о сотруднике
        /// </summary>
        /// <param name="id">Уникальный ключ (берётся из URI)</param>
        /// <param name="employee">Видоизменённые данные о сотруднике</param>
        /// <returns>Обновленные данные конкретного сотрудника</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employee)
        {
            try
            {
                if (employee.Id != id)
                    return BadRequest("id у работника и зпроса различны");

                var result = await _employeeRepository.Get(id);
                if (result == null)
                    return NotFound($"Не удалось найти сотрудника по id: {id}");
                return Ok(await _employeeRepository.Update(employee));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка обновления данных сотрудника");
            }
        }
        /// <summary>
        /// Удаление сотрудника по уникальному клчючу 
        /// </summary>
        /// <param name="id">уникальный ключ</param>
        /// <returns>Статус http 200 если сотрудник найден и удалён, 404 если сотрудника с нужным id нет, 500 - ошибка сервера при удалении сотрудника</returns>
        [HttpDelete("{id:int}")]
        [Authorize]
        [ProducesResponseType(typeof(Employee), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var result = await _employeeRepository.Get(id);
                if (result == null)
                    return NotFound($"Не удалось найти сотрудника по id: {id}");

                await _employeeRepository.Delete(id);
                return Ok($"Сотрудник {id} бы успешно удалён");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления сотрудника");
            }
        }

    }
}
