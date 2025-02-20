using Hospital.Utilities.UserInterface;

namespace Hospital.Commands.LoginWindow
{
    public class LogoutCommand : Command
    {
        private readonly LoginCommand _loginCommand;

        public LogoutCommand(
            LoginCommand loginCommand)
            : base(UiMessages.LogoutCommandMessages.Introduce)
        {
            _loginCommand = loginCommand;
        }

        public override void Execute()
        {
            _loginCommand.IsLoggedIn = false;
            return;
        }
    }
}