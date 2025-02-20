using Hospital.Commands.LoginWindow;
using Hospital.Commands.ManageEmployees;
using Hospital.Commands.ManagePatients;
using Hospital.Commands.ManageUsers;
using Hospital.Commands.ManageWards;
using Hospital.Commands.Navigation;
using Hospital.Entities.Interfaces;
using Hospital.Enums;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands
{
    internal class MainWindowCommand : Command
    {
        private readonly Lazy<ManagePatientsCommand> _managePatientsCommand;
        private readonly Lazy<ManageEmployeesCommand> _manageEmployeesCommand;
        private readonly Lazy<ManageWardsCommand> _manageWardsCommand;
        private readonly Lazy<ManageUsersCommand> _manageUsersCommand;
        private readonly Lazy<LogoutCommand> _logoutCommand;
        private readonly INavigationService _navigationService;
        private readonly IMenuHandler _menuHandler;
        private Rank _currentUserRank;

        public MainWindowCommand(
            Lazy<ManagePatientsCommand> managePatientsCommand,
            Lazy<ManageEmployeesCommand> manageEmployeesCommand,
            Lazy<ManageWardsCommand> manageWardsCommand,
            Lazy<ManageUsersCommand> manageUsersCommand,
            Lazy<LogoutCommand> logoutCommand,
            INavigationService navigationService,
            IMenuHandler menuHandler)
            : base(UiMessages.MainWindowMessages.Introduce)
        {
            _managePatientsCommand = managePatientsCommand;
            _manageEmployeesCommand = manageEmployeesCommand;
            _manageWardsCommand = manageWardsCommand;
            _manageUsersCommand = manageUsersCommand;
            _logoutCommand = logoutCommand;
            _navigationService = navigationService;
            _menuHandler = menuHandler;
        }

        public override void Execute()
        {
            _currentUserRank = LoginCommand.CurrentlyLoggedIn.Rank;
            var commands = GetAvailableCommands();
            var selectedCommand = _menuHandler.ShowInteractiveMenu(commands);

            _navigationService.Queue((Command)selectedCommand);

            switch (selectedCommand.IntroduceString)
            {
                case UiMessages.ManagePatientsMessages.Introduce:
                    _managePatientsCommand.Value.Execute();
                    return;
                case UiMessages.ManageEmployeesMessages.Introduce:
                    _manageEmployeesCommand.Value.Execute();
                    return;
                case UiMessages.ManageWardsMessages.Introduce:
                    _manageWardsCommand.Value.Execute();
                    return;
                case UiMessages.ManageUsersMessages.Introduce:
                    _manageUsersCommand.Value.Execute();
                    return;
                case UiMessages.LogoutCommandMessages.Introduce:
                    _logoutCommand.Value.Execute();
                    return;
            }
        }

        private List<IIntroduceString> GetAvailableCommands()
        {
            return _currentUserRank switch
            {
                Rank.Default => new List<IIntroduceString>
                {
                    _managePatientsCommand.Value,
                    _manageEmployeesCommand.Value,
                    _manageWardsCommand.Value,
                    _logoutCommand.Value
                },
                Rank.Admin => new List<IIntroduceString>
                {
                    _managePatientsCommand.Value,
                    _manageEmployeesCommand.Value,
                    _manageWardsCommand.Value,
                    _manageUsersCommand.Value,
                    _logoutCommand.Value
                }
            };
        }
    }
}