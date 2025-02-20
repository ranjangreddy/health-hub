using Hospital.Commands.ManageUsers;
using Hospital.Enums;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.ManageUsersTests
{
    public class ChangeUserRankCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListStorage;

        private ChangeUserRankCommand changeUserRankCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListStorage = new Mock<IListsStorage>();

            changeUserRankCommand = new ChangeUserRankCommand(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListStorage.Object);
        }

        [Fact]
        public void Execute_WhenUsersListIsEmpty_ShouldReturnEarly()
        {
            //Arrange
            SetUpMocks();

            mockListStorage.Setup(x => x.Users)
                           .Returns([]);

            //Act
            changeUserRankCommand.Execute();

            //Assert
            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.ChangeUserRankMessages.NoUsersPrompt), Times.Once());
        }

        [Fact]
        public void Execute_WhenUsersListIsNotEmpty_ShouldChangeRankOfUser()
        {
            //Arrange
            SetUpMocks();

            var mockUser = Mock.Of<User>();
            var usersList = new List<User> { mockUser };
            var expectedRank = Rank.Admin;

            mockListStorage.Setup(x => x.Users)
                           .Returns(usersList);

            mockMenuHandler.Setup(x => x.SelectObject(usersList, It.IsAny<string>()))
                           .Returns(mockUser);
            mockMenuHandler.Setup(x => x.ShowInteractiveMenu<Rank>())
                           .Returns(expectedRank);

            //Act
            changeUserRankCommand.Execute();

            //Assert
            Assert.True(mockUser.Rank == expectedRank);
        }
    }
}