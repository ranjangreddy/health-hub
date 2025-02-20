using Hospital.Commands.LoginWindow;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.UserInterface.Interfaces;
using Moq;

namespace Hospital.Test.LoginWindowTest
{
    public class LogoutCommandTest
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
        public void LogoutCommand_SuccessLogout()
        {
            SetUpMocks();

            var logoutCommand = new LogoutCommand(loginCommand);

            logoutCommand.Execute();

            Assert.False(loginCommand.IsLoggedIn);
        }
    }
}