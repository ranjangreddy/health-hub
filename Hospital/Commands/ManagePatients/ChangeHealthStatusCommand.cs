using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManagePatients.ManagePatient
{
    public class ChangeHealthStatusCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public ChangeHealthStatusCommand(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.ChangeHealthStatusMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            List<Patient> patientsList = _listsStorage.Patients;

            if (!patientsList.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt);
                return;
            }

            Patient patient = _menuHandler.SelectObject(patientsList,
                UiMessages.ChangeHealthStatusMessages.SelectPatientPrompt);
            patient.HealthStatus = _menuHandler.ShowInteractiveMenu<Health>();

            _listManage.Update(patient, patientsList);

            _menuHandler.ShowMessage(string.Format(UiMessages.ChangeHealthStatusMessages.OperationSuccessPrompt,
                patient.Name, patient.Surname));
        }
    }
}