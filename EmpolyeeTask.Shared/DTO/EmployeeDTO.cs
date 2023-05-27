using Microsoft.AspNetCore.Http;

namespace EmployeeTask.Shared.DTO
{
#nullable enable
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Нерпавельный email адресс")]
        public string? Email { get; set; }
        public int DivisionId { get; set; }
        public string? LastName { get; set; }
        public IFormFile? File { get; set; } 
        public int ServiceNumber { get; set; }
        public int OneCPass { get; set; }
        public string? Post { get; set; }
    }
}
