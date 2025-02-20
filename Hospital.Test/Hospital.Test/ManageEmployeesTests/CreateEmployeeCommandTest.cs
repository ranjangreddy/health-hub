using Hospital.Commands.ManageEmployees;
using Hospital.Database.Interfaces;
using Hospital.Entities.Employee;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;
using NHibernate;

namespace Hospital.Test.ManageEmployeesTests
{
    public class CreateEmployeeCommandTest
    {
        private Mock<IObjectsFactory> mockObjectsFactory;
        private Mock<IDTOFactory> mockDtoFactory;
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IValidateObjects> mockValidateObjects;
        private Mock<IListsStorage> mockListsStorage;
        private Mock<IDatabaseOperations> mockDatabaseOperations;

        private CreateEmployeeCommand createEmployeeCommand;

        private void SetUpMocks()
        {
            mockObjectsFactory = new Mock<IObjectsFactory>();
            mockDtoFactory = new Mock<IDTOFactory>();
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockValidateObjects = new Mock<IValidateObjects>();
            mockListsStorage = new Mock<IListsStorage>();
            mockDatabaseOperations = new Mock<IDatabaseOperations>();

            createEmployeeCommand = new CreateEmployeeCommand(
                mockObjectsFactory.Object,
                mockDtoFactory.Object,
                mockMenuHandler.Object,
                mockListManage.Object,
                mockValidateObjects.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenNoWard_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([]);

            createEmployeeCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.FactoryMessages.NoWardErrorPrompt), Times.Once());
            mockDtoFactory.Verify(x => x.GatherEmployeeData(mockListsStorage.Object.Wards), Times.Never());
        }

        [Fact]
        public void Execute_WhenEmployeeValidationFailed_ShouldReturnEarly()
        {
            SetUpMocks();

            var mockWard = new Mock<Ward>();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([mockWard.Object]);

            mockValidateObjects.Setup(x => x.ValidateEmployeeObject(It.IsAny<EmployeeDTO>()))
                               .Returns(false);

            createEmployeeCommand.Execute();

            mockObjectsFactory.Verify(x => x.CreateEmployee(It.IsAny<EmployeeDTO>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenWardsListIsNotNullAndValidationPassed_ShouldCreateEmployee()
        {
            SetUpMocks();

            var mockWard = new Mock<Ward>();
            var mockEmployee = new Mock<Employee>();
            var employeesList = new List<Employee>();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([mockWard.Object]);
            mockListsStorage.Setup(x => x.Employees)
                            .Returns(employeesList);

            mockValidateObjects.Setup(x => x.ValidateEmployeeObject(It.IsAny<EmployeeDTO>()))
                               .Returns(true);

            mockObjectsFactory.Setup(x => x.CreateEmployee(It.IsAny<EmployeeDTO>()))
                              .Returns(mockEmployee.Object);

            mockDatabaseOperations.Setup(x => x.Add(It.IsAny<Employee>(), It.IsAny<ISession>()))
                                  .Returns(true);

            mockListManage.Setup(x => x.Add(It.IsAny<Employee>(), It.IsAny<List<Employee>>()))
                          .Callback((Employee item, List<Employee> list) =>
                          {
                              if (mockDatabaseOperations.Object.Add(item, new Mock<ISession>().Object))
                              {
                                  list.Add(item);
                              }
                          });

            createEmployeeCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(string.Format(UiMessages.CreateEmployeeMessages.OperationSuccessPrompt, mockEmployee.Object.Position, mockEmployee.Object.Name, mockEmployee.Object.Surname)), Times.Once());
            Assert.Contains(mockEmployee.Object, employeesList);
        }
    }
}