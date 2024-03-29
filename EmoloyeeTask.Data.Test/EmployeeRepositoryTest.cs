﻿using EmployeeTask.Data.Repositories;
using EmployeeTask.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq;
using Xunit;


namespace EmoloyeeTask.Data.Test
{
    public class EmployeeRepositoryTest
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly AppDbContext _context;

        public EmployeeRepositoryTest()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("GetEmployeeTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            _context = new AppDbContext(_contextOptions);

        }
        [Fact]
        public void GetEmployee()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел" },
                new Division { Id = 2, DivisionName = "Отдел разработки" });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", Email = "email1@bk.ru", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", Email = "email2@bk.ru", DivisionId = 2 });

            _context.SaveChanges();

            var loggerMock = new Mock<ILogger<EmployeeRepository>>();
            ILogger<EmployeeRepository> logger = loggerMock.Object;
            var repository = new EmployeeRepository(_context, logger);

            //act
            var result = repository.Get(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);

        }
        [Fact]
        public void GetAllEmployee()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел" },
                new Division { Id = 2, DivisionName = "Отдел разработки" });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", Email = "email1@bk.ru", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", Email = "email2@bk.ru", DivisionId = 2 });

            _context.SaveChanges();

            var loggerMock = new Mock<ILogger<EmployeeRepository>>();
            ILogger<EmployeeRepository> logger = loggerMock.Object;
            var repository = new EmployeeRepository(_context, logger);

            //act
            var result = repository.GetAll().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public void UpdateEmployee()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел" },
                new Division { Id = 2, DivisionName = "Отдел разработки" });

            var addedEmployee = new Employee() { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", Email = "email2@bk.ru", Password = "123", DivisionId = 1 };
            var firstName = addedEmployee.FirstName;

            _context.Employees.Add(addedEmployee);

            _context.SaveChanges();

            var loggerMock = new Mock<ILogger<EmployeeRepository>>();
            ILogger<EmployeeRepository> logger = loggerMock.Object;
            var repository = new EmployeeRepository(_context, logger);

            var changedEmployee = new Employee() { Id = 1, FirstName = "Олег", SecondName = "Патько", Email = "email1@bk.ru", Password = "321", DivisionId = 1 };

            //act
            var result = repository.Update(employee: changedEmployee).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotEqual(firstName, result.FirstName);

        }
        [Fact]
        public void AddEmployee()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел", },
                new Division { Id = 2, DivisionName = "Отдел разработки" });

            _context.SaveChanges();

            var loggerMock = new Mock<ILogger<EmployeeRepository>>();
            ILogger<EmployeeRepository> logger = loggerMock.Object;
            var repository = new EmployeeRepository(_context, logger);

            var addedEmployee = new Employee() { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", Email = "email@bk.ru", DivisionId = 1 };

            //act
            var result = repository.Add(employee: addedEmployee).GetAwaiter().GetResult();
            _context.SaveChanges();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(_context.Employees.FirstOrDefault(x => x.FirstName == "Михаил"));
        }
        [Fact]
        public void DeleteEmployee()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Divisions.AddRange(
                new Division { Id = 1, DivisionName = "Общий отдел" },
                new Division { Id = 2, DivisionName = "Отдел разработки" });

            _context.Employees.AddRange(
                new Employee { Id = 1, FirstName = "Михаил", SecondName = "Офунаренко", Email = "email2@bk.ru", DivisionId = 1 },
                new Employee { Id = 2, FirstName = "Виктор", SecondName = "Корнеплод", Email = "email1@bk.ru", DivisionId = 2 },
                new Employee { Id = 3, FirstName = "Жерёхин", SecondName = "Палыч", Email = "email@bk.ru", DivisionId = 2 });

            _context.SaveChanges();

            var loggerMock = new Mock<ILogger<EmployeeRepository>>();
            ILogger<EmployeeRepository> logger = loggerMock.Object;
            var repository = new EmployeeRepository(_context, logger);

            var deletedEmployee = _context.Employees.FirstOrDefault(x => x.Id == 3);

            //act
            repository.Delete(id: 3).GetAwaiter().GetResult();

            //assert
            Assert.Null(_context.Divisions.FirstOrDefault(x => x.Id == 3));
            Assert.DoesNotContain(deletedEmployee, _context.Employees);
        }
    }
}
