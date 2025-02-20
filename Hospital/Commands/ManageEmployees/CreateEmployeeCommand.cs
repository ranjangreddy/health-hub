using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageEmployees
{
    public class CreateEmployeeCommand : Command
    {
        private readonly IObjectsFactory _objectsFactory;
        private readonly IDTOFactory _dtoFactory;
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IValidateObjects _validateObjects;
        private readonly IListsStorage _listsStorage;

        public CreateEmployeeCommand(
            IObjectsFactory objectsFactory,
            IDTOFactory dtoFactory,
            IMenuHandler menuHandler,
            IListManage listManage,
            IValidateObjects validateObjects,
            IListsStorage listsStorage)
            : base(UiMessages.CreateEmployeeMessages.Introduce)
        {
            _objectsFactory = objectsFactory;
            _dtoFactory = dtoFactory;
            _menuHandler = menuHandler;
            _listManage = listManage;
            _validateObjects = validateObjects;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            if (!_listsStorage.Wards.Any())
            {
                _menuHandler.ShowMessage(UiMessages.FactoryMessages.NoWardErrorPrompt);
                return;
            }

            var employeeDTO = _dtoFactory.GatherEmployeeData(_listsStorage.Wards);
            if (!_validateObjects.ValidateEmployeeObject(employeeDTO))
            {
                return;
            }

            var employee = _objectsFactory.CreateEmployee(employeeDTO);
            _listManage.Add(employee, _listsStorage.Employees);

            _menuHandler.ShowMessage(string.Format(UiMessages.CreateEmployeeMessages.OperationSuccessPrompt,
                employee.Position, employee.Name, employee.Surname));
        }
    }
}