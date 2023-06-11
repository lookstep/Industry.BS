using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Data.Repositories
{
    public class IssueRepository : DbRepository<Issue>
    {
        private readonly AppDbContext _db;
        public IssueRepository(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public override async Task<Issue> Add(Issue taskForEmployee)
        {   
            if(taskForEmployee.Project != null)
            {
                _db.Entry(taskForEmployee.Project).State = EntityState.Unchanged;
            }
            var result = await _db.Issues.AddAsync(taskForEmployee);

            await _db.SaveChangesAsync();

            return result.Entity;
        }

        public override async Task Delete(int id)
        {
            var result = await _db.Issues.FirstOrDefaultAsync(x => x.Id == id);

            if(result != null)
            {
                _db.Remove(result);

                await _db.SaveChangesAsync();
            }
        }
        public override async Task<Issue> Get(int id)
        {
            return await _db.Issues
                            .Include(x => x.LaborCosts)
                            .Include(x => x.Project)
                            .FirstOrDefaultAsync(t => t.Id == id);
        }

        public override async Task<IEnumerable<Issue>> GetAll()
        {
            return await _db.Issues
                            .Include(x => x.LaborCosts)
                            .Include(x => x.Project)
                            .ToListAsync();
        }

        public override async Task<Issue> Update(Issue taskForEmployee)
        {
            var result = await _db.Issues.FirstOrDefaultAsync(x => x.Id == taskForEmployee.Id);

            if(result != null)
            {
                result.TaskName = taskForEmployee.TaskName;
                result.TaskDiscribe = taskForEmployee.TaskDiscribe;
                if (taskForEmployee.Id != 0)
                {
                    result.ProjectId = taskForEmployee.ProjectId;
                }
                else if(taskForEmployee.Project != null)
                {
                    result.ProjectId = taskForEmployee.Project.Id;
                }

                await _db.SaveChangesAsync();

                return result;
            }
            return null!;
        }
    }
}
