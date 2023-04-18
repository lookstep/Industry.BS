using EmployeeTask.Data.Repositories;
using EmployeeTask.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Linq;
using Xunit;

namespace EmoloyeeTask.Data.Test
{
    public class ProjectRepositoryTest
    {
        private readonly DbContextOptions<AppDbContext> _contextOptions;
        private readonly AppDbContext _context;

        public ProjectRepositoryTest()
        {
            _contextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("GetProjectTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;
            _context = new AppDbContext(_contextOptions);

        }
        [Fact]
        public void GetProject()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" });

            _context.SaveChanges();

            var repository = new ProjectRepository(_context);

            //act
            var result = repository.Get(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Id);

        }
        [Fact]
        public void GetAllProject()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" });

            _context.SaveChanges();

            var repository = new ProjectRepository(_context);

            //actl
            var result = repository.GetAll().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

        }
        [Fact]
        public void UpdateProject()
        {
            //arrange
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var addedProject = new Project() { Id = 1, ProjectName = "SpaceZXC"};
            var projectName = addedProject.ProjectName;

            _context.Projects.Add(addedProject);

            _context.SaveChanges();

            var repository = new ProjectRepository(_context);

            var changedProject = new Project { Id = 1, ProjectName = "Justif_X" };

            //act
            var result = repository.Update(project: changedProject).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotEqual(projectName, result.ProjectName);
            Assert.Equal(changedProject.Id, result.Id);

        }
        [Fact]
        public void AddProject()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            var repository = new ProjectRepository(_context);

            var addedProject = new Project() { Id = 1, ProjectName = "SpaceZXC" };

            //act
            var result = repository.Add(project: addedProject).GetAwaiter().GetResult();
            _context.SaveChanges();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(_context.Projects.FirstOrDefault(x => x.ProjectName == "SpaceZXC"));
            Assert.Equal(addedProject.Id, result.Id);
        }
        [Fact]
        public void DeleteProject()
        {
            //arrange

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.Projects.AddRange(
                new Project { Id = 1, ProjectName = "SpaceZXC" },
                new Project { Id = 2, ProjectName = "Justif_X" },
                new Project { Id = 3, ProjectName = "Safari_Y" });

            _context.SaveChanges();

            var repository = new ProjectRepository(_context);

            var deletedProject = _context.Projects.FirstOrDefault(x => x.Id == 3);

            //act
            repository.Delete(id: 3).GetAwaiter().GetResult();

            //assert
            Assert.Null(_context.Divisions.FirstOrDefault(x => x.Id == 3));
            Assert.DoesNotContain(deletedProject, _context.Projects);
        }
    }
}
