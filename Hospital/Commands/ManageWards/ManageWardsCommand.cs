using Hospital.Commands.LoginWindow;
using Hospital.Commands.Navigation;
using Hospital.Entities.Interfaces;
using Hospital.Enums;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageWards
{
    public class ManageWardsCommand : Command
    {
        private readonly Lazy<CreateWardCommand> _createWardCommand;
        private readonly Lazy<DisplayWardCommand> _displayWardCommand;
        private readonly Lazy<ChangeWardOwners> _changeWardOwners;
        private readonly Lazy<DeleteWardCommand> _deleteWardCommand;
        private readonly Lazy<BackCommand> _backCommand;
        private readonly INavigationService _navigationService;
        private readonly IMenuHandler _menuHandler;
        private Rank _currentUserRank;

        public ManageWardsCommand(
            Lazy<CreateWardCommand> createWardCommand,
            Lazy<DisplayWardCommand> displayWardCommand,
            Lazy<ChangeWardOwners> changeWardOwners,
            Lazy<DeleteWardCommand> deleteWardCommand,
            Lazy<BackCommand> backCommand,
            INavigationService navigationService,
            IMenuHandler menuHandler)
            : base(UiMessages.ManageWardsMessages.Introduce)
        {
            _createWardCommand = createWardCommand;
            _displayWardCommand = displayWardCommand;
            _changeWardOwners = changeWardOwners;
            _deleteWardCommand = deleteWardCommand;
            _backCommand = backCommand;
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
                case UiMessages.CreateWardMessages.Introduce:
                    _createWardCommand.Value.Execute();
                    return;
                case UiMessages.DisplayWardMessages.Introduce:
                    _displayWardCommand.Value.Execute();
                    return;
                case UiMessages.ChangeWardOwners.Introduce:
                    _changeWardOwners.Value.Execute();
                    return;
                case UiMessages.DeleteWardMessages.Introduce:
                    _deleteWardCommand.Value.Execute();
                    return;
                case UiMessages.BackCommandMessages.Introduce:
                    _backCommand.Value.Execute();
                    return;
            }
        }

        private List<IIntroduceString> GetAvailableCommands()
        {
            return _currentUserRank switch
            {
                Rank.Default => new List<IIntroduceString>
                {
                    _displayWardCommand.Value,
                    _backCommand.Value
                },
                Rank.Admin => new List<IIntroduceString>
                {
                    _createWardCommand.Value,
                    _displayWardCommand.Value,
                    _changeWardOwners.Value,
                    _deleteWardCommand.Value,
                    _backCommand.Value
                }
            };
        }
    }
}