using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.Utilities.UserInterface.Interfaces
{
    public interface IDTOFactory
    {
        PersonDTO GatherPersonData();
        EmployeeDTO GatherEmployeeData(List<Ward> wards);
        PatientDTO GatherPatientData(List<Ward> wards);
        UserDTO GatherUserData();
        WardDTO GatherWardData();
    }
}