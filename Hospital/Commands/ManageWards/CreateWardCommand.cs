using Hospital.Commands.LoginWindow;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManageWards
{
    public class CreateWardCommand : Command
    {
        private readonly IObjectsFactory _objectsFactory;
        private readonly IValidateObjects _validateObjects;
        private readonly IDTOFactory _dtoFactory;
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public CreateWardCommand(
            IObjectsFactory objectsFactory,
            IValidateObjects validateObjects,
            IDTOFactory dtoFactory,
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.CreateWardMessages.Introduce)
        {
            _objectsFactory = objectsFactory;
            _validateObjects = validateObjects;
            _dtoFactory = dtoFactory;
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            WardDTO wardDTO = _dtoFactory.GatherWardData();

            if (!_validateObjects.ValidateWardObject(wardDTO))
            {
                return;
            }

            Ward ward = _objectsFactory.CreateWard(wardDTO);
            ward.AssignedUsers.Add(LoginCommand.CurrentlyLoggedIn);
            _listManage.Add(ward, _listsStorage.Wards);

            _menuHandler.ShowMessage(string.Format(UiMessages.CreateWardMessages.OperationSuccessPrompt,
                ward.Name));
        }
    }
}