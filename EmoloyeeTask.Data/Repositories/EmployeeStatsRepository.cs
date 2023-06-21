using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmoloyeeTask.Data.Repositories
{
    public class EmployeeStatsRepository : IEmployeeStatsRepository
    {
        private readonly AppDbContext _db;

        public EmployeeStatsRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<float>> GetWeeklyStats(int employeeId)
        {
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
            var weekEnd = weekStart.AddDays(5);
            var stats = await _db.Set<LaborCost>()
                .Where(lc => lc.EmployeeId == employeeId && lc.Date >= weekStart && lc.Date < weekEnd)
                .GroupBy(lc => lc.Date.DayOfWeek)
                .Select(g => new { DayOfWeek = g.Key, Hours = g.Sum(x => x.HourCount) })
                .ToListAsync();

            var weekStats = Enumerable.Repeat(0f, 5).ToList();
            foreach (var dayStat in stats)
                weekStats[(int)dayStat.DayOfWeek - 1] = dayStat.Hours;

            return weekStats;
        }

        public async Task<List<float>> GetMonthlyStats(int employeeId)
        {
            var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var monthEnd = monthStart.AddMonths(1);
            var stats = await _db.Set<LaborCost>()
                .Where(lc => lc.EmployeeId == employeeId && lc.Date >= monthStart && lc.Date < monthEnd)
                .GroupBy(lc => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(lc.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => new { WeekOfYear = g.Key, Hours = g.Sum(x => x.HourCount) })
                .ToListAsync();

            var monthStats = Enumerable.Repeat(0f, 5).ToList();
            foreach (var weekStat in stats)
                monthStats[weekStat.WeekOfYear - 1] = weekStat.Hours;

            return monthStats;
        }

        public async Task<List<float>> GetYearlyStats(int employeeId)
        {
            var yearStart = new DateTime(DateTime.Today.Year, 1, 1);
            var yearEnd = yearStart.AddYears(1);
            var stats = await _db.Set<LaborCost>()
                .Where(lc => lc.EmployeeId == employeeId && lc.Date >= yearStart && lc.Date < yearEnd)
                .GroupBy(lc => lc.Date.Month)
                .Select(g => new { Month = g.Key, Hours = g.Sum(x => x.HourCount) })
                .ToListAsync();

            var yearStats = Enumerable.Repeat(0f, 12).ToList();
            foreach (var monthStat in stats)
                yearStats[monthStat.Month - 1] = monthStat.Hours;

            return yearStats;
        }
    }
}
