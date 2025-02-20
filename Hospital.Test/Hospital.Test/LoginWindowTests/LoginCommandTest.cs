using Hospital.Commands.LoginWindow;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.LoginWindowTest
{
    public class LoginCommandTest
    {
        private Mock<IAuthenticationService> mockAuthenticationService;
        private Mock<IMenuHandler> mockMenuHandler;
        private Mock<IInputHandler> mockInputHandler;

        private LoginCommand loginCommand;

        private void SetUpMocks()
        {
            mockAuthenticationService = new Mock<IAuthenticationService>();
            mockMenuHandler = new Mock<IMenuHandler>();
            mockInputHandler = new Mock<IInputHandler>();

            loginCommand = new LoginCommand(
                mockAuthenticationService.Object,
                mockMenuHandler.Object,
                mockInputHandler.Object);
        }

        [Fact]
        public void Execute_WhenUserIsNull_ShouldReturnEarly()
        {
            SetUpMocks();

            mockAuthenticationService.Setup(x => x.GetUserByLogin(It.IsAny<string>()))
                                     .Returns((User)null);

            loginCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.LoginCommandMessages.CantFindLoginPrompt), Times.Once());
        }


        [Fact]
        public void Execute_WhenPasswordIsInncorect_ShouldReturnEarly()
        {
            SetUpMocks();

            var mockUser = new Mock<User>();

            mockAuthenticationService.Setup(x => x.GetUserByLogin(mockUser.Object.Login))
                                     .Returns(mockUser.Object);
            mockAuthenticationService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                                     .Returns(false);

            loginCommand.Execute();

            mockMenuHandler.Verify(x => x.ShowMessage(UiMessages.LoginCommandMessages.WrongPasswordPrompt), Times.Once());
        }


        [Fact]
        public void Execute_WhenUserInputIsValid_ShouldSetVariableToTrue()
        {
            SetUpMocks();

            var mockUser = new Mock<User>();

            mockAuthenticationService.Setup(x => x.GetUserByLogin(mockUser.Object.Login))
                                     .Returns(mockUser.Object);
            mockAuthenticationService.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
                                     .Returns(true);

            loginCommand.Execute();

            Assert.True(loginCommand.IsLoggedIn);
        }
    }
}