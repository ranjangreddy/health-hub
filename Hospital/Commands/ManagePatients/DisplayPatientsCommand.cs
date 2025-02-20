using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManagePatients
{
    public class DisplayPatientsCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListsStorage _listsStorage;

        public DisplayPatientsCommand(
            IMenuHandler menuHandler,
            IListsStorage listsStorage)
            : base(UiMessages.DisplayPatientsMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            if (!_listsStorage.Patients.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt);
                return;
            }

            _menuHandler.DisplayList(_listsStorage.Patients);
        }
    }
}