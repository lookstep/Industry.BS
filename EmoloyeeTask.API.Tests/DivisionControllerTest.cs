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
    public class DivisionControllerTest
    {
        [Fact]
        public void GetAllDivisionsStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.GetAll()).Throws(new Exception());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.Divisions().GetAwaiter().GetResult();
            
            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка получения данных с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void GetDivisionStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.GetDivision(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка получения данных с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void CreateDivisionStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Add(new Division())).Throws(new Exception());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            var randName = Guid.NewGuid().ToString();
            var createdDivision = new Division() { DivisionName = randName };
            //act

            var result = divisionController.CreateDivision(division: createdDivision).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка добавления данных", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateDivisionStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Update(null!)).Throws(new Exception());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            var randName = Guid.NewGuid().ToString();
            //act

            var result = divisionController.UpdateDivision(id: 0, division: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка обновления данных", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void DeleteDivisionStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());
            mock.Setup(x => x.Delete(1)).Throws(new Exception());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.DeleteDivision(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Ошибка удаления подразделения", ((ObjectResult)result).Value!.ToString());
        }
        [Fact]
        public void GetDivisionStatusOk()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(new Division());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.GetDivision(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void GetAllDivisionsStatusOK()
        {
            //arrange
            Division[] divisions = new Division[] { new Division() };
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.GetAll()).ReturnsAsync(divisions);

            DivisionsController divisionController = new DivisionsController(mock.Object);
            //act
            var result = divisionController.Divisions().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void UpdateDivisionStatusOK()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedDivision = new Division() { DivisionName = randName, Id = 1 };

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Update(updatedDivision)).ReturnsAsync(updatedDivision);
            mock.Setup(x => x.Get(1)).ReturnsAsync(new Division());

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.UpdateDivision(id: 1, division: updatedDivision).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void DeleteDivisionStatusOk()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var deletedDivision = new Division() { DivisionName = randName, Id = 1 };

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedDivision);
            mock.Setup(x => x.Delete(1));

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.DeleteDivision(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
            Assert.Equal("Отдел 1 бы успешно удалён", ((ObjectResult)result).Value!.ToString());
        }

        [Fact]
        public void CreateDivisionStatusCreated()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var createdDivision = new Division() { DivisionName = randName };

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Add(createdDivision)).ReturnsAsync(createdDivision);

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.CreateDivision(division: createdDivision).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void CreateDivisionStatusBadRequest()
        {
            //arrange

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Add(null!));

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.CreateDivision(division: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Подразделение пустое или некорректно добавлены данные", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void UpdateDivisionStatusBadRequest()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedDivision = new Division() { DivisionName = randName, Id = 2 };

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Update(updatedDivision)).ReturnsAsync(updatedDivision);
            mock.Setup(x => x.Get(2)).ReturnsAsync(updatedDivision);

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.UpdateDivision(id: 1, division: updatedDivision).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("id у подразделения и зпроса различны", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void GetDivisionStatusNotFound()
        {
            //arrange
            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Get(0));

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.GetDivision(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти подразделение по данному id", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateDivisionStatusNotFound()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedDivision = new Division() { DivisionName = randName, Id = 2 };

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Update(updatedDivision)).ReturnsAsync(updatedDivision);
            mock.Setup(x => x.Get(1)).ReturnsAsync(updatedDivision);

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.UpdateDivision(id: 2, division: updatedDivision).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти подразделение по id: 2", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void DeleteDivisionStatusNotFound()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var deletedDivision = new Division() { DivisionName = randName, Id = 2 };

            Mock<IDbRepository<Division>> mock = new Mock<IDbRepository<Division>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedDivision);
            mock.Setup(x => x.Delete(2));

            DivisionsController divisionController = new DivisionsController(mock.Object);

            //act
            var result = divisionController.DeleteDivision(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.Equal("Не удалось найти подразделение по id: 2", ((ObjectResult)result).Value!.ToString());
        }

    }
}