using EmployeeTask.Data.Repositories;
using EmployeeTask.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using Xunit;

namespace EmoloyeeTask.Data.Test
{
    public class IssueRepositoryTest
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly AppDbContext _context;

        public IssueRepositoryTest()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("GetAssigmentTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            _context = new AppDbContext(_contextOptions);

        }
        [Fact]
        public void GetAssignment()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
               new Project { Id = 1, ProjectName = "SpaceZXC" },
               new Project { Id = 2, ProjectName = "Justif_X" });

            _context.Issues.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1},
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2}
                );

            _context.SaveChanges();

            var repository = new IssueRepository(_context);

            //act
            var result = repository.Get(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);

        }
        [Fact]
        public void GetAllAssignment()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" });

            _context.Issues.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 }
                );

            _context.SaveChanges();

            var repository = new IssueRepository(_context);

            //act
            var result = repository.GetAll().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public void UpdateAssignment()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" });

            var addedAssignment = new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 };
            var taskName = addedAssignment.TaskName;

            _context.Issues.Add(addedAssignment);

            _context.SaveChanges();

            var repository = new IssueRepository(_context);

            var changedAssignment = new Issue() { Id = 1, TaskName = "Сделать круто", ProjectId = 1 };

            //act
            var result = repository.Update(taskForEmployee: changedAssignment).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotEqual(taskName, result.TaskName);
        }
        [Fact]
        public void AddAssignment()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" });

            _context.SaveChanges();

            var repository = new IssueRepository(_context);

            var addedAssignment = new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 };

            //act
            var result = repository.Add(taskForEmployee: addedAssignment).GetAwaiter().GetResult();
            _context.SaveChanges();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(_context.Issues.FirstOrDefault(x => x.TaskName == "Сделать красиво"));
            Assert.Equal(addedAssignment.Id, result.Id);
        }
        [Fact]
        public void DeleteAssignment()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" });

            _context.Issues.AddRange(
                new Issue() { Id = 1, TaskName = "Сделать красиво", ProjectId = 1 },
                new Issue() { Id = 2, TaskName = "Сделать круто", ProjectId = 2 },
                new Issue() { Id = 3, TaskName = "Сделать замечательно", ProjectId = 2 }
                );

            _context.SaveChanges();

            var repository = new IssueRepository(_context);

            var deletedAssignment = _context.Issues.FirstOrDefault(x => x.Id == 3);

            //act
            repository.Delete(id: 3).GetAwaiter().GetResult();

            //assert
            Assert.Null(_context.Divisions.FirstOrDefault(x => x.Id == 3));
            Assert.DoesNotContain(deletedAssignment, _context.Issues);
        }
    }
}
