using EmployeeTask.Data.Repositories;
using EmployeeTask.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using Xunit;

namespace EmoloyeeTask.Data.Test
{
    public class DivisionRepositoryTest
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly AppDbContext _context;

        public DivisionRepositoryTest()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("GetDivisionTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            _context = new AppDbContext(_contextOptions);

        }
        [Fact]
        public void GetDivision()
        {
            //arrange       
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел"},
                new Division { Id = 2, DivisionName = "Отдел разработки"});

            _context.SaveChanges();

            var repository = new DivisionRepository(_context);

            //act
            var result = repository.Get(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);

        }
        [Fact]
        public void GetAllDivision()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел" },
                new Division { Id = 2, DivisionName = "Отдел разработки" });

            _context.SaveChanges();

            var repository = new DivisionRepository(_context);

            //act
            var result = repository.GetAll().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public void UpdateDivision()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            
            var addedDivision = new Division() { Id = 1, DivisionName = "Общий отдел" };
            var divisionName = addedDivision.DivisionName;

            _context.Divisions.Add(addedDivision);

            _context.SaveChanges();

            var repository = new DivisionRepository(_context);
            var changedDivision = new Division() { Id = 1, DivisionName = "Отдел кадров" };

            //act
            var result = repository.Update(division: changedDivision).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotEqual(divisionName, result.DivisionName);

        }
        [Fact]
        public void AddDivision()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var addedDivision = new Division() { Id = 1, DivisionName = "Общий отдел" };

            var repository = new DivisionRepository(_context);

            //act
            var result = repository.Add(division: addedDivision).GetAwaiter().GetResult();
            _context.SaveChanges();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(_context.Divisions.FirstOrDefault(x => x.DivisionName == "Общий отдел"));
        }
        [Fact]
        public void DeleteDivision()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
            new Division() { Id = 1, DivisionName = "Общий отдел" },
            new Division() { Id = 2, DivisionName = "Отдел разработки" },
            new Division() { Id = 3, DivisionName = "Отдел тестирования" });
          
            _context.SaveChanges();

            var deletedValue = _context.Divisions.FirstOrDefault(x => x.Id == 3);

            var repository = new DivisionRepository(_context);

            //act
            repository.Delete(id: 3).GetAwaiter().GetResult();

            //assert
            Assert.Null(_context.Divisions.FirstOrDefault(x => x.Id == 3));
            Assert.DoesNotContain(deletedValue, _context.Divisions);
        }
    }
}