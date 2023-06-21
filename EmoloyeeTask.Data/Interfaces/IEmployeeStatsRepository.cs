using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Interfaces
{
    public interface IEmployeeStatsRepository
    {
        Task<List<float>> GetWeeklyStats(int employeeId);
        Task<List<float>> GetMonthlyStats(int employeeId);
        Task<List<float>> GetYearlyStats(int employeeId);
    }
}
