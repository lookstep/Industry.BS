using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        public override async Task<Employee> AddWithFile(Employee employee, IFormFile file)
        {
            if (file != null)
            {
                // Check file size
                if (file.Length > 1 * 1024 * 1024) // 1 MB
                    throw new Exception("File is too large.");

                // Check file type
                var supportedTypes = new[] { "jpg", "jpeg", "png" };
                var fileExt = Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt))
                    throw new Exception("Invalid file type.");

                // Check file content
                try
                {
                    using var image = Image.Load(file.OpenReadStream());
                }
                catch (Exception)
                {
                    throw new Exception("Invalid image content.");
                }

                var path = Path.Combine(
                            Directory.GetCurrentDirectory(), "storage/PersonalAccount",
                            $"{employee.FirstName}personalPhoto_{Guid.NewGuid()}");

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

                employee.IconPath = path;
            }

            return await Add(employee);
        }



        public override async Task Delete(int id)
        {
            var result = await _db.Employees
                                  .FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {
                _db.Remove(result);
                await _db.SaveChangesAsync();
            }
        }

        public override async Task<Employee> Get(int id)
        {
            return await _db.Employees
                            .Include(x => x.Division)
                            .Include(x => x.LaborCosts)
                            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public override async Task<IEnumerable<Employee>> GetAll()
        {
            return await _db.Employees
                            .Include(x => x.Division)
                            .Include(x => x.LaborCosts)
                            .ToListAsync();
        }

        public override async Task<Employee> Update(Employee employee)
        {
            var result = await Get(employee.Id);
            if (result != null)
            {
                result.FirstName = employee.FirstName;
                result.SecondName = employee.SecondName;
                result.LastName = employee.LastName;
                result.Role = employee.Role;
                result.OneCPass = employee.OneCPass;
                result.ServiceNumber = employee.ServiceNumber;
                result.Post = employee.Post;
                result.Email = employee.Email;
                result.DivisionId = employee.DivisionId;
                result.LaborCosts = employee.LaborCosts;
                result.IconPath = employee.IconPath;

                if (!string.IsNullOrEmpty(employee.NewPassword))
                {
                    var hasher = new PasswordHasher<Employee>();
                    employee.NewPassword = hasher.HashPassword(employee, employee.NewPassword);
                    result.Password = employee.NewPassword;
                }

                await _db.SaveChangesAsync();

                return result;
            }
            return null!;
        }

    }
}
