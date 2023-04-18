using EmoloyeeTask.Data.Interfaces;
using EmployeeTask.API.Controllers;
using EmployeeTask.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using Xunit;


namespace EmoloyeeTask.API.Tests
{
    public class ProjectControllerTest
    {
        [Fact]
        public void GetAllProjectStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.GetAll()).Throws(new Exception());

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.GetProjects().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка получения данных с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void GetProjectStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.GetProjectById(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка получения данных с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void CreateProjectStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Add(new Project())).Throws(new Exception());

            ProjectsController projectController = new ProjectsController(mock.Object);

            var randName = Guid.NewGuid().ToString();
            var createdProject = new Project() { ProjectName = randName };
            //act

            var result = projectController.CreateProject(project: createdProject).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка создания нового проекта", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateProjectStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Update(null!)).Throws(new Exception());

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act

            var result = projectController.UpdateProject(id: 0, project: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка изменения данных проекта", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void DeleteProjectStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());
            mock.Setup(x => x.Delete(1)).Throws(new Exception());

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.DeleteProject(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Ошибка удаления данных проекта", ((ObjectResult)result).Value!.ToString());
        }
        [Fact]
        public void GetProjectStatusOk()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(new Project());

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.GetProjectById(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void GetAllProjectStatusOK()
        {
            //arrange
            Project[] project = new Project[] { new Project() };
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.GetAll()).ReturnsAsync(project);

            ProjectsController projectController = new ProjectsController(mock.Object);
            //act
            var result = projectController.GetProjects().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void UpdateProjectStatusOK()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedProject = new Project() { ProjectName = randName, Id = 1 };

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Update(updatedProject)).ReturnsAsync(updatedProject);
            mock.Setup(x => x.Get(1)).ReturnsAsync(new Project());

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.UpdateProject(id: 1, project: updatedProject).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void DeleteProjectStatusOk()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var deletedProject = new Project() { ProjectName = randName, Id = 1 };

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedProject);
            mock.Setup(x => x.Delete(1));

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.DeleteProject(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
            Assert.Equal($"Проект по id 1 был успешно удалён", ((ObjectResult)result).Value!.ToString());
        }

        [Fact]
        public void CreateProjectStatusCreated()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var createdProject = new Project() { ProjectName = randName };

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Add(createdProject)).ReturnsAsync(createdProject);

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.CreateProject(project: createdProject).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void CreateProjectStatusBadRequest()
        {
            //arrange

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Add(null!));

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.CreateProject(project: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Даннные проекта пусты или некорректно введены", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void UpdateProjectStatusBadRequest()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedProject = new Project() { ProjectName = randName, Id = 2 };

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Update(updatedProject)).ReturnsAsync(updatedProject);
            mock.Setup(x => x.Get(2)).ReturnsAsync(updatedProject);

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.UpdateProject(id: 1, project: updatedProject).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("id у проекта и зпроса различны", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void GetProjectStatusNotFound()
        {
            //arrange
            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Get(0));

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.GetProjectById(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти проект по данному id", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateProjectStatusNotFound()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedProject = new Project() { ProjectName = randName, Id = 2 };

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Update(updatedProject)).ReturnsAsync(updatedProject);
            mock.Setup(x => x.Get(1)).ReturnsAsync(updatedProject);

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.UpdateProject(id: 2, project: updatedProject).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти подразделение по id: 2", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void DeleteProjectStatusNotFound()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var deletedProject = new Project() { ProjectName = randName, Id = 2 };

            Mock<IDbRepository<Project>> mock = new Mock<IDbRepository<Project>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedProject);
            mock.Setup(x => x.Delete(2));

            ProjectsController projectController = new ProjectsController(mock.Object);

            //act
            var result = projectController.DeleteProject(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.Equal("Не удалось найти проект по id: 2", ((ObjectResult)result).Value!.ToString());
        }
    }
}
