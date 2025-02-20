using Hospital.Enums;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManagePatients.ManagePatient
{
    public class DeletePatientCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;
        private readonly IManageCapacity _manageCapacity;

        public DeletePatientCommand(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage,
            IManageCapacity manageCapacity)
            : base(UiMessages.DeletePatientMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
            _manageCapacity = manageCapacity;
        }

        public override void Execute()
        {
            if (!_listsStorage.Patients.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt);
                return;
            }

            var selectedPatient = _menuHandler.SelectObject(
                _listsStorage.Patients, UiMessages.DeletePatientMessages.DeletePrompt);

            _listManage.SoftDelete(selectedPatient, _listsStorage.Patients);

            _manageCapacity.UpdateWardCapacity(selectedPatient.AssignedWard, selectedPatient,
                Operation.Remove);

            _menuHandler.ShowMessage(string.Format(UiMessages.DeletePatientMessages.OperationSuccessPrompt,
                selectedPatient.Name, selectedPatient.Surname));
        }
    }
}