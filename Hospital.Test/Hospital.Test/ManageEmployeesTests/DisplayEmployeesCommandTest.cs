using Hospital.Commands.ManageEmployees;
using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManageEmployeesTests
{
    public class DisplayEmployeesCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListsStorage> mockListsStorage;

        private DisplayEmployeesCommand displayEmployeesCommand;

        private void SetUpMocks()
        {
            mockListsStorage = new Mock<IListsStorage>();
            mockMenuHandler = new Mock<IMenuHandler>();

            displayEmployeesCommand = new DisplayEmployeesCommand(
                mockMenuHandler.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenNoEmployee_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.SetupGet(x => x.Employees)
                            .Returns([]);

            displayEmployeesCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayEmployeesMessages.NoEmployeesPrompt), Times.Once());
            mockMenuHandler.Verify(m => m.DisplayList(It.IsAny<List<Patient>>()), Times.Never());

        }

        [Fact]
        public void Execute_WhenIsEmployee_ShouldDisplayEmployees()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Employees)
                            .Returns([It.IsAny<Employee>()]);

            displayEmployeesCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayEmployeesMessages.NoEmployeesPrompt), Times.Never());
            mockMenuHandler.Verify(x => x.DisplayList(It.IsAny<List<Employee>>()), Times.Once());
        }
    }
}