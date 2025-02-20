using Hospital.Commands.LoginWindow;
using Hospital.Commands.ManagePatients.ManagePatient;
using Hospital.Commands.Navigation;
using Hospital.Entities.Interfaces;
using Hospital.Enums;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManagePatients
{
    public class ManagePatientsCommand : Command
    {
        private readonly Lazy<CreatePatientCommand> _createPatientCommand;
        private readonly Lazy<DisplayPatientsCommand> _displayPatientsCommand;
        private readonly Lazy<AssignToDoctorCommand> _assignToDoctorCommand;
        private readonly Lazy<ChangeHealthStatusCommand> _changeHealthStatusCommand;
        private readonly Lazy<DeletePatientCommand> _deletePatientCommand;
        private readonly Lazy<BackCommand> _backCommand;
        private readonly INavigationService _navigationService;
        private readonly IMenuHandler _menuHandler;
        private Rank _currentUserRank;

        public ManagePatientsCommand(
            Lazy<CreatePatientCommand> createPatientCommand,
            Lazy<DisplayPatientsCommand> displayPatientsCommand,
            Lazy<AssignToDoctorCommand> assignToDoctorCommand,
            Lazy<ChangeHealthStatusCommand> changeHealthStatusCommand,
            Lazy<DeletePatientCommand> deletePatientCommand,
            Lazy<BackCommand> backCommand,
            INavigationService navigationService,
            IMenuHandler menuHandler)
            : base(UiMessages.ManagePatientsMessages.Introduce)
        {
            _createPatientCommand = createPatientCommand;
            _displayPatientsCommand = displayPatientsCommand;
            _assignToDoctorCommand = assignToDoctorCommand;
            _changeHealthStatusCommand = changeHealthStatusCommand;
            _deletePatientCommand = deletePatientCommand;
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
                case UiMessages.CreatePatientMessages.Introduce:
                    _createPatientCommand.Value.Execute();
                    return;
                case UiMessages.DisplayPatientsMessages.Introduce:
                    _displayPatientsCommand.Value.Execute();
                    return;
                case UiMessages.AssignToDoctorMessages.Introduce:
                    _assignToDoctorCommand.Value.Execute();
                    return;
                case UiMessages.ChangeHealthStatusMessages.Introduce:
                    _changeHealthStatusCommand.Value.Execute();
                    return;
                case UiMessages.DeletePatientMessages.Introduce:
                    _deletePatientCommand.Value.Execute();
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
                    _displayPatientsCommand.Value,
                    _assignToDoctorCommand.Value,
                    _changeHealthStatusCommand.Value,
                    _backCommand.Value
                },
                Rank.Admin => new List<IIntroduceString>
                {
                    _createPatientCommand.Value,
                    _displayPatientsCommand.Value,
                    _assignToDoctorCommand.Value,
                    _changeHealthStatusCommand.Value,
                    _deletePatientCommand.Value,
                    _backCommand.Value
                }
            };
        }
    }
}