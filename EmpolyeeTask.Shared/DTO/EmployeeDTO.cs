using Microsoft.AspNetCore.Http;

namespace EmployeeTask.Shared.DTO
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public int DivisionId { get; set; }
        public string? LastName { get; set; }
        public string? IconPath { get; set; }
        public int ServiceNumber { get; set; }
        public int OneCPass { get; set; }
        public string? Post { get; set; }
        public IFormFile Icon { get; set; } 
    }
}
