using Hospital.Enums;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.PersonClass;
using Hospital.PeopleCategories.WardClass;

namespace Hospital.Entities.Employee
{
    public class EmployeeDTO : PersonDTO
    {
        public EmployeeDTO(PersonDTO person)
        {
            Name = person.Name;
            Surname = person.Surname;
            Gender = person.Gender;
            Birthday = person.Birthday;
        }

        public Ward AssignedWard { get; set; }

        public Position Position { get; set; }

        public IList<Patient> AssignedPatients { get; set; }
    }
}