using Hospital.Commands.ManageUsers;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManageUsersTests
{
    public class DisplayUsersCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListsStorage> mockListsStorage;

        private DisplayUsersCommand displayUsersCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListsStorage = new Mock<IListsStorage>();

            displayUsersCommand = new DisplayUsersCommand(
                mockMenuHandler.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenUsersListNotEmpty_ShouldDisplayUsersList()
        {
            SetUpMocks();

            displayUsersCommand.Execute();

            mockMenuHandler.Verify(x => x.DisplayList(It.IsAny<List<User>>()), Times.Once());
        }
    }
}