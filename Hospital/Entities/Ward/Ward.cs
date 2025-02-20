using Hospital.Entities.Employee;
using Hospital.Entities.Interfaces;
using Hospital.PeopleCategories.PatientClass;
using Hospital.PeopleCategories.UserClass;
using Hospital.Utilities.UserInterface;

namespace Hospital.PeopleCategories.WardClass
{
    public class Ward : IIntroduceString, IIdentifier, IIsDeleted
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual int Capacity { get; set; }
        public virtual IList<Patient> AssignedPatients { get; set; }
        public virtual IList<Employee> AssignedEmployees { get; set; }
        public virtual IList<User> AssignedUsers { get; set; }
        public virtual bool IsDeleted { get; set; }
        private int? _patientsNumber;
        public virtual int PatientsNumber
        {
            get => _patientsNumber ?? AssignedPatients?.Count ?? 0;
            set => _patientsNumber = value;
        }
        public virtual string IntroduceString { get; set; }

        protected Ward() { }

        public Ward(
            string name,
            int capacity,
            IList<Patient> assignedPatients,
            IList<Employee> assignedEmployees,
            IList<User> assignedUsers)
        {
            Name = name;
            Capacity = capacity;
            AssignedPatients = assignedPatients;
            AssignedEmployees = assignedEmployees;
            AssignedUsers = assignedUsers;
            IsDeleted = false;
            IntroduceString = string.Format(
                UiMessages.WardObjectMessages.Introduce, name);
        }
    }
}