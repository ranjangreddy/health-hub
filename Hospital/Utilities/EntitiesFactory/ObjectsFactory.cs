using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.PeopleCategories.WardClass;
using Hospital.Utilities.EntitiesFactory.Interfaces;

namespace Hospital.Utilities.EntitiesFactory
{
    public class ObjectsFactory : IObjectsFactory
    {
        public Patient CreatePatient(PatientDTO dto)
        {
            return new Patient(
                dto.Name,
                dto.Surname,
                dto.Gender,
                dto.Birthday,
                dto.PersonalIdNumber,
                dto.AssignedWard);
        }

        public Employee CreateEmployee(EmployeeDTO dto)
        {
            return new Employee(
                dto.Name,
                dto.Surname,
                dto.Gender,
                dto.Birthday,
                dto.AssignedWard,
                dto.Position,
                dto.AssignedPatients);
        }

        public User CreateUser(UserDTO dto)
        {
            return new User(
                dto.Name,
                dto.Surname,
                dto.Gender,
                dto.Birthday,
                dto.Login,
                dto.Password,
                dto.Rank,
                dto.AssignedWards);
        }

        public Ward CreateWard(WardDTO dto)
        {
            return new Ward(
                dto.Name,
                dto.Capacity,
                dto.AssignedPatients,
                dto.AssignedEmployees,
                dto.AssignedUsers);
        }
    }
}