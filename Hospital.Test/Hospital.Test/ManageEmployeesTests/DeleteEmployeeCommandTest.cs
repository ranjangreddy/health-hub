using Hospital.Commands.ManageEmployees;
using Hospital.Database.Interfaces;
using Hospital.Entities.Employee;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;
using NHibernate;

namespace Hospital.Test.ManageEmployeesTests
{
    public class DeleteEmployeeCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;
        private Mock<IDatabaseOperations> mockDatabaseOperations;

        private DeleteEmployeeCommand deleteEmployeeCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();
            mockDatabaseOperations = new Mock<IDatabaseOperations>();

            deleteEmployeeCommand = new DeleteEmployeeCommand(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenNoEmployee_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Employees)
                            .Returns([]);

            deleteEmployeeCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DeleteEmployeeMessages.NoEmployeesPrompt), Times.Once());
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DeleteEmployeeMessages.SelectPrompt), Times.Never());
        }

        [Fact]
        public void Execute_WhenIsEmployee_ShouldFireEmployee()
        {
            SetUpMocks();

            var mockEmployee = new Mock<Employee>();
            mockEmployee.SetupAllProperties();
            mockEmployee.Object.IsDeleted = false;

            var employeesList = new List<Employee> { mockEmployee.Object };

            mockListsStorage.Setup(x => x.Employees)
                            .Returns(employeesList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Employee>>(), It.IsAny<string>()))
                           .Returns(mockEmployee.Object);

            mockDatabaseOperations.Setup(x => x.Update(It.IsAny<Employee>(), It.IsAny<ISession>()))
                                  .Returns(true);

            mockListManage.Setup(x => x.SoftDelete(It.IsAny<Employee>(), It.IsAny<List<Employee>>()))
                          .Callback((Employee employee, List<Employee> list) =>
                          {
                              employee.IsDeleted = true;
                              list.Remove(employee);
                          });

            deleteEmployeeCommand.Execute();

            Assert.DoesNotContain(mockEmployee.Object, employeesList);
            Assert.True(mockEmployee.Object.IsDeleted);
        }
    }
}