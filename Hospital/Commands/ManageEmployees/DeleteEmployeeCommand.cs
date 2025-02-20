using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageEmployees
{
    public class DeleteEmployeeCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public DeleteEmployeeCommand(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.DeleteEmployeeMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            if (!_listsStorage.Employees.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DeleteEmployeeMessages.NoEmployeesPrompt);
                return;
            }

            var selectedEmployee = _menuHandler.SelectObject(
                _listsStorage.Employees, UiMessages.DeleteEmployeeMessages.SelectPrompt);
            _listManage.SoftDelete(selectedEmployee, _listsStorage.Employees);

            _menuHandler.ShowMessage(string.Format(UiMessages.DeleteEmployeeMessages.OperationSuccessPrompt,
                selectedEmployee.Position, selectedEmployee.Name, selectedEmployee.Surname));
        }
    }
}