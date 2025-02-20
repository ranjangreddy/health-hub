using Hospital.Enums;
using Hospital.Utilities.EntitiesFactory.Interfaces;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManagePatients
{
    public class CreatePatientCommand : Command
    {
        private readonly IObjectsFactory _objectsFactory;
        private readonly IValidateObjects _validateObjects;
        private readonly IDTOFactory _dtoFactory;
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;
        private readonly IManageCapacity _manageCapacity;

        public CreatePatientCommand(
            IObjectsFactory objectsFactory,
            IValidateObjects validateObjects,
            IDTOFactory dtoFactory,
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage,
            IManageCapacity manageCapacity)
            : base(UiMessages.CreatePatientMessages.Introduce)
        {
            _objectsFactory = objectsFactory;
            _validateObjects = validateObjects;
            _dtoFactory = dtoFactory;
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
            _manageCapacity = manageCapacity;
        }

        public override void Execute()
        {
            if (!_listsStorage.Wards.Any())
            {
                _menuHandler.ShowMessage(UiMessages.CreatePatientMessages.NoWardErrorPrompt);
                return;
            }

            var patientDTO = _dtoFactory.GatherPatientData(_listsStorage.Wards);
            if (!_validateObjects.ValidatePatientObject(patientDTO))
            {
                return;
            }

            var createdPatient = _objectsFactory.CreatePatient(patientDTO);

            _manageCapacity.UpdateWardCapacity(createdPatient.AssignedWard, createdPatient,
                Operation.Add);

            _listManage.Add(createdPatient, _listsStorage.Patients);

            _menuHandler.ShowMessage(string.Format(UiMessages.CreatePatientMessages.OperationSuccessPrompt,
                createdPatient.Name, createdPatient.Surname));
        }
    }
}