using Hospital.Commands.LoginWindow;
using Hospital.Commands.ManageWards;
using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManageWardsTests
{
    public class DisplayWardCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListsStorage> mockListsStorage;

        private DisplayWardCommand displayWardCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListsStorage = new Mock<IListsStorage>();

            displayWardCommand = new DisplayWardCommand(
                mockMenuHandler.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenWardsListEmpty_ShouldReturnEarly()
        {
            //Arrange
            SetUpMocks();

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([]);

            //Act
            displayWardCommand.Execute();

            //Assert
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayWardMessages.NoWardPrompt), Times.Once());
            mockMenuHandler.Verify(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenUserDontHaveAnyAssignedWards_ShouldReturnEarly()
        {
            //Arrange
            SetUpMocks();

            var mockUser = Mock.Of<User>();
            mockUser.AssignedWards = new List<Ward>();

            LoginCommand.CurrentlyLoggedIn = mockUser;

            mockListsStorage.Setup(x => x.Wards)
                            .Returns([It.IsAny<Ward>()]);

            //Act
            displayWardCommand.Execute();

            //Assert
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayWardMessages.NoWardAssignedToUser), Times.Once());
            mockMenuHandler.Verify(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public void Execute_WhenWardsListNotEmpty_ShouldDisplayPatientsList()
        {
            // Arrange
            SetUpMocks();

            var mockUser = Mock.Of<User>();
            LoginCommand.CurrentlyLoggedIn = mockUser;

            var ward = new Ward("ward", 10, new List<Patient>(), new List<Employee>(), new List<User> { mockUser });
            mockUser.AssignedWards = new List<Ward> { ward };

            var expectedString = string.Format(
                UiMessages.DisplayWardMessages.DisplayInformationPrompt,
                ward.Name,
                ward.PatientsNumber,
                ward.Capacity,
                ward.PatientsNumber / ward.Capacity,
                ward.AssignedEmployees.Count
            );

            mockListsStorage.Setup(x => x.Wards)
                            .Returns(new List<Ward> { ward });

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<Ward>>(), It.IsAny<string>()))
                           .Returns(ward);

            // Act
            displayWardCommand.Execute();

            // Assert
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DisplayWardMessages.NoWardPrompt), Times.Never());
            mockMenuHandler.Verify(x => x.ShowMessage(expectedString), Times.Once());
        }
    }
}