using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EmoloyeeTask.Data.Repositories
{
    public class EmployeeStatsRepository : IEmployeeStatsRepository
    {
        private readonly AppDbContext _db;

        public EmployeeStatsRepository(AppDbContext db)
        {
            _db = db;
        }

        public List<float> GetWeeklyStats(int employeeId)
        {
            var weekStart = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);
            var weekEnd = weekStart.AddDays(7);

            var weekStats = _db.LaborCosts
                .Where(l => l.EmployeeId == employeeId && l.Date >= weekStart && l.Date < weekEnd)
                .AsEnumerable() 
                .GroupBy(l => l.Date.DayOfWeek)
                .Select(g => g.Sum(l => l.HourCount));

            return weekStats.ToList();
        }

        // Статистика за месяц
        public IEnumerable<float> GetMonthlyStats(int employeeId)
        {
            var monthStart = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            var monthEnd = monthStart.AddMonths(1);

            var monthStats = _db.LaborCosts
                .Where(l => l.EmployeeId == employeeId && l.Date >= monthStart && l.Date < monthEnd)
                .AsEnumerable() // move subsequent operations to client side
                .GroupBy(l => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(l.Date, CalendarWeekRule.FirstDay, DayOfWeek.Monday))
                .Select(g => g.Sum(l => l.HourCount));

            return monthStats.AsEnumerable();
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
