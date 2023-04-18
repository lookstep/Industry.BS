using EmployeeTask.Data.Repositories;
using EmployeeTask.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using Xunit;

namespace EmoloyeeTask.Data.Test
{
    public class LaborCostRepositoryTest
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly AppDbContext _context;

        public LaborCostRepositoryTest()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("GetLaborCostsTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            _context = new AppDbContext(_contextOptions);

        }
        [Fact]
        public void GetLaborCost()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Assignments.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", DivisionId = 2 });

            _context.LaborCosts.AddRange(
                new LaborCost() { Id = 1, Date = System.DateTime.Now, IssueId = 1, EmployeeId = 1},
                new LaborCost() { Id = 2, Date = System.DateTime.Today, IssueId = 2, EmployeeId = 2 }
                );


            _context.SaveChanges();

            var repository = new LaborCostRepository(_context);

            //act
            var result = repository.Get(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);

        }
        [Fact]
        public void GetAllLaborCost()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Assignments.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", DivisionId = 2 });

            _context.LaborCosts.AddRange(
                new LaborCost() { Id = 1, Date = System.DateTime.Now, IssueId = 1, EmployeeId = 1 },
                new LaborCost() { Id = 2, Date = System.DateTime.Today, IssueId = 2, EmployeeId = 2 }
                );


            _context.SaveChanges();

            var repository = new LaborCostRepository(_context);

            //actl
            var result = repository.GetAll().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public void UpdateLaborCost()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Assignments.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", DivisionId = 2 });

            var addedLaborCost = new LaborCost() { Id = 1, Date = System.DateTime.Now, IssueId = 1, EmployeeId = 1 };
            var laborCostDate = addedLaborCost.Date;

            _context.LaborCosts.Add(addedLaborCost);

            _context.SaveChanges();

            var repository = new LaborCostRepository(_context);

            var changedLaborCost = new LaborCost() { Id = 1, Date = System.DateTime.Today, IssueId = 2, EmployeeId = 2 };

            //act
            var result = repository.Update(laborCosts: changedLaborCost).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotEqual(laborCostDate, result.Date);
            Assert.Equal(changedLaborCost.Id, result.Id);

        }
        [Fact]
        public void AddLaborCost()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Assignments.AddRange(
               new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
               new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", DivisionId = 2 });

            _context.SaveChanges();

            var repository = new LaborCostRepository(_context);

            var addedLaborCost = new LaborCost() { Id = 1, Date = System.DateTime.Now, IssueId = 1, EmployeeId = 1 };
            var laborCostDate = addedLaborCost.Date;

            //act
            var result = repository.Add(laborCosts: addedLaborCost).GetAwaiter().GetResult();


            //assert
            Assert.NotNull(result);
            Assert.NotNull(_context.LaborCosts.FirstOrDefault(x => x.Date == laborCostDate));
            Assert.Equal(addedLaborCost.Id, result.Id);
        }
        [Fact]
        public void DeleteLaborCost()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Assignments.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", DivisionId = 2 });

            _context.LaborCosts.AddRange(
                new LaborCost() { Id = 1, Date = System.DateTime.Now, IssueId = 1, EmployeeId = 1 },
                new LaborCost() { Id = 2, Date = System.DateTime.Today, IssueId = 2, EmployeeId = 2 },
                new LaborCost() { Id = 3, Date = System.DateTime.UtcNow, IssueId = 1, EmployeeId = 2 }
                );

            _context.SaveChanges();

            var repository = new LaborCostRepository(_context);

            var deletedLaborCost = _context.LaborCosts.FirstOrDefault(x => x.Id == 3);

            //act
            repository.Delete(id: 3).GetAwaiter().GetResult();

            //assert
            Assert.Null(_context.Divisions.FirstOrDefault(x => x.Id == 3));
            Assert.DoesNotContain(deletedLaborCost, _context.LaborCosts);
        }
    }
}
