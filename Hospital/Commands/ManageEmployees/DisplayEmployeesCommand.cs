using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageEmployees
{
    public class DisplayEmployeesCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListsStorage _listsStorage;

        public DisplayEmployeesCommand(
            IMenuHandler menuHandler,
            IListsStorage listsStorage)
            : base(UiMessages.DisplayEmployeesMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            if (!_listsStorage.Employees.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayEmployeesMessages.NoEmployeesPrompt);
                return;
            }

            _menuHandler.DisplayList(_listsStorage.Employees);
        }
    }
}