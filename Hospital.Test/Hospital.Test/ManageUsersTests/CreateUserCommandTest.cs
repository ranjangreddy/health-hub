using Hospital.Commands.ManageUsers;
using Hospital.Database.Interfaces;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;
using NHibernate;

namespace Hospital.Test.ManageUsers
{
    public class CreateUserCommandTest
    {
        private Mock<IObjectsFactory> mockObjectsFactory;
        private Mock<IValidateObjects> mockValidateObjects;
        private Mock<IDTOFactory> mockDTOFactory;
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IListsStorage> mockListsStorage;
        private Mock<IListManage> mockListManage;
        private Mock<IDatabaseOperations> mockDatabaseOperations;

        private CreateUserCommand createAccountCommand;

        private void SetUpMocks()
        {
            mockObjectsFactory = new Mock<IObjectsFactory>();
            mockValidateObjects = new Mock<IValidateObjects>();
            mockDTOFactory = new Mock<IDTOFactory>();
            mockMenuHandler = new Mock<IMenuHandler>();
            mockListsStorage = new Mock<IListsStorage>();
            mockListManage = new Mock<IListManage>();
            mockDatabaseOperations = new Mock<IDatabaseOperations>();

            createAccountCommand = new CreateUserCommand(
                mockObjectsFactory.Object,
                mockValidateObjects.Object,
                mockDTOFactory.Object,
                mockMenuHandler.Object,
                mockListManage.Object,
                mockListsStorage.Object);
        }

        [Fact]
        public void Execute_WhenAccountDataNotPassValidation_ShouldReturnEarly()
        {
            SetUpMocks();

            mockValidateObjects.Setup(x => x.ValidateUserObject(It.IsAny<UserDTO>()))
                               .Returns(false);

            createAccountCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(It.Is<string>(s => s.Contains(UiMessages.CreateUserCommandMessages.CreatedAccountPrompt))), Times.Never());
        }

        [Fact]
        public void Execute_WhenValidationPassed_ShouldCreatesAccount()
        {
            SetUpMocks();

            var mockUser = new Mock<User>();
            var usersList = new List<User>();

            mockValidateObjects.Setup(x => x.ValidateUserObject(It.IsAny<UserDTO>()))
                               .Returns(true);

            mockObjectsFactory.Setup(x => x.CreateUser(It.IsAny<UserDTO>()))
                              .Returns(mockUser.Object);

            mockListsStorage.Setup(x => x.Users)
                            .Returns(usersList);

            mockDatabaseOperations.Setup(x => x.Add(It.IsAny<User>(), It.IsAny<ISession>()))
                                  .Returns(true);

            mockListManage.Setup(x => x.Add(It.IsAny<User>(), usersList))
                          .Callback((User item, List<User> list) =>
                          {
                              if (mockDatabaseOperations.Object.Add(item, new Mock<ISession>().Object))
                              {
                                  list.Add(item);
                              }
                          });

            createAccountCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(string.Format(UiMessages.CreateUserCommandMessages.CreatedAccountPrompt, mockUser.Object.Login)), Times.Once());
            Assert.Contains(mockUser.Object, usersList);
        }
    }
}