using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTask.Data.Repositories
{
    public class ProjectRepository : DbRepository<Project>
    {
        private readonly AppDbContext _db;
        public ProjectRepository(AppDbContext db) : base (db)
        {
            _db = db;
        }
        public override async Task<Project> Add(Project project)
        {
            var result = await _db.Projects
                                  .AddAsync(project);

            await _db.SaveChangesAsync();

            return result.Entity;
        }

        public override async Task Delete(int id)
        {
            var result = await _db.Projects
                                  .FirstOrDefaultAsync(x => x.Id == id);
            if(result != null)
            {
                _db.Projects.Remove(result);

                await _db.SaveChangesAsync();
            }
        }

        public override async Task<Project> Get(int id)
        {
            
            return await _db.Projects
                            .Include(x => x.Issues)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<Project>> GetAll()
        {
            return await _db.Projects
                            .Include(x => x.Issues)
                            .ToListAsync();
        }

        public override async Task<Project> Update(Project project)
        {
            var result = await _db.Projects
                                  .FirstOrDefaultAsync(x => x.Id == project.Id);

            if (result != null)
            {
                result.ProjectName = project.ProjectName;
                result.Issues = project.Issues;

                await _db.SaveChangesAsync();

                return result;
            }
            return null!;
        }
    }
}
