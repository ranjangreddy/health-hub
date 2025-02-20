using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.Utilities.EntitiesFactory.Interfaces
{
    public interface IObjectsFactory
    {
        Patient CreatePatient(PatientDTO dto);
        Employee CreateEmployee(EmployeeDTO dto);
        User CreateUser(UserDTO dto);
        Ward CreateWard(WardDTO dto);
    }
}