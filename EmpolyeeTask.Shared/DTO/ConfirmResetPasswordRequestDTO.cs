namespace EmployeeTask.Shared.DTO
{
    public class ConfirmResetPasswordRequestDTO
    {
        public string? ConfirmationCode { get; set; }
        public string? NewPassword { get; set; }
    }
}
