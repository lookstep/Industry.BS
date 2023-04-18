using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Data.Repositories
{
    public class EmployeeRepository : DbRepository<Employee>
    {
        private readonly AppDbContext _db;
        public EmployeeRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public override async Task<Employee> Add(Employee employee)
        {

            if (employee.Division != null)
            {
                _db.Entry(employee.Division).State = EntityState.Unchanged;
            }
            var result = await _db.Employees
                                  .AddAsync(employee);

            await _db.SaveChangesAsync();

            return result.Entity;
        }
        public override async Task Delete(int id)
        {
            var result = await _db.Employees
                                  .FirstOrDefaultAsync(x => x.Id == id);
            if(result != null)
            {
                _db.Remove(result);
                await _db.SaveChangesAsync();
            }
        }
        public override async Task<Employee> Get(int id)
        {
            return await _db.Employees
                            .Include(x => x.Division)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }
        public override async Task<IEnumerable<Employee>> GetAll()
        {
            return await _db.Employees
                            .Include(x => x.Division)
                            .ToListAsync();
        }
        public override async Task<Employee> Update(Employee employee)
        {
            var result = await Get(employee.Id);
            if(result != null)
            {
                result.FirstName = employee.FirstName;
                result.SecondName = employee.SecondName;
                result.LastName = employee.LastName;
                result.Role = employee.Role;
                result.Password = employee.Password;
                result.OneCPass = employee.OneCPass;
                result.ServiceNumber = employee.ServiceNumber;
                result.Post = employee.Post;
                result.DivisionId = employee.DivisionId;
                result.LaborCosts = employee.LaborCosts;

                await _db.SaveChangesAsync();

                return result;
            }
            return null!;
        }
    }
}
