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
    public class EmployeeControllerTest
    {
        [Fact]
        public void GetAllEmployeeStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.GetAll()).Throws(new Exception());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.GetEmployees().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось получить информацию с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void GetEmployeeStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.GetEmployee(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось получить информацию с сервера", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void CreateEmployeeStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Add(null!)).Throws(new Exception());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            var randFirstName = Guid.NewGuid().ToString();
            var randSecondName = Guid.NewGuid().ToString();
            var createdEmployee = new Employee() { FirstName = randFirstName, SecondName = randSecondName, DivisionId = 1 };
            //act

            var result = employeeController.AddEmployee(employee: createdEmployee, null).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось добавить нового отрудника", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateEmployeeStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Update(null!)).Throws(new Exception());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            var randName = Guid.NewGuid().ToString();
            //act

            var result = employeeController.UpdateEmployee(id: 0, employee: null!).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Ошибка обновления данных сотрудника", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void DeleteEmployeeStatusInternalServerError()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Get(1)).Throws(new Exception());
            mock.Setup(x => x.Delete(1)).Throws(new Exception());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.DeleteEmployee(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, ((ObjectResult)result).StatusCode);
            Assert.Equal("Ошибка удаления сотрудника", ((ObjectResult)result).Value!.ToString());
        }
        [Fact]
        public void GetEmployeeStatusOk()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(new Employee());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.GetEmployee(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void GetAllEmployeesStatusOK()
        {
            //arrange
            Employee[] employees = new Employee[] { new Employee()  };
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.GetAll()).ReturnsAsync(employees);

            EmployeesController employeeController = new EmployeesController(mock.Object);
            //act
            var result = employeeController.GetEmployees().GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void UpdateEmployeeStatusOK()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedEmployee = new Employee() { FirstName = randName, Id = 1 };

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Update(updatedEmployee)).ReturnsAsync(updatedEmployee);
            mock.Setup(x => x.Get(1)).ReturnsAsync(new Employee());

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.UpdateEmployee(id: 1, employee: updatedEmployee).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void DeleteEmployeeStatusOk()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var deletedEmployee = new Employee() { FirstName = randName, Id = 1 };

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Get(1)).ReturnsAsync(deletedEmployee);
            mock.Setup(x => x.Delete(1));

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.DeleteEmployee(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status200OK, ((ObjectResult)result).StatusCode);
            Assert.Equal($"Сотрудник 1 бы успешно удалён", ((ObjectResult)result).Value!.ToString());
        }
        [Fact]
        public void CreateEmployeeStatusCreated()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var createdEmployee = new Employee() { FirstName = randName };

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Add(createdEmployee)).ReturnsAsync(createdEmployee);

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.AddEmployee(employee: createdEmployee, null).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status201Created, ((ObjectResult)result.Result!).StatusCode);
        }
        [Fact]
        public void CreateEmployeeStatusBadRequest()
        {
            //arrange

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Add(null!));

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.AddEmployee(employee: null!, null).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Данные о сотруднике не заполненны пустое или некорректно введены", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void UpdateEmployeeStatusBadRequest()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedEmployee = new Employee() { FirstName = randName, Id = 2 };

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Update(updatedEmployee)).ReturnsAsync(updatedEmployee);
            mock.Setup(x => x.Get(2)).ReturnsAsync(updatedEmployee);

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.UpdateEmployee(id: 1, employee: updatedEmployee).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status400BadRequest, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("id у работника и зпроса различны", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void GetEmployeeStatusNotFound()
        {
            //arrange
            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Get(0));

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.GetEmployee(id: 1).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти сотрудника по данному id", ((ObjectResult)result.Result!).Value!.ToString());
        }
        [Fact]
        public void UpdateEmployeeStatusNotFound()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var updatedEmployee = new Employee() { FirstName = randName, Id = 2 };

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Update(updatedEmployee)).ReturnsAsync(updatedEmployee);
            mock.Setup(x => x.Get(1)).ReturnsAsync(updatedEmployee);

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.UpdateEmployee(id: 2, employee: updatedEmployee).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Null(result.Value);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result.Result!).StatusCode);
            Assert.Equal("Не удалось найти сотрудника по id: 2", ((ObjectResult)result.Result).Value!.ToString());
        }
        [Fact]
        public void DeleteEmployeeStatusNotFound()
        {
            //arrange
            var randName = Guid.NewGuid().ToString();
            var deletedEmployee = new Employee() { FirstName = randName, Id = 1};

            Mock<IDbRepository<Employee>> mock = new Mock<IDbRepository<Employee>>();
            mock.Setup(x => x.Get(1));
            mock.Setup(x => x.Delete(1));

            EmployeesController employeeController = new EmployeesController(mock.Object);

            //act
            var result = employeeController.DeleteEmployee(id: 2).GetAwaiter().GetResult();

            //assert
            Assert.NotNull(result);
            Assert.Equal(StatusCodes.Status404NotFound, ((ObjectResult)result).StatusCode);
            Assert.Equal($"Не удалось найти сотрудника по id: 2", ((ObjectResult)result).Value!.ToString());
        }
    }
}
