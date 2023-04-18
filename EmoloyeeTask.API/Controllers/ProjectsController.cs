using EmoloyeeTask.Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeTask.API.Controllers
{
    /// <summary>
    /// Контроллер проектов
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IDbRepository<Project> _projectRepository;
        /// <summary>
        /// Внедрение зависимости
        /// </summary>
        /// <param name="projectRepository">зависимость projectRepository</param>
        public ProjectsController(IDbRepository<Project> projectRepository)
        {
            _projectRepository = projectRepository;
        }
        /// <summary>
        /// Получение всех проектов
        /// </summary>
        /// <returns>Список пректов</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            try
            {
                return Ok(await _projectRepository.GetAll());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Получение конкретного проекта по id
        /// </summary>
        /// <param name="id">Уникальный ключ</param>
        /// <returns>конкретный проект</returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Project>> GetProjectById(int id)
        {
            try
            {
                var result = await _projectRepository.Get(id);

                if (result == null)
                    return NotFound("Не удалось найти проект по данному id");

                return Ok(result);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }
        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="project">проект</param>
        /// <returns>созданный проект</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Project), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            try
            {
                if (project == null)
                    return BadRequest("Даннные проекта пусты или некорректно введены");

                var createdProject = await _projectRepository.Add(project);

                return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, createdProject);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка создания нового проекта");
            }
        }
        /// <summary>
        /// Обновление данных о проекте
        /// </summary>
        /// <param name="id">Уникальный ключ из URI</param>
        /// <param name="project">Видоизменённые данные о проект</param>
        /// <returns>Видоизменённый проект</returns>
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Project>> UpdateProject(int id, Project project)
        {
            try
            {
                if (id != project.Id)
                    return BadRequest("id у проекта и зпроса различны");

                var result = await _projectRepository.Get(id);

                if (result == null)
                    return NotFound($"Не удалось найти подразделение по id: {id}");

                return Ok(await _projectRepository.Update(project));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка изменения данных проекта");
            }
        }
        /// <summary>
        /// Удаление информации о проекте
        /// </summary>
        /// <param name="id">Уникальнеый ключ</param>
        /// <returns>Код 200 если удаление прошло успешно, код 404 если не найден проект по id, код 500 если случилась ошибка сервера</returns>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(typeof(Project), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteProject(int id)
        {
            try
            {
                var result = await _projectRepository.Get(id);

                if (result == null)
                    return NotFound($"Не удалось найти проект по id: {id}");

                else
                    await _projectRepository.Delete(id);

                return Ok($"Проект по id {id} был успешно удалён");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления данных проекта");
            }

        }
    }
}
