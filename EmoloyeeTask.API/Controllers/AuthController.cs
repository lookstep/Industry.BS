using EmoloyeeTask.API.Auth;
using EmoloyeeTask.Data.Interfaces;
using EmployeeTask.Shared.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace EmoloyeeTask.API.Controllers
{
    /// <summary>
    /// Аутентификация
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDbRepository<Employee> _employeeRepository;
        private readonly IMailSender _mailSender;

        /// <summary>
        /// Конструктро для заполнения сервисов
        /// </summary>
        /// <param name="employeeRepository">сотрудники</param>
        /// <param name="mailSender">почта</param>
        public AuthController(IDbRepository<Employee> employeeRepository, IMailSender mailSender)
        {
            _employeeRepository = employeeRepository;
            _mailSender = mailSender;
        }

        /// <summary>
        /// Получение токена
        /// </summary>
        /// <param name="employeeModel">Дто сотруднка</param>
        /// <returns>jwt токен</returns>
        [HttpPost("token")]
        public ActionResult Token([FromBody] EmployeeDto employeeModel)
        {
            try
            {
                var employee = _employeeRepository.GetAll().Result.FirstOrDefault(x => x.Email == employeeModel.Email);

                if (employee == null)
                    return BadRequest("Пользователь не найден");

                var hasher = new PasswordHasher<Employee>();

                var verifyResult = hasher.VerifyHashedPassword(employee, employee.Password, employeeModel.Password);

                if (verifyResult == PasswordVerificationResult.Failed)
                    return BadRequest("Неверный пароль");

                var token = GetJwt(employee);
                var decode = new JwtSecurityTokenHandler().ReadJwtToken(token);
                return Ok(token);
            }
            catch
            {
                return BadRequest("Ну удалось авторизироваться (неправельный логин или пароль)");
            }
        }

        /// <summary>
        /// Отправка кода для востановления пароля
        /// </summary>
        /// <param name="request">Емайл пользователя</param>
        /// <returns>Отправленное по почте письмо</returns>
        [HttpPost]
        [Route("/reset-password")]
        public async Task<ActionResult> ResetPassword([FromBody]ResetPasswordRequestDTO request)
        {
            try
            {
                var employee = _employeeRepository.GetAll().Result.FirstOrDefault(x => x.Email == request.Email);
                if (employee == null)
                    return BadRequest("Пользователь с таким адресом электронной почты не найден");

                var confirmationCode = GenerateConfirmationCode();

                employee.ConfirmationCode = confirmationCode;
                await _employeeRepository.Update(employee);

                await _mailSender.SendMailMessageAsync(employee.Email, confirmationCode);

                return Ok("На вашу почту отправлено сообщение");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось отправить письмо на электронную почту. Проверьте адресс");
            }
        }

        /// <summary>
        /// Востановление пароля
        /// </summary>
        /// <param name="request">Код подтверждения и пароль</param>
        /// <returns>Измененный пароль</returns>
        [HttpPost]
        [Route("/confirm-reset-password")]
        public async Task<ActionResult> ConfirmResetPassword([FromBody]ConfirmResetPasswordRequestDTO request)
        {
            try
            {
                var employee = _employeeRepository.GetAll().Result.FirstOrDefault(x => x.ConfirmationCode == request.ConfirmationCode);
                if (employee == null)
                    return BadRequest("Неверный код подтверждения");

                var hasher = new PasswordHasher<Employee>();
                var hashedPassword = hasher.HashPassword(employee, request.NewPassword);

                employee.Password = hashedPassword;
                employee.ConfirmationCode = null;
                await _employeeRepository.Update(employee);

                return Ok("Ваш пароль был изменён.");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Не удалось обновить пароль.");
            }
        }

        private string GetJwt(Employee employee)
        {
            var nowDate = DateTime.Now;

            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.Issuer,
                audience: AuthOptions.Audience,
                notBefore: nowDate,
                claims: GetClaims(employee),
                expires: nowDate.AddMinutes(AuthOptions.TTL),
                signingCredentials: new SigningCredentials(AuthOptions.PrivateKey, SecurityAlgorithms.RsaSha256Signature)
                );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return tokenString;
        }

        private IEnumerable<Claim> GetClaims(Employee employee)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString())
            };
            return claims;
        }
        private string GenerateConfirmationCode()
        {
            var random = new Random();
            var code = random.Next(100000, 999999).ToString();
            return code;
        }

    }
}
