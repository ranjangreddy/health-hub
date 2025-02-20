using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.Interfaces;
using Hospital.Utilities.ListManagement.Interfaces;
using Hospital.Utilities.UserInterface;

namespace Hospital.Utilities
{
    public class ManageCapacity : IManageCapacity
    {
        private readonly IListManage _listManage;
        private readonly IListsStorage _listsStorage;

        public ManageCapacity(IListManage listManage, IListsStorage listStorage)
        {
            _listManage = listManage;
            _listsStorage = listStorage;
        }

        public void UpdateWardCapacity(Ward ward, Patient patient, Operation operation)
        {
            switch (operation)
            {
                case Operation.Add:
                    ward.AssignedPatients.Add(patient);
                    ward.PatientsNumber++;
                    break;
                case Operation.Remove:
                    ward.AssignedPatients.Remove(patient);
                    ward.PatientsNumber--;
                    break;
            }

            ward.IntroduceString = string.Format(UiMessages.WardObjectMessages.Introduce, ward.Name, ward.PatientsNumber, ward.Capacity);
            _listManage.Update(ward, _listsStorage.Wards);
        }
    }
}