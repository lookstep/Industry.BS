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
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IDbRepository<Employee> _employeeRepository;
        /// <summary>
        /// Внедрение зависимости интерфейса для работе с сотдрудником
        /// </summary>
        /// <param name="employeeRepository">зависимость</param>
        public AuthController(IDbRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        /// <summary>
        /// Отправка JWT токена
        /// </summary>
        /// <param name="employeeModel">Сотрудник</param>
        /// <returns>Декодированный токен ключ</returns>
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
        
        
    }
}
