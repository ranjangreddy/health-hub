using Hospital.Entities.Employee;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;

namespace Hospital.PeopleCategories.WardClass
{
    public class WardDTO
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public IList<Patient> AssignedPatients { get; set; }
        public IList<Employee> AssignedEmployees { get; set; }
        public IList<User> AssignedUsers { get; set; }
    }
}