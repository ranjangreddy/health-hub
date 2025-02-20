using Hospital.Enums;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;
using Hospital.Utilities.UserInterface.Interfaces;

namespace Hospital.Commands.ManagePatients.ManagePatient
{
    public class AssignToDoctorCommand : Command
    {
        private readonly IMenuHandler _menuHandler;
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public AssignToDoctorCommand(
            IMenuHandler menuHandler,
            IListManage listManage,
            IListsStorage listsStorage)
            : base(UiMessages.AssignToDoctorMessages.Introduce)
        {
            _menuHandler = menuHandler;
            _listManage = listManage;
            _listsStorage = listsStorage;
        }

        public override void Execute()
        {
            if (!_listsStorage.Patients.Any())
            {
                _menuHandler.ShowMessage(UiMessages.DisplayPatientsMessages.NoPatientsPrompt);
                return;
            }

            var doctors = _listsStorage.Employees.Where(e => e.Position == Position.Doctor).ToList();
            if (!doctors.Any())
            {
                _menuHandler.ShowMessage(UiMessages.AssignToDoctorMessages.NoDoctorsPrompt);
                return;
            }

            var patient = _menuHandler.SelectObject(_listsStorage.Patients,
                UiMessages.AssignToDoctorMessages.SelectPatientPrompt);
            var doctor = _menuHandler.SelectObject(doctors,
                UiMessages.AssignToDoctorMessages.SelectDoctorPrompt);

            if (patient.AssignedWard != doctor.AssignedWard)
            {
                _menuHandler.ShowMessage(UiMessages.AssignToDoctorMessages.WrongWardPrompt);
                return;
            }

            patient.AssignedDoctor = doctor;
            _listManage.Update(patient, _listsStorage.Patients);

            _menuHandler.ShowMessage(string.Format(UiMessages.AssignToDoctorMessages.OperationSuccessPrompt,
                UiMessages.DoctorObjectMessages.Position, doctor.Surname, patient.Name, patient.Surname));
        }
    }
}