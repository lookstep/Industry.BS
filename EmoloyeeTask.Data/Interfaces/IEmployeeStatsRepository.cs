﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Interfaces
{
    public interface IEmployeeStatsRepository
    {
        public List<float> GetWeeklyStats(int employeeId);
        public IEnumerable<float> GetMonthlyStats(int employeeId);
        public Task<List<float>> GetYearlyStats(int employeeId);
    }
}
