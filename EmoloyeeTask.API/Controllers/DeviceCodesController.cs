using EmoloyeeTask.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmoloyeeTask.API.Controllers
{
    public class DeviceCodesController : Controller
    {

        private readonly CachedDeviceCodeService _cachedDeviceCodeService;

        public DeviceCodesController(CachedDeviceCodeService cachedDeviceCodeService)
        {
            _cachedDeviceCodeService = cachedDeviceCodeService;
        }
        /// <summary>
        /// Получение всех подразделений
        /// </summary>
        /// <returns>Список подразделений</returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<AllowedDeviceCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AllowedDeviceCode>>> GetAllowedDeviceCode()
        {
            try
            {
                return Ok(await _cachedDeviceCodeService.GetAllowedDeviceCode());
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
        [ProducesResponseType(typeof(AllowedDeviceCode), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AllowedDeviceCode>> CreateDivision(AllowedDeviceCode deviceCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceCode?.DeviceCode ?? null))
                    return BadRequest("Код устройства пустой");

                await _cachedDeviceCodeService.AddAllowedDeviceCode(deviceCode.DeviceCode);


                return CreatedAtAction("NewDeviceCode", new { id = deviceCode.Id }, deviceCode.DeviceCode);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка добавления данных");
            }
        }
        /// <summary>
        /// Удаление информации о подразделении
        /// </summary>
        /// <param name="id">Уникальнеый ключ</param>
        /// <returns>Код 200 если удаление прошло успешно, код 404 если не найдено подразделение по id, код 500 если случилась ошибка сервера</returns>
        [HttpDelete]
        [Authorize]
        [ProducesResponseType(typeof(Division), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDivision([FromBody]AllowedDeviceCode deviceCode)
        {
            try
            {
                await _cachedDeviceCodeService.RemoveAllowedDeviceCode(deviceCode.DeviceCode);
                return Ok($"Код устройства {deviceCode} бы успешно удалён");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления подразделения");
            }
        }
    }
}
