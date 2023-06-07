using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeTask.Shared.DTO
{
    public class ConfirmResetPasswordRequestDTO
    {
        public string? ConfirmationCode { get; set; }
        public string? NewPassword { get; set; }
    }
}
