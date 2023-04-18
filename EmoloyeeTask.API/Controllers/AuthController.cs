using EmoloyeeTask.API.Auth;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.AspNetCore.Http;
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
        /// <param name="employeeModle">Сотрудник</param>
        /// <returns>Декодированный токен ключ</returns>
        [HttpPost("token")]
        public ActionResult Token([FromBody] Employee employeeModle)
        {
            var employee = _employeeRepository.GetAll().Result.FirstOrDefault(x => x.FirstName == employeeModle.FirstName);

            if (employee == null)
                return BadRequest();
            if(employee.Password != employeeModle.Password)
                return BadRequest();

            var token = GetJwt(employee);
            var decode = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return Ok(decode.ToString());

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
                new Claim(ClaimsIdentity.DefaultNameClaimType, employee.FirstName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, employee.Role),
                new Claim(ClaimTypes.NameIdentifier, employee.FirstName)
            };
            return claims;
        }
        
        
    }
}
