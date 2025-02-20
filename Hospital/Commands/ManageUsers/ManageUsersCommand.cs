using Hospital.Commands.Navigation;
using Hospital.Entities.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageUsers
{
    internal class ManageUsersCommand : Command
    {
        private readonly Lazy<CreateUserCommand> _createUserCommand;
        private readonly Lazy<ChangeUserRankCommand> _changeUserRankCommand;
        private readonly Lazy<DisplayUsersCommand> _displayUsersCommand;
        private readonly Lazy<DeleteUserCommand> _deleteUserCommand;
        private readonly Lazy<BackCommand> _backCommand;
        private readonly INavigationService _navigationService;
        private readonly IMenuHandler _menuHandler;

        public ManageUsersCommand(
            Lazy<CreateUserCommand> createUserCommand,
            Lazy<ChangeUserRankCommand> changeUserRankCommand,
            Lazy<DisplayUsersCommand> displayUsersCommand,
            Lazy<DeleteUserCommand> deleteUserCommand,
            Lazy<BackCommand> backCommand,
            INavigationService navigationService,
            IMenuHandler menuHandler)
            : base(UiMessages.ManageUsersMessages.Introduce)
        {
            _createUserCommand = createUserCommand;
            _changeUserRankCommand = changeUserRankCommand;
            _displayUsersCommand = displayUsersCommand;
            _deleteUserCommand = deleteUserCommand;
            _backCommand = backCommand;
            _navigationService = navigationService;
            _menuHandler = menuHandler;
        }

        public override void Execute()
        {
            var commands = new List<IIntroduceString>
            {
                _createUserCommand.Value,
                _changeUserRankCommand.Value,
                _displayUsersCommand.Value,
                _deleteUserCommand.Value,
                _backCommand.Value
            };
            var selectedCommand = _menuHandler.ShowInteractiveMenu(commands);

            _navigationService.Queue((Command)selectedCommand);

            switch (selectedCommand.IntroduceString)
            {
                case UiMessages.CreateUserCommandMessages.Introduce:
                    _createUserCommand.Value.Execute();
                    return;
                case UiMessages.ChangeUserRankMessages.Introduce:
                    _changeUserRankCommand.Value.Execute();
                    return;
                case UiMessages.DisplayUsersCommandMessages.Introduce:
                    _displayUsersCommand.Value.Execute();
                    return;
                case UiMessages.DeleteUserMessages.Introduce:
                    _deleteUserCommand.Value.Execute();
                    return;
                case UiMessages.BackCommandMessages.Introduce:
                    _backCommand.Value.Execute();
                    return;
            }
        }
    }
}