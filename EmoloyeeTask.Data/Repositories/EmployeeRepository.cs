using EmoloyeeTask.Data;
using EmoloyeeTask.Data.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeTask.Data.Repositories
{
    public class EmployeeRepository : DbRepository<Employee>
    {
        private readonly AppDbContext _db;
        private readonly ILogger<EmployeeRepository> _logger;
        public EmployeeRepository(AppDbContext db, ILogger<EmployeeRepository> logger) : base(db)
        {
            _db = db;
            _logger = logger;
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
            employee.IconPath = await HandleFile(file, employee.FirstName);
            return await Add(employee);
        }

        private async Task<string> HandleFile(IFormFile file, string fileNamePrefix)
        {
            var defaultImagePath = Path.Combine(Directory.GetCurrentDirectory(), "storage/PersonalAccount", "default-user.png");
            var path = string.Empty;
            if (file == null)
            {
                return File.Exists(defaultImagePath)
                    ? defaultImagePath
                    : null!;
            }

            if (file.Length > 1 * 1024 * 1024)
                throw new Exception("Файл больше разрещённого рзмера");

            var fileExt = Path.GetExtension(file.FileName).Substring(1);
            if (!IsSupportedFileType(fileExt))
                throw new Exception("Расширение данного файла неподдерживается");

            try
            {
                using var image = SixLabors.ImageSharp.Image.Load(file.OpenReadStream());

                path = Path.Combine(
                          Directory.GetCurrentDirectory(),
                          "storage/PersonalAccount",
                          $"{fileNamePrefix}_personalPhoto_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");

                using var stream = new FileStream(path, FileMode.Create);
                await file.CopyToAsync(stream);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Произошла ошибка добавления файла: {ex.Message}");
                path = defaultImagePath;
            }
            return path;
        }

        public override async Task Delete(int id)
        {
            var result = await _db.Employees
                                  .FirstOrDefaultAsync(x => x.Id == id);
            if (result != null)
            {

                try
                {
                    File.Delete(result.IconPath);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Произошла ошибка удаления файла: {ex.Message}");
                }
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
                result.ConfirmationCode = employee.ConfirmationCode;

                if (!result.Password.Equals(employee.Password))
                {
                    var hasher = new PasswordHasher<Employee>();
                    result.Password = hasher.HashPassword(employee, employee.Password);
                }

                await _db.SaveChangesAsync();

                return result;
            }
            return null!;
        }

        public override async Task<Employee> UpdateFile(Employee employee, IFormFile file)
        {
            var result = await Get(employee.Id);
            if (result != null)
            {
                result.IconPath = await HandleFile(file, employee.FirstName);
            }

            await _db.SaveChangesAsync();
            return result;
        }

        public override void DeleteFile(Employee employee, string filePath)
        {
            base.DeleteFile(employee, filePath);

            employee.IconPath = Path.Combine(Directory.GetCurrentDirectory(), "storage/PersonalAccount", "default-user.png");
            _db.SaveChanges();
        }

        private static bool IsSupportedFileType(string fileExtension)
        {
            var supportedTypes = new[] { "jpg", "jpeg", "png" };
            return supportedTypes.Contains(fileExtension, StringComparer.OrdinalIgnoreCase);
        }
    }
}
