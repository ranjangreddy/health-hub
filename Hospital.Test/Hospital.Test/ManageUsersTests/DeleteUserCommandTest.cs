using Hospital.Commands.LoginWindow;
using Hospital.Commands.ManageUsers;
using Hospital.Database.Interfaces;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;
using NHibernate;

namespace Hospital.Test.ManageUsers
{
    public class DeleteUserCommandTest
    {
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListManage> mockListManage;
        private Mock<IListsStorage> mockListsStorage;
        private Mock<IDatabaseOperations> mockDatabaseOperations;

        private DeleteUserCommand deleteUserCommand;

        private void SetUpMocks()
        {
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListManage = new Mock<IListManage>();
            mockListsStorage = new Mock<IListsStorage>();
            mockDatabaseOperations = new Mock<IDatabaseOperations>();


            deleteUserCommand = new DeleteUserCommand(
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenUsersListEmpty_ShouldReturnEarly()
        {
            SetUpMocks();

            mockListsStorage.Setup(x => x.Users)
                            .Returns([]);

            deleteUserCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.DeleteUserMessages.NoUserPrompt), Times.Once());
            mockMenuHandler.Verify(x => x.SelectObject(It.IsAny<List<User>>(), UiMessages.DeleteUserMessages.SelectUserPrompt), Times.Never());
        }

        [Fact]
        public void Execute_WhenUserToDeleteIsTheSameAsCurrentlyLoggedIn_ShouldReturnEarly()
        {
            SetUpMocks();

            var mockUser = new Mock<User>();
            mockUser.SetupAllProperties();
            mockUser.Object.IsDeleted = false;

            var usersList = new List<User>() { mockUser.Object };

            LoginCommand.CurrentlyLoggedIn = mockUser.Object;

            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<User>>(), It.IsAny<string>()))
                           .Returns(mockUser.Object);

            deleteUserCommand.Execute();

            mockListManage.Verify(x => x.SoftDelete(It.IsAny<User>(), It.IsAny<List<User>>()), Times.Never());
            Assert.Contains(mockUser.Object, usersList);
            Assert.False(mockUser.Object.IsDeleted);
        }

        [Fact]
        public void Execute_WhenUsersListNotEmpty_ShouldRemoveUser()
        {
            SetUpMocks();

            var mockUser = new Mock<User>();
            mockUser.SetupAllProperties();
            mockUser.Object.IsDeleted = false;

            var usersList = new List<User>() { mockUser.Object };

            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);

            mockMenuHandler.Setup(x => x.SelectObject(It.IsAny<List<User>>(), It.IsAny<string>()))
                           .Returns(mockUser.Object);

            mockDatabaseOperations.Setup(x => x.Update(It.IsAny<User>(), It.IsAny<ISession>()))
                                  .Returns(true);

            mockListManage.Setup(x => x.SoftDelete(It.IsAny<User>(), It.IsAny<List<User>>()))
                          .Callback((User user, List<User> list) =>
                          {
                              user.IsDeleted = true;
                              list.Remove(user);
                          });

            deleteUserCommand.Execute();

            Assert.DoesNotContain(mockUser.Object, usersList);
            Assert.True(mockUser.Object.IsDeleted);
        }
    }
}