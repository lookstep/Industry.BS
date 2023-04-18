using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Data.Repositories
{
    public class DivisionRepository : DbRepository<Division>
    {
        private readonly AppDbContext _db;
        public  DivisionRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public override async Task<Division> Add(Division division)
        {
            var result = await _db.Divisions
                                  .AddAsync(division);          
            await _db.SaveChangesAsync();        

            return result.Entity;
        }

        public override async Task Delete(int id)
        {
            var result = await _db.Divisions
                                  .FirstOrDefaultAsync(x => x.Id == id);
            if(result != null)
            {
                _db.Divisions.Remove(result);

                await _db.SaveChangesAsync();
            }
        }

        public override async Task<Division> Get(int id)
        {
             return await _db.Divisions
                             .Include(x => x.Employees)
                             .FirstOrDefaultAsync(x => x.Id == id)!;
        }

        public override async Task<IEnumerable<Division>> GetAll()
        {
            return await _db.Divisions
                            .Include(x => x.Employees)
                            .ToListAsync();
        }

        public override async Task<Division> Update(Division division)
        {
            var result = await _db.Divisions
                                  .FirstOrDefaultAsync(x => x.Id == division.Id);

            if(result != null)
            {
                result.DivisionName = division.DivisionName;

                await _db.SaveChangesAsync();

                return result;
            }
            return null!;
        }
    }
}
