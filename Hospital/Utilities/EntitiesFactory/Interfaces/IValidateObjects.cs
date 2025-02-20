using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.Utilities.EntitiesFactory.Interfaces
{
    public interface IValidateObjects
    {
        bool ValidatePatientObject(PatientDTO dto);
        bool ValidateEmployeeObject(EmployeeDTO dto);
        bool ValidateUserObject(UserDTO dto);
        bool ValidateWardObject(WardDTO dto);
    }
}
