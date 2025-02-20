using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.Utilities.Interfaces
{
    public interface IManageCapacity
    {
        void UpdateWardCapacity(Ward ward, Patient patient, Operation operation);
    }
}