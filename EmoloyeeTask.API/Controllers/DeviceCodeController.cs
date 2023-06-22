using Industry.BS.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Industry.BS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceCodeController : ControllerBase
    {
        private readonly IDeviceCodeRepository _deviceCodeRepository;

        public DeviceCodeController(IDeviceCodeRepository deviceCodeRepository)
        {
            _deviceCodeRepository = deviceCodeRepository;
        }

        [HttpGet("GetAllowedDeviceCode")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<AllowedDeviceCode>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<AllowedDeviceCode>>> GetAllowedDeviceCode()
        {
            try
            {
                return Ok(await _deviceCodeRepository.GetAllAllowedDeviceCodes());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка получения данных с сервера");
            }
        }

        [HttpPost("CreateDeviceCode")]
        [ProducesResponseType(typeof(AllowedDeviceCode), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AllowedDeviceCode>> CreateDeviceCode(AllowedDeviceCode deviceCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(deviceCode.DeviceCode))
                    return BadRequest("Код устройства пустой");

                await _deviceCodeRepository.AddAllowedDeviceCode(deviceCode.DeviceCode);


                return Ok(deviceCode.DeviceCode);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка добавления данных");
            }
        }

        [HttpDelete("DeleteDeviceCode")]
        [Authorize]
        [ProducesResponseType(typeof(AllowedDeviceCode), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteDeviceCode([FromBody] AllowedDeviceCode deviceCode)
        {
            try
            {
                await _deviceCodeRepository.RemoveAllowedDeviceCode(deviceCode.DeviceCode);
                return Ok($"Код устройства {deviceCode} бы успешно удалён");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ошибка удаления подразделения");
            }
        }
    }
}
