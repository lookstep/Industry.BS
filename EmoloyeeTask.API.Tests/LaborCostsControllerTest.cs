using EmoloyeeTask.Data.Interfaces;
using EmployeeTask.API.Controllers;
using EmployeeTask.Data.Repositories;
using EmployeeTask.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EmoloyeeTask.API.Tests
{
    public class LaborCostsControllerTest
    {
        [Fact]
        public void GetAllLaborCostStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.GetAll()).Throws(new Exception());

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.GetLaborCosts().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка получения данных с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void GetLaborCostStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.GetLaborCostById(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка получения данных с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void CreateLaborCostStatusInternalServerError()
        {
            //arrange
            var createdLaborCost = new LaborCost();
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Add(createdLaborCost)).Throws(new Exception());

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act

            var result = laborCostController.CreateLaborCosts(laborCosts: createdLaborCost).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка создания данных о трудозатратах", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateLaborCostStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Update(null!)).Throws(new Exception());

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);
;
            //act

            var result = laborCostController.UpdateLaborCosts(id: 0, laborCosts: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка обновления данных о трудозатратах", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void DeleteLaborCostStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());
            mock.Setup(x => x.Delete(1)).Throws(new Exception());

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.DeleteLaborCosts(id: 1).GetAwaiter().GetResult().Result!;

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Ошибка удаления данных о трудозатратах", ((ObjectResult)result).Value!.ToString());
        }
        [Fact]
        public void GetLaborCostStatusOk()
        {
            //arrange
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(new LaborCost());

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.GetLaborCostById(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void GetAllLaborCostStatusOK()
        {
            //arrange
            LaborCost[] laborCosts = new LaborCost[] { new LaborCost() };
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.GetAll()).ReturnsAsync(laborCosts);

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);
            //act
            var result = laborCostController.GetLaborCosts().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void UpdateLaborCostStatusOK()
        {
            //arrange
            var updatedLaborCost = new LaborCost() { Date = DateTime.Now, Id = 1, HourCount = 5 };

            var mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(updatedLaborCost);
            mock.Setup(x => x.Save()).Returns(Task.CompletedTask);

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.UpdateLaborCosts(id: 1, laborCosts: updatedLaborCost).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }

        [Fact]
        public void DeleteLaborCostStatusOk()
        {
            //arrange
            var deletedLaborCost = new LaborCost() { Date = DateTime.Now, Id = 1 };

            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedLaborCost);
            mock.Setup(x => x.Delete(1));

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.DeleteLaborCosts(id: 1).GetAwaiter().GetResult().Result!;

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
            Assert.Equal("Данные о трудозатратах с id 1 были удалены", ((ObjectResult)result).Value!.ToString());
        }

        [Fact]
        public void CreateLaborCostStatusCreated()
        {
            //arrange
            var createdLaborCost = new LaborCost() { Date = DateTime.Now };

            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Add(createdLaborCost)).ReturnsAsync(createdLaborCost);

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.CreateLaborCosts(laborCosts: createdLaborCost).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void CreateLaborCostStatusBadRequest()
        {
            //arrange

            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Add(null!));

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.CreateLaborCosts(laborCosts: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Данные о трудозатратах пусты или некорректно добавлены", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void UpdateLaborCostStatusBadRequest()
        {
            //arrange
            var updatedLaborCost = new LaborCost() { Date = DateTime.Now, Id = 2 };

            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Update(updatedLaborCost)).ReturnsAsync(updatedLaborCost);
            mock.Setup(x => x.Get(2)).ReturnsAsync(updatedLaborCost);

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.UpdateLaborCosts(id: 1, laborCosts: updatedLaborCost).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("id у трудозатрат и зпроса различны", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void GetLaborCostStatusNotFound()
        {
            //arrange
            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(0));

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.GetLaborCostById(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти трудозатраты по данному id", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateLaborCostStatusNotFound()
        {
            //arrange
            var updatedLaborCost = new LaborCost() { Date = DateTime.Now, Id = 2 };

            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Update(updatedLaborCost)).ReturnsAsync(updatedLaborCost);
            mock.Setup(x => x.Get(1)).ReturnsAsync(updatedLaborCost);

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.UpdateLaborCosts(id: 2, laborCosts: updatedLaborCost).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal($"Не удалось найти трудозатраты по данному id", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void DeleteLaborCostStatusNotFound()
        {
            //arrange
            var deletedLaborCost = new LaborCost() { Date = DateTime.Now, Id = 2 };

            Mock<IDbRepository<LaborCost>> mock = new Mock<IDbRepository<LaborCost>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedLaborCost);
            mock.Setup(x => x.Delete(2));

            LaborCostsController laborCostController = new LaborCostsController(mock.Object);

            //act
            var result = laborCostController.DeleteLaborCosts(id: 2).GetAwaiter().GetResult().Result!;

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.Equal("Не удалось найти трудозатраты по id: 2", ((ObjectResult)result).Value!.ToString());
        }
    }
}
